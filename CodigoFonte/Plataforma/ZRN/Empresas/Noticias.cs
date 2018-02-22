using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;


namespace ZRN.Empresas
{

    public class Noticias
    {
        private string _servidorElastic;
        private string _indexElastic;

        private ZBD.Model.zeengEntities db = new ZBD.Model.zeengEntities();

        public Noticias()
        {
        }

        public Noticias(string servidorElastic, string indexElastic)
        {
            _servidorElastic = servidorElastic;
            _indexElastic = indexElastic;
        }

        public int RetornaQuantidadeNoticias(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var quantidadeNoticias = (from noticia in db.noticias_empresa
                                      where noticia.idempresa == idEmpresa &&
                                      noticia.noticias.datapublicacao >= dtInicial &&
                                      noticia.noticias.datapublicacao <= dtFinal && 
                                      noticia.noticias.conteudo.Length > 200
                                      select noticia).Count();

            return quantidadeNoticias;
        }

        public int RetornaQuantidadeNoticiasMesElastic(int idEmpresa, DateTime dataInicial, DateTime dataFinal)
        {

            var dtInicial = DateTime.Now.AddMonths(-11).AddDays(1 - DateTime.Now.Day);
            var dtFinal = DateTime.Now;
         

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var resultado = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                          .Query(q =>
                                    q.Match(
                                        m => m
                                                .Field("empresas.idempresa").Query(idEmpresa.ToString())
                                        )
                                    &&
                                        q.DateRange(d => d
                                            .Field(f => f.datapublicacao)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy 00:00:00}", dtInicial))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy 23:59:59}", dtFinal))
                                    )
                                    )
                            .ScriptFields(sf => sf
                                                .ScriptField("test", c => c.Inline("doc['conteudo'].value.length > 200"))
                            )
                          );

            var total = resultado.Total;

            return (Int32)total;
        }

        public List<Graficos.Linha> RetornaNoticiasPorMes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {



            string dataInicial = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTotalNoticiasPorMes('" + idEmpresa + "', '" + dataInicial + "', '" + dataFim + "');";

            List<NoticiaPerfil> listaNoticias = db.Database.SqlQuery<NoticiaPerfil>(query).ToList();
            var datasGraf = dtInicial.AddDays(1 - dtInicial.Day).Date;

            //var d = datasGraf.Date;
            //var d2 = dtFinal.Date;

            var linhas = new List<Linha>();
            var i = 0;


            if (listaNoticias.Any())
            {

                while (datasGraf.Date < dtFinal.Date)
                {
                    long v = 0;
                    if (listaNoticias.Count > i && listaNoticias[i].data == datasGraf)
                    {
                        v = listaNoticias[i].total;
                        i++;
                    }

                    var l = new Linha()
                    {
                        categoria = "Noticias",
                        data = datasGraf.ToString("yyyy-MM-dd"),
                        valor = v
                    };
                    linhas.Add(l);
                    datasGraf = datasGraf.AddMonths(1);

                }




                return linhas;
            }

            else
            {
                return null;
            }

        }

        public List<Graficos.Linha> RetornaNoticiasPorSemana(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            string dataInicial = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTotalNoticiasPorSemana('" + idEmpresa + "', '" + dataInicial + "', '" + dataFim + "');";

            List<NoticiaPerfil> listaNoticias = db.Database.SqlQuery<NoticiaPerfil>(query).ToList();

            //int numeroMes = Convert.ToDateTime(dataInicial).Month;

            //var dataGrafico = dtInicial.AddMonths(0 - numeroMes);
            //var dadosGraf = new List<Graficos.Linha>();

            //while (dataGrafico <= Convert.ToDateTime(dtFinal))
            //{
            //    var valorLinha = new Graficos.Linha();
            //    valorLinha.categoria = "Noticias";
            //    valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
            //    valorLinha.valor = 0;

            //    var dadosNoticia = listaNoticias.Where(x =>
            //                                           Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
            //                                           Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year
            //                                       ).FirstOrDefault();

            //    if (dadosNoticia != null)
            //    {
            //        valorLinha.valor = dadosNoticia.total;
            //    }

            //    dadosGraf.Add(valorLinha);
            //    dataGrafico = dataGrafico.AddMonths(1);

            //}

            var linhas = (from d in listaNoticias
                          select new Linha()
                          {
                              data = d.data.ToString("yyyy-MM-dd"),
                              valor = d.total,
                              categoria = "Noticias"
                          }).ToList();




            return linhas;
        }


        //public List<String> RetornaPrincipaisTermosNoticias(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        //{

        //    var db = new ZBD.Model.zeengEntities();

        //    string dataInicial = dtInicial.ToString("yyyy-MM-dd");
        //    string dataFim = dtFinal.ToString("yyyy-MM-dd");

        //    var lista = (from termo in db.noticia_termos
        //                 join noticia in db.noticias on termo.idnoticia equals noticia.idnoticia
        //                 join noticiaEmpresa in db.noticias_empresa on noticia.idnoticia equals noticiaEmpresa.idnoticia
        //                 where noticiaEmpresa.idempresa == idEmpresa &&
        //                 noticia.datapublicacao >= dtInicial &&
        //                 noticia.datapublicacao <= dtFinal
        //                 orderby termo.frequencia descending
        //                 select termo.termo).ToList();

        //    return lista;
        //}

        public List<ZRN.Graficos.TagCloud.Termo> RetornaPrincipaisTermosNoticias(FiltroNoticiasEmpresa filtro)
        {
            filtro.fontes = filtro.fontes == null || filtro.fontes.Count == 0 ? new List<int>() { } : filtro.fontes;

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var response = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                            .Source(f => f.Includes(i => i.Field("idnoticia")))
                            .Size(10000)
                            .Query(q =>

                                    q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                    (
                                       q.MultiMatch(m => m
                                                        .Fields(f => f.Field("titulo").Field("subtitulo").Field("conteudo"))
                                                        .Query(filtro.expressao)
                                                        .Operator(Operator.And)
                                                        )
                                     ) &&
                                     q.Terms(p => p.Field(f => f.idfonte).Terms(filtro.fontes)) &&
                                     q.DateRange(d => d
                                        .Field(f => f.datapublicacao)
                                        .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataInicial))
                                        .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataFinal))
                                        )
                                     )
                                .Sort(x => x.Descending(p => p.datapublicacao))
                            );

            // var resultado = response.Documents.ToList<NoticiasElastic.noticias>();

            var ids = (from n in response.Documents
                       select n.idnoticia).ToList().Take(200);

            //db.Database.Connection.ConnectionTimeout

            var termos = (from n in db.noticia_termos
                          where ids.Contains(n.idnoticia.Value)
                          group n by n.termo into termos_noticia
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = termos_noticia.Key,
                              frequencia = termos_noticia.Sum(x => x.frequencia)
                          }).OrderByDescending(o => o.frequencia).Take(filtro.qtdRegistros).Where(x => x.frequencia > 1).ToList();

            //var notEmpresa = new NoticiasEmpresa();
            //notEmpresa.TotalNoticias = Convert.ToInt32(response.Total);
            //notEmpresa.noticias = response.Documents.ToList<NoticiasElastic.noticias>();

            return termos;

        }

        public List<NoticiaEmpresaFonte> RetornaFonteNotciasEmpresa(int idempresa)
        {

            var listafonte = (from fn in db.fontes_noticias
                                  //join n in db.noticias on fn.idfonte equals n.idfonte
                                  //join ne in db.noticias_empresa on n.idnoticia equals ne.idnoticia
                                  //where ne.idempresa == idempresa
                                  //group fn by new { fn.idfonte, ne.idempresa, fn.nome } into g
                                  //orderby g.Key.nome ascending
                              orderby fn.nome ascending
                              select new NoticiaEmpresaFonte
                              {

                                  //idempresa = g.Key.idempresa,
                                  //idfonte = g.Key.idfonte,
                                  //nome = g.Key.nome
                                  idempresa = idempresa,
                                  idfonte = fn.idfonte,
                                  nome = fn.nome

                              }).ToList();

            return listafonte;
        }

        public NoticiasEmpresa Pesquisar(FiltroNoticiasEmpresa filtro)
        {
            filtro.fontes = filtro.fontes == null || filtro.fontes.Count == 0 ? new List<int>() { } : filtro.fontes;

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var response = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                            .From(filtro.inicial)
                            .Size(filtro.qtdRegistros)
                            .Query(q =>
                                q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                q.MultiMatch(m => m
                                    .Fields(f => f.Field(p => p.titulo)
                                                    .Field(p => p.subtitulo)
                                                    .Field(p => p.conteudo)
                                    )
                                    .Query(filtro.titulo)
                                    .Operator(Operator.And)
                                ) &&
                                //q.Term("empresas.idempresa", filtro.idEmpresa) &&
                                q.Terms(p => p.Field(f => f.idfonte).Terms(filtro.fontes)) &&
                                q.DateRange(d => d
                                    .Field(f => f.datapublicacao)
                                    .GreaterThanOrEquals(string.Format("{0:dd/MM/yy 00:00:00}", filtro.dataInicial))
                                    .LessThanOrEquals(string.Format("{0:dd/MM/yy 23:59:59}", filtro.dataFinal))
                                )
                            )
                            .Highlight(h => h
                                .Fields(f => f.Field("*").PreTags("<span class='highlight'>").PostTags("</span>").NumberOfFragments(0).ForceSource(true))
                            )
                             //.Query(q =>
                             //        (
                             //            q.Term(p => p.titulo, filtro.titulo) ||
                             //            q.Term(p => p.subtitulo, filtro.subtitulo) ||
                             //            q.Term(p => p.conteudo, filtro.conteudo) 
                             //         ) &&
                             //         q.Term("empresas.idempresa", filtro.idEmpresa) &&
                             //         q.Terms(p => p.Field(f => f.idfonte).Terms(filtro.fontes)) &&
                             //         q.DateRange(d => d
                             //            .Field(f => f.datapublicacao)
                             //            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataInicial))
                             //            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataFinal))
                             //            ))
                             .Sort(a => a.Descending(p => p.datapublicacao))
                            );

            // var resultado = response.Documents.ToList<NoticiasElastic.noticias>();            

            var notEmpresa = new NoticiasEmpresa();
            notEmpresa.TotalNoticias = Convert.ToInt32(response.Total);



            notEmpresa.noticias = (from d in response.Documents
                                   select new NoticiaEmpresa()
                                   {
                                       idnoticia = d.idnoticia,
                                       idfonte = d.idfonte,
                                       linguagem = d.linguagem,
                                       localidade = d.localidade,
                                       autor = d.autor,
                                       conteudo = d.conteudo,
                                       subtitulo = d.subtitulo,
                                       titulo = d.titulo,
                                       datapublicacao = d.datapublicacao,
                                       dominio = d.dominio,
                                       nomefonte = d.nomefonte,
                                       url = d.url,
                                       idnoticiaempresa = d.empresas.Where(x => x.idempresa == filtro.idEmpresa).Select(x => x.idnoticiaempresa).FirstOrDefault(),
                                       nomeEmpresa = d.empresas.Where(x => x.idempresa == filtro.idEmpresa).Select(x => x.nomeempresa).FirstOrDefault(),
                                       tags = d.empresas.Select(x => new TagNoticia() { termo = x.nomeempresa }).ToList()

                                   }).ToList();



            // Aplicando highliths em título e conteúdo...
            var ListaHigh = response.Hits;

            foreach (var lista in ListaHigh)
            {
                var id = lista.Id;
                var noticia = notEmpresa.noticias.Where(x => x.idnoticia == Convert.ToInt32(id)).FirstOrDefault();

                var _keys = lista.Highlights;
                foreach (var k in _keys)
                {
                    if (k.Key == "conteudo")
                        noticia.conteudo = k.Value.Highlights.FirstOrDefault();

                    if (k.Key == "titulo")
                        noticia.titulo = k.Value.Highlights.FirstOrDefault();
                }
            }

            return notEmpresa;
        }

        public List<Linha> PesquisaNoticiasPorDia(FiltroNoticiasEmpresa filtro)
        {
            filtro.fontes = filtro.fontes == null || filtro.fontes.Count == 0 ? new List<int>() { } : filtro.fontes;

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var resultado = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                            .Query(q =>
                                        q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                        (
                                            q.Term(p => p.titulo, filtro.titulo) ||
                                            q.Term(p => p.subtitulo, filtro.subtitulo) ||
                                            q.Term(p => p.conteudo, filtro.conteudo)
                                        ) &&
                                            //q.Term("empresas.idempresa", filtro.idEmpresa) &&
                                            q.Terms(p => p.Field(f => f.idfonte).Terms(filtro.fontes)) &&
                                            q.DateRange(d => d
                                        .Field(f => f.datapublicacao)
                                        .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataInicial))
                                        .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtro.dataFinal))
                                        ))
                            .Aggregations(a => a.DateHistogram("byday", h => h
                                         .Field("datapublicacao")
                                         .Interval(DateInterval.Day)))
                            );

            var linhas = new List<Linha>();

            if (resultado != null && resultado.Documents.Count > 0)
            {
                linhas = (from item in resultado.Aggs.DateHistogram("byday").Buckets
                          select new Linha()
                          {
                              data = item.Date.Date.ToString("dd/MM/yyyy"),
                              datahora = item.Date,
                              valor = Convert.ToInt32(item.DocCount)
                          }).ToList();
            }
            else
                linhas = null;

            return linhas;
        }

        public List<NoticiasFonte> PesquisaTopFontes(FiltroNoticiasEmpresa filtro)
        {
            // filtro.fontes = null;

            //var DiaFimAtual = DateTime.Now;
            var dataFimAtual = filtro.dataFinal;
            //var DiaInicioAtual = DiaFimAtual.AddDays(-30); // ************* valor correto -7 *****************
            var dataInicioAtual = filtro.dataInicial;

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            // Resultado top fontes apartir dos filtros
            // =========================================================================================
            var resultado = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                                .MatchAll()
                                .Query(q =>
                                        q.Match(m => m.Field("empresas.idempresa").Query(filtro.idEmpresa.ToString())) &&
                                        (
                                            q.Term(p => p.titulo, filtro.titulo) ||
                                            q.Term(p => p.subtitulo, filtro.subtitulo) ||
                                            q.Term(p => p.conteudo, filtro.conteudo)
                                        ) &&
                                            //    q.Term("empresas.idempresa", filtro.idEmpresa) &&
                                            q.Terms(p => p.Field(f => f.idfonte).Terms(filtro.fontes)) &&
                                            q.DateRange(d => d
                                            .Field(f => f.datapublicacao)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataInicioAtual))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataFimAtual))
                                            ))
                                .Aggregations(a => a
                                       .Terms("TotalNoticias", f => f.Field(c => c.idfonte)))
                            );

            //periodo anterior
            var diferenca = ((dataFimAtual - dataInicioAtual).TotalDays) + 1;
            var dataInicialPeriodoAnterior = dataInicioAtual.AddDays(-diferenca);
            var dataFinalPeriodoAnterior = dataInicioAtual.AddDays(-1);

            var lista = new List<NoticiasFonte>();
            if (resultado != null && resultado.Aggs.Terms("TotalNoticias").Buckets.Count > 0)
            {
                lista = (
                        from a in resultado.Aggs.Terms("TotalNoticias").Buckets
                        select new NoticiasFonte()
                        {
                            idFonte = Convert.ToInt32(a.Key),
                            total = Convert.ToInt32(a.DocCount),
                            nomeFonte = GetNomeFonte(Int32.Parse(a.Key)),
                            totalNoticias = PesquisaGraficoNoticiasPorDiaFonte(dataInicioAtual, dataFimAtual, Convert.ToInt32(a.Key), filtro.idEmpresa),
                            totalPeriodoAnterior = PesquisaNoticiasTotalPeriodoAnterior(dataInicialPeriodoAnterior, dataFinalPeriodoAnterior, Convert.ToInt32(a.Key), filtro.idEmpresa)
                        }
                        ).Take(10).ToList();
            }
            else
                lista = null;

            return lista;
        }

        private string GetNomeFonte(int id) {
            var nomeFonte = (from f in db.fontes_noticias
                                       where f.idfonte == id
                                       select f.nome
                                       ).FirstOrDefault();

            return nomeFonte;
        }

        public List<Linha> PesquisaGraficoNoticiasPorDiaFonte(DateTime dataInicial, DateTime dataFinal, int idFonte, int idEmpresa)
        {

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var resultado = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                            .Query(q =>
                                        q.Match(m => m.Field("empresas.idempresa").Query(idEmpresa.ToString())) &&
                                        //q.Term("empresas.idempresa", idEmpresa) &&
                                        q.Term("idfonte", idFonte) &&
                                        q.DateRange(d => d
                                        .Field(f => f.datapublicacao)
                                        .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataInicial))
                                        .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataFinal))
                                        ))
                            .Aggregations(a => a.DateHistogram("byday", h => h
                                         .Field("datapublicacao")
                                         .Interval(DateInterval.Day)))
                            );

            var linhas = new List<Linha>();

            if (resultado != null && resultado.Documents.Count > 0)
            {
                linhas = (from item in resultado.Aggs.DateHistogram("byday").Buckets
                          orderby item.Date descending
                          select new Linha()
                          {
                              data = item.Date.Date.ToString("dd/MM/yyyy"),
                              datahora = item.Date,
                              valor = Convert.ToInt32(item.DocCount)
                          }).ToList();
            }
            else
                linhas = null;








            return linhas;
        }

        public int PesquisaNoticiasTotalPeriodoAnterior(DateTime dataInicial, DateTime dataFinal, int idFonte, int idEmpresa)
        {

            var node = new Uri(_servidorElastic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            // Resultado top fontes apartir dos filtros
            // =========================================================================================
            var resultado = client.Search<NoticiasElastic.NoticiaElastic>(s => s
                                .MatchAll()
                                .Query(q =>
                                            q.Match(m => m.Field("empresas.idempresa").Query(idEmpresa.ToString())) &&
                                            //q.Term("empresas.idempresa", idEmpresa) &&
                                            q.Term("idfonte", idFonte) &&
                                            q.DateRange(d => d
                                            .Field(f => f.datapublicacao)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataInicial))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dataFinal))
                                            ))
                                .Aggregations(a => a
                                       .Terms("TotalNoticias", f => f.Field(c => c.idfonte)))
                            );

            return Convert.ToInt32(resultado.Total);

        }

    }
}
