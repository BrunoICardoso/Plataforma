using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBD.Model;

namespace ZRN.Empresas
{
    public class Empresas
    {
        private int _totalEmpresas;
        private string _servidorElastic = "";
        private string _indexElastic = "";

        public Empresas(string servidorElastic, string indexElastic)
        {
            _bd = new ZBD.Model.zeengEntities();

            _servidorElastic = servidorElastic;
            _indexElastic = indexElastic;
        }

        public int TotalEmpresas
        {
            get { return _totalEmpresas; }
            set { _totalEmpresas = value; }
        }

        public enum CampoOrdenacao
        {
            Nome = 1,
            Setor = 2
        }

        public enum SentidoOrdenacao
        {
            Crescente = 1,
            Decrescente = 2
        }

        private ZBD.Model.zeengEntities _bd;
        public Empresas()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public Empresa RetornaPerfilEmpresa(int idEmpresa)
        {
            // var dataIni = DateTime.Now.AddMonths(-12);
            var dataAnoAnterior = DateTime.Now.AddYears(-1);
            dataAnoAnterior = dataAnoAnterior.AddMonths(1);
            var dataIni = new DateTime(dataAnoAnterior.Year, dataAnoAnterior.Month, 1);

            var dataFim = DateTime.Now;

            return RetornaPerfilEmpresa(idEmpresa, dataIni, dataFim);
        }

        public Empresa RetornaPerfilEmpresa(int idEmpresa, DateTime dataIni, DateTime dataFim)
        {
            var empresa = (from e in _bd.empresas
                           where !e.excluido && e.idempresa == idEmpresa
                           select new Empresa()
                           {
                               idempresa = e.idempresa,
                               idsetor = e.idsetor,
                               nomeSetor = e.setoresempresa.nome,
                               nome = e.nome,
                               descricao = e.descricao,
                               urlsite = e.urlsite,

                               marcas = (from m in e.marcas
                                         select new Marca.Marca() { idmarca = m.idmarca, nome = m.nome }).AsEnumerable(),
                               imagem = e.imagem,

                               redesesociais = (from r in e.empresaredessociais
                                                select new RedesSociais.RedeSocial() { idredesocial = r.idredesocial, urlredesocial = r.urlredesocial, nomeredesocial = r.redessociais.nome }
                                                ).AsEnumerable()

                           }).SingleOrDefault();

            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada");
            }

            return empresa;
        }

        public Empresa RetornaDescricaoEmpresa(int idEmpresa)
        {

            var empresa = (from e in _bd.empresas
                           where e.idempresa == idEmpresa
                           select new Empresa()
                           {
                               nome = e.nome,
                               descricao = e.descricao,
                               urlsite = e.urlsite,
                               imagem = e.imagem,
                               nomeSetor = e.setoresempresa.nome

                           }).FirstOrDefault();


            return empresa;
        }

        private class TotalMapaSemana
        {

            public string Semana { get; set; }
            public int Total { get; set; }
        }

        private class TotalMapaMes
        {
            public string Mes { get; set; }
            public int Total { get; set; }
        }

        public List<Graficos.Linha> RetornaGraficoLancamentoProdutos(int idEmpresa, int QtdeMeses = 12)
        {

            var dtInicial = DateTime.Now.AddMonths(-QtdeMeses);
            var dtFinal = DateTime.Now;

            //return RetornaGraficoLancamentoProdutos(idEmpresa, dtInicial, dtFinal);
            return RetornaGraficoLancamentoProdutosPorAno(idEmpresa, dtInicial, dtFinal);
        }

        public List<Graficos.Linha> RetornaGraficoLancamentoProdutos(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTotalMapaPorSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<TotalMapaSemana> dadosMapa = _bd.Database.SqlQuery<TotalMapaSemana>(query).ToList();


            int diaSemana = (int)dtInicial.DayOfWeek;

            var dataGrafico = dtInicial.AddDays(0 - diaSemana);
            var dadosGraf = new List<Graficos.Linha>();
            while (dataGrafico <= dtFinal)
            {

                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "MAPA";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var dadosEmpresa = dadosMapa.Where(x => Convert.ToDateTime(x.Semana).Date == dataGrafico.Date).FirstOrDefault();

                if (dadosEmpresa != null)
                {
                    valorLinha.valor = dadosEmpresa.Total;
                }

                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddDays(7);

            }
            return dadosGraf;
        }

