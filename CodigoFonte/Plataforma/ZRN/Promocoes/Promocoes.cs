using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class Promocoes
    {

        public string _server = "";
        private string _indexElastic = "";
        private ZBD.Model.zeengEntities _bd;

        public Promocoes(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        public Promocoes()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public Promocoes(ZBD.Model.zeengEntities bd)
        {
            _bd = bd;
        }

        public List<Listas.Item> RetornaItensModalidades()
        {

            var modalidades = (from modalidade in _bd.promo_modalidades
                               where modalidade.excluido == false
                               select new Listas.Item()
                               {
                                   id = modalidade.idpromomodalidade,
                                   nome = modalidade.nome
                               }).ToList();

            return modalidades;
        }

        public List<Listas.Item> RetornaItensAbrangencia()
        {

            var estados = (from estado in _bd.estados
                           select new Listas.Item()
                           {
                               id = estado.idestado,
                               nome = estado.nome + " - " + estado.uf.ToUpper()
                           }).ToList();

            return estados;
        }

        public List<Graficos.ItemRosca> RetornaGraficoModalidade(FiltroPromocoes filtro)
        {

            var listaModalidades = (filtro.idModalidades == null || filtro.idModalidades.Count == 1 && filtro.idModalidades.Contains(0)) ? null : filtro.idModalidades;

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                                .Type("promocao")
                                                .Size(0)
                                                .Query(q =>
                                                 q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                                    q.MultiMatch(m =>
                                                       m.Fields(f => f
                                                                 .Field("nomepromocao")
                                                                 .Field("outrosinteressados")
                                                                 .Field("premiospromo")
                                                                 .Field("nomeorgaoregulador")
                                                                 .Field("linksitepromocao")
                                                                 .Field("produtosparticipantes")
                                                                 .Field("mecanicapromo")
                                                                 .Field("textoregulamento")
                                                                 .Field("certificadoautorizacao")
                                                               ).Query(filtro.pesquisa)
                                                    )
                                                    &&
                                                    q.Terms(g => g.Field("idmodalidade").Terms(listaModalidades))
                                                    &&
                                                   (
                                                        filtro.abrangencia.Nacional != null ? q.Term("abrangencia_nacional", filtro.abrangencia.Nacional) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(m => m.Field("abrangmunicipios.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(r => r.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )                                                    
                                    &&
                                    (
                                    q.DateRange(d => d.
                                                        Field("dtcadastro")
                                                            .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                            .LessThanOrEquals(filtro.dataCadastroFinalNEST)
                                                )
                                    )
                                    &&
                                     (
                                    q.DateRange(d => d.
                                    Field("dtvigenciaini")
                                    .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                    )
                                    )
                                    &&
                                      (
                                    q.DateRange(d => d.
                                    Field("dtvigenciafim")
                                    .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                    )
                                    )
                                    )
                                                .Aggregations(a => a
                                                        .Terms("group_by_nomemodalidade", t => t.Field("nomemodalidade.raw"))
                                                        )


            );

            var resultado = response.Aggs.Terms("group_by_nomemodalidade").Buckets;

            if (resultado.Any())
            {
                List<Graficos.ItemRosca> listaItens = new List<Graficos.ItemRosca>();
                foreach (var item in resultado)
                {
                    listaItens.Add(new Graficos.ItemRosca()
                    {
                        categoria = item.Key,
                        valor = Convert.ToInt32(item.DocCount)
                    });
                }
                return listaItens;
            }

            return new List<Graficos.ItemRosca>();
        }

        public List<Graficos.ItemRosca> RetornaGraficoModalidadePerfilEmpresa(int idEmpresa)
        {

            DateTime dtFinal = DateTime.Now;
            //var dataFinal = dtFinal.AddDays(1 - dtFinal.Day).Date;
            var dataFinal = DateTime.Now;
            var dataInicial = dataFinal.AddMonths(-11);
            
            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                                .Type("promocao")
                                                //.Size(0)
                                                .Query(q =>
                                                 q.Match(m => m.Field("empresas.idempresa").Query(idEmpresa.ToString()))
                                                 &&
                                                    (
                                                    q.DateRange(d => d.
                                                                        Field("dtcadastro")
                                                                            .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", dataInicial))
                                                                            .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", dataFinal))
                                                                )
                                                    )
                                                 )                                       
                                                .Aggregations(a => a
                                                                    .Terms("group_by_nomemodalidade", t => t.Field("nomemodalidade.raw"))
                                                                    )
                                                );

            var resultado = response.Aggs.Terms("group_by_nomemodalidade").Buckets;

            if (resultado.Any())
            {
                List<Graficos.ItemRosca> listaItens = new List<Graficos.ItemRosca>();
                foreach (var item in resultado)
                {
                    listaItens.Add(new Graficos.ItemRosca()
                    {
                        categoria = item.Key,
                        valor = Convert.ToInt32(item.DocCount)
                    });
                }
                return listaItens;
            }

            return new List<Graficos.ItemRosca>();

        }



        public List<ZRN.PromocoesElastic.Noticia> RetornaNoticias(FiltroPromocoes filtro)
        {

            var listaModalidades = (filtro.idModalidades.Count == 1 && filtro.idModalidades.Contains(0)) ? null : filtro.idModalidades;

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                     .Type("promocao")
                                      .Query(q =>
                                                q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                                    q.MultiMatch(m =>
                                                       m.Fields(f => f
                                                                 .Field("nomepromocao")
                                                                 .Field("outrosinteressados")
                                                                 .Field("premiospromo")
                                                                 .Field("nomeorgaoregulador")
                                                                 .Field("linksitepromocao")
                                                                 .Field("produtosparticipantes")
                                                                 .Field("mecanicapromo")
                                                                 .Field("textoregulamento")
                                                                 .Field("certificadoautorizacao")
                                                               ).Query(filtro.pesquisa)
                                                    )
                                                    &&
                                                    q.Terms(g => g.Field("idmodalidade").Terms(listaModalidades))
                                                    &&
                                                   (
                                                        filtro.abrangencia.Nacional != null ? q.Term("abrangencia_nacional", filtro.abrangencia.Nacional) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(m => m.Field("abrangmunicipios.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(r => r.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )

                                    &&
                                    (
                                    q.DateRange(d => d.
                                                        Field("dtcadastro")
                                                            .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                            .LessThanOrEquals(filtro.dataCadastroFinalNEST)
                                                )
                                    )

                                    &&
                                     (
                                    q.DateRange(d => d.
                                    Field("dtvigenciaini")
                                    .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                    )
                                    )
                                    &&
                                      (
                                    q.DateRange(d => d.
                                    Field("dtvigenciafim")
                                    .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                    )
                                    )

                                    )
                                     .Source(ss => ss.
                                            Includes(i => i.Field("noticias")))
                                     .Aggregations(a => a.
                                            Terms("noticias", t => t.
                                                Field("noticias.idnoticia").
                                                Aggregations(aa => aa.
                                                    TopHits("noticia_docs", dd => dd.Size(1)))
                                                )
                                           )
                                     );


            var promos = response.Aggs.Terms("noticias").Buckets.ToList();

            var noticias = new List<PromocoesElastic.Noticia>();
            ZBD.Model.zeengEntities bd = new ZBD.Model.zeengEntities();
            foreach (var p in promos)
            {
                var n = p.TopHits("noticia_docs").Documents<PromocoesElastic.PromocaoElastic>().First().noticias;

                foreach(var noti in n)
                {
                    var NoticiaEmpresa = bd.noticias_empresa.Where(x => x.idempresa == filtro.idEmpresa && x.idnoticia == noti.idnoticia).FirstOrDefault();
                    if(NoticiaEmpresa != null)
                    {
                        var obj = n.Where(x => x.idnoticia == noti.idnoticia).FirstOrDefault();
                        obj.idnoticiaempresa = NoticiaEmpresa.idnoticiaempresa;
                        obj.promocoes = bd.promo_promonoticias.Where(x => x.idnoticia == noti.idnoticia).Select(s => new PromocoesElastic.Promocao() { idpromocao = s.idpromocao.Value }).ToList();
                    }
                    
                    //n.Add(obj);
                }

                noticias.AddRange(n);
            }

            var resultado = noticias.OrderByDescending(x => x.datapublicacao).GroupBy(x => x.idnoticia).Select(x => x.FirstOrDefault()).Take(10).ToList();

            return resultado;
        }

        public List<ZRN.PromocoesElastic.PromocaoElastic> RetornaPromocoes(FiltroPromocoes filtro)
        {

            var listaModalidades = (filtro.idModalidades.Count == 1 && filtro.idModalidades.Contains(0)) ? null : filtro.idModalidades;

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                     .Type("promocao")
                                     .Query(q =>

                                                q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                                    q.MultiMatch(m =>
                                                       m.Fields(f => f
                                                                 .Field("nomepromocao")
                                                                 .Field("outrosinteressados")
                                                                 .Field("premiospromo")
                                                                 .Field("nomeorgaoregulador")
                                                                 .Field("linksitepromocao")
                                                                 .Field("produtosparticipantes")
                                                                 .Field("mecanicapromo")
                                                                 .Field("textoregulamento")
                                                                 .Field("certificadoautorizacao")
                                                               ).Query(filtro.pesquisa)
                                                    )
                                                    &&
                                                    q.Terms(g => g.Field("idmodalidade").Terms(listaModalidades))
                                                    &&
                                                   (
                                                        filtro.abrangencia.Nacional != null ? q.Term("abrangencia_nacional", filtro.abrangencia.Nacional) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(m => m.Field("abrangmunicipios.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(r => r.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                        &&
                                        (
                                        q.DateRange(d => d.
                                                            Field("dtcadastro")
                                                                .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                                .LessThanOrEquals(filtro.dataCadastroFinalNEST)

                                                    )
                                        )

                                        &&
                                         (
                                            q.DateRange(d => d.
                                                                Field("dtvigenciaini")
                                                                .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                            )
                                        )
                                        &&
                                          (
                                            q.DateRange(d => d.
                                                                Field("dtvigenciafim")
                                                                .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                            )
                                        )

                                    )));


            var resultado = response.Documents.ToList<ZRN.PromocoesElastic.PromocaoElastic>();

            return resultado;
        }

        public Promocao RetornaPromocaoDetalhe(int idPromocao, int IdEmpresa)
        {

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s =>
                                    s.Query(q =>
                                                q.Match(m => m.Field("idpromocao").Query(idPromocao.ToString())) 
                                                &&
                                                q.Term("empresas.idempresa", IdEmpresa)


                                                
                                            )
                                        );

            if (response.Documents.Count > 0)
            {
                


            var promocoes = response.Documents.Select(s => new Promocao
                {
                    idPromocao = s.idPromocao,
                    nomePromocao = s.nomePromocao,
                    empresas = s.empresas.Select(e => new Empresas { nome = e.nome }).ToList(),
                NomeModalidade = s.NomeModalidade,
                Abrangecia = new Abrangencia(s.abrangencia_nacional)
                {
                    Estados = s.abrangestados.Select(ss => new Estados.Estado { UF = ss.uf }).ToList(),
                    Municipios = s.abrangmunicipios.Select(am => new Municipios.Municipio { Nome = am.nome, UF = am.uf }).ToList()
                },

                dtvigenciaini = Convert.ToDateTime(s.dtVigenciaIni),
                dtvigenciafim = Convert.ToDateTime(s.dtVigenciaFim),
                NomeOrgaoRegulador = s.NomeOrgaoRegulador,
                OutrosInteressados = s.OutrosInteressados,
                CertificadoAutorizacao = s.CertificadoAutorizacao,
                LinkFacebook = s.LinkFacebook,
                LinkInstagram = s.LinkInstagram,
                LinkTwitter = s.LinkTwitter,
                LinkYoutube = s.LinkYoutube,
                LinkRegulamento = s.LinkRegulamento,
                LinkSitePromocao = s.LinkSitePromocao,
                Mecanica = s.Mecanicapromo,
                ProdutosParticipantes = s.ProdutosParticipantes,
                Premios = s.PremiosPromo,
                ValorPremios = s.ValorPremios,
                TextoRegulamento = s.TextoRegulamento,
                arquivosregulamento = s.arquivosregulamento != null ? s.arquivosregulamento.Select(ss => new Arquivo { NomeArquivo = ss.NomeArquivo, Tipo = ss.Tipo }).ToList() : new List<Arquivo>(),
                arquivosrelacionados = s.arquivosrelacionados != null ? s.arquivosrelacionados.Select(ss => new Arquivo { NomeArquivo = ss.NomeArquivo, Tipo = ss.Tipo, Url = ss.Url }).ToList() : new List<Arquivo>()

            }).First();

                

                return promocoes;
            }


            return null;




            

        }

        public PesquisaPromocoes RetornaPromocoesTimeLine(FiltroPromocoes filtro)
        {

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                         .Query(q =>
                                                q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString()))

                                                   && (
                                                      filtro != null && filtro.pesquisa != null ?
                                                      q.Term("nomepromocao", filtro.pesquisa) ||
                                                      q.Term("outrosinteressados", filtro.pesquisa) ||
                                                      q.Term("mecanicapromo", filtro.pesquisa) ||
                                                      q.Term("produtosparticipantes", filtro.pesquisa) ||
                                                      q.Term("premiospromo", filtro.pesquisa) ||
                                                      q.Term("textoregulamento", filtro.pesquisa)

                                                      : null
                                                      )

                                                   &&
                                                                 q.DateRange(d => d
                                                                 .Field("dtvigenciaini")
                                                                 .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                                                 )


                                                   &&
                                                        (
                                                              filtro != null && filtro.dataVigenciaFinal != null ?
                                                                q.DateRange(d => d
                                                                //.Field(f => f.dtVigenciaFim)
                                                                .Field("dtvigenciafim")
                                                                .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                                                 ) : null
                                                        )
                                                   &&
                                                                 q.DateRange(dc => dc
                                                                 //.Field(f => f.dtCadastro)
                                                                .Field("dtcadastro")
                                                                .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                                .LessThanOrEquals(filtro.dataCadastroFinalNEST)
                                                                 )


                                                   && (

                                                     filtro != null && filtro.abrangencia != null && filtro.abrangencia.idEstados != null ?
                                                     q.Terms(p => p.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados))

                                                     : null
                                                     )

                                                   && (

                                                   filtro != null && filtro.idModalidades != null ?
                                                     q.Terms(p => p.Field("idmodalidade").Terms(filtro.idModalidades))

                                                     : null
                                                     )

                                                     )//Query

                                                     .Sort(a=> a.Descending("dtcadastro"))


                                                    );

            _bd = new ZBD.Model.zeengEntities();

            if (response.Documents.Count > 0)
            {
                var promocoes = new PesquisaPromocoes();

                promocoes.totalRegistros = response.Documents.Count;

                var timeline = response.Documents.Select(s=> new Promocao {
                                                                           idPromocao = s.idPromocao,
                                                                           nomePromocao = s.nomePromocao,
                                                                           empresas = s.empresas.Select(e=> new Empresas{nome = e.nome }).ToList(),
                                                                           NomeModalidade = s.NomeModalidade,
                                                                           Abrangecia = new Abrangencia(s.abrangencia_nacional) {
                                                                                        Estados = s.abrangestados.Select(ss => new Estados.Estado { UF = ss.uf }).ToList(), 
                                                                                        Municipios = s.abrangmunicipios.Select(am => new Municipios.Municipio {Nome = am.nome, UF = am.uf }).ToList()
                                                                           },
                                                                           dtCadastro = Convert.ToDateTime(s.dtCadastro),
                                                                           dtvigenciaini = Convert.ToDateTime(s.dtVigenciaIni),
                                                                           dtvigenciafim = Convert.ToDateTime(s.dtVigenciaFim),
                                                                           NomeOrgaoRegulador = s.NomeOrgaoRegulador,
                                                                           CertificadoAutorizacao = s.CertificadoAutorizacao

                                                                           }).Skip(filtro.pagina).Take(filtro.qtdregistros).ToList(); ;

                promocoes.Promocoes = timeline;

                return promocoes;
            }


            return null;
        }

        public List<Graficos.Linha> RetornaGraficoMes(FiltroPromocoes filtro)
        {
            return null;
        }

        //public List<RedesSociais.Facebook.Post> RetornaPostsFace(int idPromocao, string filtro, int pagina, int qtdregistros)
        //{
        //    return RetornaPostsFace(new List<int>(idPromocao), filtro, pagina, qtdregistros);
        //}

        public Graficos.Mapas.Brasil RetornaPromocoesPorEstado(FiltroPromocoes filtro)
        {
            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                         .Query(q =>
                                                q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString()))

                                                   && (
                                                      filtro != null && filtro.pesquisa != null ?
                                                      q.Term("nomepromocao", filtro.pesquisa) ||
                                                      q.Term("outrosinteressados", filtro.pesquisa) ||
                                                      q.Term("mecanicapromo", filtro.pesquisa) ||
                                                      q.Term("produtosparticipantes", filtro.pesquisa) ||
                                                      q.Term("premiospromo", filtro.pesquisa) ||
                                                      q.Term("textoregulamento", filtro.pesquisa)

                                                      : null
                                                      )

                                                   &&
                                                                 q.DateRange(d => d
                                                                 .Field("dtvigenciaini")
                                                                 .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                                                 )


                                                   &&
                                                        (
                                                              filtro != null && filtro.dataVigenciaFinal != null ?
                                                                q.DateRange(d => d
                                                                //.Field(f => f.dtVigenciaFim)
                                                                .Field("dtvigenciafim")
                                                                .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                                                 ) : null
                                                        )
                                                   &&
                                                                 q.DateRange(dc => dc
                                                                 //.Field(f => f.dtCadastro)
                                                                .Field("dtcadastro")
                                                                .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                                .LessThanOrEquals(filtro.dataCadastroFinalNEST)
                                                                 )


                                                   && (

                                                     filtro != null && filtro.abrangencia != null && filtro.abrangencia.idEstados != null ?
                                                     q.Terms(p => p.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados))

                                                     : null
                                                     )

                                                   && (

                                                   filtro != null && filtro.idModalidades != null ?
                                                     q.Terms(p => p.Field("idmodalidade").Terms(filtro.idModalidades))

                                                     : null
                                                     )

                                                     )//Query

                                                    .Aggregations(a => a.Terms("SomaEstados", t => t.Field("abrangestados.idestado"))
                                                     .Terms("SomaMunicipios", t => t.Field("abrangmunicipios.idestado")))





                                                    );


            _bd = new ZBD.Model.zeengEntities();

            if (response.Documents.Count > 0)
            {

                var groupEstados = response.Aggs.Terms("SomaEstados").Buckets.Select(e => new { idestado = Convert.ToInt32(e.Key), total = Convert.ToInt32(e.DocCount) }).ToList();
                var groupMunicipio = response.Aggs.Terms("SomaMunicipios").Buckets.Select(e => new { idestado = Convert.ToInt32(e.Key), total = Convert.ToInt32(e.DocCount) }).ToList();

                var SomaEstadoMunicpio = (from ge in groupEstados
                                          join gm in groupMunicipio on ge.idestado equals gm.idestado
                                          select new
                                          {

                                              idestado = ge.idestado,
                                              total = ge.total + gm.total

                                          }

                                         ).ToList();


                var resultado = (from ge in groupEstados
                                 select new
                                 {
                                     idestado = ge.idestado,
                                     total = SomaEstadoMunicpio.Where(x => x.idestado == ge.idestado).Select(s => s.total).Count() > 0 ?
                                              SomaEstadoMunicpio.Where(x => x.idestado == ge.idestado).Select(s => s.total).First()
                                              : ge.total
                                 }
                               );

                if (filtro != null && filtro.abrangencia != null && filtro.abrangencia.idEstados != null)
                {
                    resultado = resultado.Where(x => filtro.abrangencia.idEstados.Contains(x.idestado)).ToList();
                }

                var SomatarioEstados = (from e in _bd.estados.ToList()
                                        join a in resultado on e.idestado equals a.idestado
                                        orderby a.total descending
                                        select new Graficos.Mapas.Estado()
                                        {
                                            total = a.total,
                                            uf = e.uf

                                        }).ToList();

                var mapBrasil = new Graficos.Mapas.Brasil();
                mapBrasil.Estados = SomatarioEstados;
                return mapBrasil;
            }

            return null;
        }
        
        public List<PostPromocao> RetornaPostagensPromocao(FiltroPromocoes filtro)
        {
            var listaModalidades = (filtro.idModalidades.Count == 1 && filtro.idModalidades.Contains(0)) ? null : filtro.idModalidades;

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);
            
            var response = client.Search<ZRN.PromocoesElastic.PromocaoElastic>(s => s
                                            .Type("promocao")
                                            .Query(q =>
                                                    q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                                    q.MultiMatch(m =>
                                                                 m.Fields(f => f
                                                                             .Field("nomepromocao")
                                                                             .Field("outrosinteressados")
                                                                             .Field("premiospromo")
                                                                             .Field("nomeorgaoregulador")
                                                                             .Field("linksitepromocao")
                                                                             .Field("produtosparticipantes")
                                                                             .Field("mecanicapromo")
                                                                             .Field("textoregulamento")
                                                                             .Field("certificadoautorizacao")
                                                               ).Query(filtro.pesquisa)
                                                    )
                                                    &&
                                                    q.Terms(g => g.Field("idmodalidade").Terms(listaModalidades))
                                                    &&   
                                                    (                                                 
                                                        filtro.abrangencia.Nacional != null ? q.Term("abrangencia_nacional", filtro.abrangencia.Nacional) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(m => m.Field("abrangmunicipios.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(r => r.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados)) : null                                                      
                                                    )
                                                    &&
                                                    (
                                                        q.DateRange(d => d.
                                                                            Field("dtcadastro")
                                                                                .GreaterThanOrEquals(filtro.dataCadastroInicialNEST)
                                                                                .LessThanOrEquals(filtro.dataCadastroFinalNEST)
                                                                    )
                                                    )
                                                    &&
                                                     (
                                                        q.DateRange(d => d.
                                                        Field("dtvigenciaini")
                                                        .GreaterThanOrEquals(filtro.dataVigenciaInicialNEST)
                                                        )
                                                      )
                                                    &&
                                                      (
                                                        q.DateRange(d => d.
                                                        Field("dtvigenciafim")
                                                        .LessThanOrEquals(filtro.dataVigenciaFinalNEST)
                                                          )
                                                    )

                                                    &&
                                                    (
                                                        q.ConstantScore(c => c.Filter(qq => qq.Exists(f => f.Field("postsfacebook"))))
                                                        ||
                                                        q.ConstantScore(c => c.Filter(qq => qq.Exists(f => f.Field("poststwitter"))))
                                                        ||
                                                        q.ConstantScore(c => c.Filter(qq => qq.Exists(f => f.Field("postsinstagram"))))
                                                        ||
                                                        q.ConstantScore(c => c.Filter(qq => qq.Exists(f => f.Field("videosyoutube"))))
                                                    )
                                        )
                                //.Source(ss => ss.
                                //    Includes(i => i.Field("postsfacebook")))
                                //.Aggregations(a => a.
                                //    Terms("postsfacebook", t => t.
                                //        Field("postsfacebook.idpost").
                                //        Aggregations(aa => aa.
                                //            TopHits("posts_face", dd => dd.Size(1))
                                //        )
                                //    )
                                //)
                            );

            var res = response.Documents;

            var lista = new List<PostPromocao>();

            foreach (var item in res)
            {

                if(item.postsfacebook != null)
                {
                    var listaTempFace = item.postsfacebook.Select(x => new PostPromocao()
                    {
                        comentarios = x.qtdcomentarios,
                        compartilhamentos = x.compartilhamentos,
                        curtidas = x.curtidas,
                        dataPostagem = Convert.ToDateTime(x.datahora).Date,
                        imagem = x.nomeimagem,
                        redeSocial = "facebook",
                        postagem = x.postagem,
                        //promocoes = x.promocoes.Select(t => new tagPromo() { nome = t.nomepromocao }).ToList()                                                                                   
                    }).ToList();
                    lista.AddRange(listaTempFace);
                }
                
                if(item.postsinstagram != null)
                {
                    var listaTempInsta = item.postsinstagram.Select(x => new PostPromocao()
                    {
                        comentarios = x.qtdcomentarios,
                        curtidas = x.curtidas,
                        dataPostagem = Convert.ToDateTime(x.datahora).Date,
                        redeSocial = "instagram",
                        postagem = x.postagem,
                        imagem = x.nomeimagem,
                        //promocoes = x.promocoes.Select(t => new tagPromo() { nome = t.nomepromocao }).ToList()
                    }).ToList();
                    lista.AddRange(listaTempInsta);
                }
                
                if(item.poststwitter != null)
                {
                    var listaTempTw = item.poststwitter.Select(x => new PostPromocao()
                    {
                        curtidas = x.curtidas,
                        dataPostagem = Convert.ToDateTime(x.datahora).Date,
                        redeSocial = "twitter",
                        postagem = x.postagem,
                        retweets = x.retweets,
                        imagem = x.nomeimagem,
                        //promocoes = x.promocoes.Select(t => new tagPromo() { nome = t.nomepromocao }).ToList()
                    }).ToList();
                    lista.AddRange(listaTempTw);
                }                

                if(item.videosyoutube != null)
                {
                    var listaTempVideos = item.videosyoutube.Select(x => new PostPromocao()
                    {
                        curtidas = x.curtidas,
                        dataPostagem = Convert.ToDateTime(x.datahora).Date,
                        redeSocial = "youtube",
                        postagem = x.descricao,
                        visualizacoes = x.visualizacoes,
                        imagem = x.nomeimagem,
                        //promocoes = x.promocoes.Select(t => new tagPromo() { nome = t.nomepromocao }).ToList()
                    }).ToList();
                    lista.AddRange(listaTempVideos);
                }
            }

            lista = lista.OrderByDescending(x => x.dataPostagem).Take(10).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna as notícias das promoções conforme o filtro
        /// Ordenadas de forma decrescente
        /// </summary>
        /// <param name="idPromocao">Id da promoção</param>
        /// <param name="filtro">Filtro do tipo texto para pesquisa no conteúdo da notícia</param>
        /// <param name="pagina">Página a ser retornada</param>
        /// <param name="qtdregistros">Quantidade de registros a serem retornada</param>
        /// <returns></returns>
        public List<NoticiasElastic.NoticiaElastic> RetornaNoticias(int idPromocao, string filtro, int pagina, int qtdregistros)
        {
            return null;
        }

        /// <summary>
        /// Retorna as notícias das promoções conforme o filtro
        /// Ordenadas de forma decrescente
        /// </summary>
        /// <param name="idPromocao">Lista de ids de promoção</param>
        /// <param name="filtro">Filtro do tipo texto para pesquisa no conteúdo da notícia</param>
        /// <param name="pagina">Página a ser retornada</param>
        /// <param name="qtdregistros">Quantidade de registros a serem retornada</param>
        /// <returns></returns>
        public List<NoticiasElastic.NoticiaElastic> RetornaNoticias(List<int> idPromocao, string filtro, int pagina, int qtdregistros)
        {
            return null;
        }

        public List<Graficos.Barra> RetornaGraficoVigencia(FiltroPromocoes filtro)
        {
            var listaModalidades = (filtro.idModalidades == null || filtro.idModalidades.Count == 1 && filtro.idModalidades.Contains(0)) ? null : filtro.idModalidades;


            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<Promocao>(s =>
                                    s.Query(q =>

                                              q.DateRange(d => d.Field("dtvigenciaini").GreaterThanOrEquals(filtro.dataVigenciaInicialNEST).LessThanOrEquals(filtro.dataVigenciaFinalNEST)) &&
                                              q.DateRange(d => d.Field("dtvigenciafim").GreaterThanOrEquals(filtro.dataVigenciaInicialNEST).LessThanOrEquals(filtro.dataVigenciaFinalNEST)) &&
                                              q.DateRange(d => d.Field("dtcadastro").GreaterThanOrEquals(filtro.dataCadastroInicialNEST).LessThanOrEquals(filtro.dataCadastroFinalNEST)) &&
                                              q.Terms(g => g.Field("empresas.idempresa").Terms(filtro.idEmpresa)) &&
                                                q.MultiMatch(m =>
                                                       m.Fields(f => f
                                                                 .Field("nomepromocao")
                                                                 .Field("outrosinteressados")
                                                                 .Field("premiospromo")
                                                                 .Field("nomeorgaoregulador")
                                                                 .Field("linksitepromocao")
                                                                 .Field("produtosparticipantes")
                                                                 .Field("mecanicapromo")
                                                                 .Field("textoregulamento")
                                                                 .Field("certificadoautorizacao")
                                                               ).Query(filtro.pesquisa)
                                                    )
                                                    &&
                                                    q.Terms(g => g.Field("idmodalidade").Terms(listaModalidades))
                                                    &&                                                  
                                                    (
                                                        filtro.abrangencia.Nacional != null ? q.Term("abrangencia_nacional", filtro.abrangencia.Nacional) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(m => m.Field("abrangmunicipios.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )
                                                    &&
                                                    (
                                                        filtro.abrangencia.idEstados.Count > 1 && filtro.abrangencia.idEstados != null ? q.Terms(r => r.Field("abrangestados.idestado").Terms(filtro.abrangencia.idEstados)) : null
                                                    )

                                              )

                                            .Sort(x => x.Ascending(p => p.dtvigenciaini))
                                            .Size(10000)
                                        );

            
            
            //DateTime dataContador = Convert.ToDateTime(filtro.dataVigenciaInicialNEST);
            //DateTime dataContadorFinal = Convert.ToDateTime(filtro.dataVigenciaFinalNEST);
            //var rangeIntervaloDatas = new List<DateTime>();

            //while(dataContador <= dataContadorFinal)
            //{
            //    rangeIntervaloDatas.Add(dataContador);
            //    dataContador = dataContador.AddDays(1);
            //}

            var dicionarioDatasValores = new Dictionary<DateTime, int>();
            foreach (var promo in response.Documents.ToList())
            {                
                var dataIni = new DateTime(promo.dtvigenciaini.Year, promo.dtvigenciaini.Month, 01);
                var dataFim = new DateTime(promo.dtvigenciafim.Year, promo.dtvigenciafim.Month, 01);
                var contadorDatas = dataIni;

                while(contadorDatas <= dataFim)
                {
                    if (dicionarioDatasValores.ContainsKey(contadorDatas))
                    {
                        dicionarioDatasValores[contadorDatas] += 1;
                    }
                    else
                    {
                        dicionarioDatasValores[contadorDatas] = 1;
                    }
                                        
                    contadorDatas = contadorDatas.AddMonths(1);
                }                
            }


            var dadosGraficoBarra = new List<Graficos.Barra>();
            //var dataValor = new Dictionary<DateTime, int>();
            //DateTime indiceArrayDataIni, indiceArrayDataFim;

            //foreach (var promo in response.Documents.ToList())
            //{
            //    // Verifica a data ini e fim está no mesmo mês e ano
            //    if ((promo.dtvigenciaini.Month == promo.dtvigenciafim.Month) && (promo.dtvigenciaini.Year == promo.dtvigenciafim.Year))
            //    {
            //        indiceArrayDataIni = new DateTime(promo.dtvigenciaini.Year, promo.dtvigenciaini.Month, 01);
            //        if (dataValor.ContainsKey(indiceArrayDataIni))
            //        {
            //            dataValor[indiceArrayDataIni] += 1;
            //        }
            //        else
            //        {
            //            dataValor[indiceArrayDataIni] = 1;
            //        }

            //    }
            //    else
            //    {
            //        indiceArrayDataIni = new DateTime(promo.dtvigenciaini.Year, promo.dtvigenciaini.Month, 01);
            //        indiceArrayDataFim = new DateTime(promo.dtvigenciafim.Year, promo.dtvigenciafim.Month, 01);

            //        //Ini
            //        if (dataValor.ContainsKey(indiceArrayDataIni))
            //        {
            //            dataValor[indiceArrayDataIni] += 1;
            //        }
            //        else
            //        {
            //            dataValor[indiceArrayDataIni] = 1;
            //        }

            //        // Fim
            //        if (dataValor.ContainsKey(indiceArrayDataFim))
            //        {
            //            dataValor[indiceArrayDataFim] += 1;
            //        }
            //        else
            //        {
            //            dataValor[indiceArrayDataFim] = 1;
            //        }
            //    }
            //}

            //var dataValorOrdenado = dataValor.OrderBy(x => x.Key);
            var dataValorOrdenado = dicionarioDatasValores.OrderBy(x => x.Key);

            foreach (var dv in dataValorOrdenado)
            {
                var obj = new Graficos.Barra();
                obj.data = dv.Key.ToString("yyyy-MM-dd");
                obj.valor = dv.Value;

                dadosGraficoBarra.Add(obj);
            }

            return dadosGraficoBarra;

        }
    }



}