        public List<Graficos.Linha> RetornaGraficoLancamentoProdutosPorAno(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTotalMapaPorMes('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<TotalMapaMes> dadosMapa = _bd.Database.SqlQuery<TotalMapaMes>(query).ToList();

            int numeroMes = (int)dtInicial.Month;

            var dataGrafico = dtInicial.AddMonths(0 - numeroMes);
            var dadosGraf = new List<Graficos.Linha>();
            while (dataGrafico <= dtFinal)
            {
                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "MAPA";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var dadosEmpresa = dadosMapa.Where(x =>
                                                        Convert.ToDateTime(x.Mes).Date.Month == dataGrafico.Date.Month &&
                                                        Convert.ToDateTime(x.Mes).Date.Year == dataGrafico.Date.Year
                                                    ).FirstOrDefault();

                if (dadosEmpresa != null)
                {
                    valorLinha.valor = dadosEmpresa.Total;
                }

                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddMonths(1);

            }
            return dadosGraf;
        }

        public IEnumerable<Empresa> RetornaEmpresas(FiltroEmpresas filtro, CampoOrdenacao ordernacao, SentidoOrdenacao sentidoord)
        {

            var RNCliente = new Clientes.Clientes();


            var empresasBD = (from e in _bd.empresas
                              join ea in _bd.cliente_empresas on e.idempresa equals ea.idempresa
                              where !e.excluido && ea.idcliente == filtro.idcliente && ea.excluido == false &&
                              (
                              e.nome.ToLower() == filtro.pesquisa.ToLower() ||
                              e.nome.Contains(filtro.pesquisa.ToLower()) ||
                              filtro.pesquisa == "" ||
                              filtro.pesquisa == null
                              )
                              select e);


            if (RNCliente.AcessoTodasEmpresas(filtro.idcliente) == true)
            {
                empresasBD = (from e in _bd.empresas
                              where !e.excluido &&
                               (
                               e.nome.ToLower() == filtro.pesquisa.ToLower() ||
                               e.nome.Contains(filtro.pesquisa.ToLower()) ||
                               filtro.pesquisa == "" ||
                               filtro.pesquisa == null
                               )
                              select e);
            }



            var rncliente = new ZRN.Clientes.Clientes();

            var vertentes = rncliente.RetornaVertentesAssociada(filtro.idcliente);

            switch (ordernacao)
            {
                case CampoOrdenacao.Nome:
                    if (sentidoord == SentidoOrdenacao.Crescente)
                    {
                        empresasBD = empresasBD.OrderBy(x => x.nome);
                    }
                    else
                    {
                        empresasBD = empresasBD.OrderByDescending(x => x.nome);
                    }
                    break;
                case CampoOrdenacao.Setor:
                    if (sentidoord == SentidoOrdenacao.Crescente)
                    {
                        empresasBD = empresasBD.OrderBy(x => x.setoresempresa.nome);
                    }
                    else
                    {
                        empresasBD = empresasBD.OrderByDescending(x => x.setoresempresa.nome);
                    }
                    break;
                default:
                    empresasBD = empresasBD.OrderBy(x => x.nome);
                    break;
            }
            _totalEmpresas = empresasBD.Count();
            if (_totalEmpresas > 0)
            {
                empresasBD = empresasBD.Skip(filtro.regInicial).Take(filtro.qtdregistros);

                var dataAnoAnterior = DateTime.Now.AddYears(-1);
                dataAnoAnterior = dataAnoAnterior.AddMonths(1);
                var dataIni = new DateTime(dataAnoAnterior.Year, dataAnoAnterior.Month, 1);

                //  var dataIni = DateTime.Now.AddYears(-1);
                var dataFim = DateTime.Now;

                var RNNoticias = new ZRN.Empresas.Noticias(_servidorElastic, _indexElastic);
                var RNPromocoes = new ZRN.Empresas.Promocoes();


                var empresas = (from e in empresasBD.ToList()
                                select new Empresa()
                                {
                                    idempresa = e.idempresa,
                                    idsetor = e.idsetor,
                                    nomeSetor = e.setoresempresa.nome,
                                    nome = e.nome,
                                    descricao = e.descricao,
                                    urlsite = e.urlsite,
                                    imagem = e.imagem,
                                    VertenteClientes = new Clientes.ClienteVertentes()
                                    {
                                        noticias = vertentes.noticias,
                                        produtos = vertentes.produtos,
                                        promocoes = vertentes.promocoes,
                                        redessociais = vertentes.redessociais,
                                        presencaonline = vertentes.presencaonline
                                    },

                                    qtdLancamentos = vertentes.produtos == true ? (from mapaRegistro in _bd.mapa_registros
                                                                                   join empresaMapa in _bd.mapa_registro_empresa on mapaRegistro.idregistro equals empresaMapa.idregistro
                                                                                   where empresaMapa.idempresa == e.idempresa && mapaRegistro.dataconcessao >= dataIni && mapaRegistro.dataconcessao <= dataFim
                                                                                   select mapaRegistro).Count() : 0,

                                    qtdNoticias = vertentes.noticias == true ? RNNoticias.RetornaQuantidadeNoticiasMesElastic(e.idempresa, dataIni, dataFim) : 0,

                                    qtdPromocoes = vertentes.promocoes == true ? RNPromocoes.RetornaQtdPromocoesUltimosDozeMeses(e.idempresa) : 0,

                                    rankBrasil = vertentes.presencaonline == true ? (from pOnline in _bd.presenca_online_capturas
                                                                                     where pOnline.datacaptura >= dataIni &&
                                                                                           pOnline.datacaptura <= dataFim &&
                                                                                           pOnline.idempresa == e.idempresa
                                                                                     orderby pOnline.datacaptura descending
                                                                                     select pOnline.rankbrasil).FirstOrDefault() : 0,

                                }).ToList();

                List<int> listaIds = new List<int>();

                foreach (var e in empresas)
                {
                    listaIds.Add(e.idempresa);
                }

                string listaIdsEmpresa = String.Join(",", listaIds);

                string dataInial = dataIni.ToString("yyyy-MM-dd");
                string dataFinal = dataFim.ToString("yyyy-MM-dd");

                if (vertentes.redessociais == true)
                {
                    string query = "CALL GetTotalPublicacoesRedesSociais('" + listaIdsEmpresa + "', '" + dataInial + "', '" + dataFinal + "');";

                    var resultados = _bd.Database.SqlQuery<TotalInteracoes>(query).ToList();

                    foreach (var empresa in empresas)
                    {
                        if (resultados != null)
                        {
                            var totalPublicacoes = resultados.FirstOrDefault(r => r.idEmpresa == empresa.idempresa);
                            if (totalPublicacoes != null)
                            {
                                empresa.qtdPublicacoes = Convert.ToInt64(totalPublicacoes.totalGeral);
                            }
                        }
                    }

                }


                return empresas;

            }
            else
            {
                return null;
            }
        }

        public List<NoticiasEmpresa> RetornaEmpresasNoticia(int idNoticia)
        {

            var db = new ZBD.Model.zeengEntities();

            var listaEmpresas = (
                                    from ne in db.noticias_empresa
                                    join e in db.empresas on ne.idempresa equals e.idempresa
                                    where ne.idnoticia == idNoticia
                                    select new NoticiasEmpresa()
                                 ).ToList();

            return listaEmpresas;
        }

        public EmpresaNoticia RetornaEmpresaNoticia(int id)
        {

            //var NoticiaEmpresa = (from ne in _bd.noticias_empresa
            //                      where ne.idnoticiaempresa == id
            //                      select new EmpresaNoticia()
            //                      {
            //                          titulo = ne.noticias.titulo,
            //                          //Empresas = (_bd.noticias_empresa.Where(x => x.idnoticia == ne.idnoticia).Select(n => new Empresa() { idempresa = n.idempresa.Value, nome = n.empresas.nome }).ToList()),

            //                          //Imagens = ne.noticias.noticia_imagens.Select(x => new ZRN.Noticias.NoticiaImagem() {titulo = x.titulo, creditos = x.creditos, url  = x.url }).ToList(),
            //                          Imagens = (
            //                                        from i in ne.noticias.noticia_imagens
            //                                        select new ZRN.Noticias.NoticiaImagem()
            //                                        {
            //                                            titulo = i.titulo,
            //                                            url = i.url,
            //                                            creditos = i.creditos
            //                                        }
            //                                    ).AsEnumerable(),

            //                          nomeFonte = ne.noticias.fontes_noticias.nome,
            //                          subtitulo = ne.noticias.subtitulo,
            //                          //datapublicacao = ne.noticias.datapublicacao.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm"),
            //                          //datapublicacao = DateTime.ParseExact(ne.noticias.datapublicacao.ToString(),"dd/MM/yyyy HH:mm",null),
            //                          datapublicacao = ne.noticias.datapublicacao,
            //                          conteudo = ne.noticias.conteudo,
            //                          autor = ne.noticias.autor,
            //                          url = ne.noticias.url,
            //                          idNoticiaEmpresa = ne.idnoticiaempresa,
            //                          nomeEmpresa = ne.empresas.nome,
            //                          idEmpresa = ne.idempresa
            //                      }
            //                      ).FirstOrDefault();


            return null;
        }

        public List<TagNoticia> RetornaTagsNoticia(int idNoticia)
        {

            var db = new ZBD.Model.zeengEntities();

            var listaTags = (from empresa in db.empresas
                             join noticiaEmpresa in db.noticias_empresa on empresa.idempresa equals noticiaEmpresa.idempresa
                             where noticiaEmpresa.idnoticia == idNoticia
                             select new TagNoticia()
                             {
                                 valor = empresa.idempresa,
                                 termo = empresa.nome
                             }).ToList();

            return listaTags;

        }

        public List<ZRN.Combos.ItemCombo> RetornaListaEmpresas(int idCliente)
        {
            var RNCliente = new Clientes.Clientes();

            dynamic resultado;

            if (RNCliente.AcessoTodasEmpresas(idCliente) == true)
            {
                resultado = (from e in _bd.empresas
                             where !e.excluido
                             orderby e.nome
                             select new ZRN.Combos.ItemCombo() { id = e.idempresa, nome = e.nome }).ToList();
            }
            else
            {
                resultado = (from empresa in _bd.empresas
                             join clienteempresa in _bd.cliente_empresas
                             on empresa.idempresa equals clienteempresa.idempresa
                             where !empresa.excluido && clienteempresa.idcliente == idCliente && clienteempresa.excluido == false
                             orderby empresa.nome
                             select new ZRN.Combos.ItemCombo() { id = empresa.idempresa, nome = empresa.nome }).ToList();
            }

            return resultado;
        }
    }
}
