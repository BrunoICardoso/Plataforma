using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class PresencaOnline
    {


        private ZBD.Model.zeengEntities _bd;
        public PresencaOnline()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public PrensencaOnline.PresencaOnlineCaptura RetornaPresencaOnline(int idEmpresa, DateTime anomes)
        {

            var mesAtual = anomes.Month;
            var anoAtual = anomes.Year;

            var presencaOn = new PrensencaOnline.PresencaOnlineCaptura();


            //INDICADORES
            var indicadorAtualBD = (from ind in _bd.presenca_online_capturas
                                    where
                                        ind.idempresa == idEmpresa &&
                                        ind.datacaptura.Month == mesAtual &&
                                        ind.datacaptura.Year == anoAtual
                                    orderby ind.datacaptura descending
                                    select ind).FirstOrDefault();

            if (indicadorAtualBD != null)
            {
                presencaOn.indicadorAtual = new PrensencaOnline.Indicadores()
                {

                    rankBrasil = indicadorAtualBD.rankbrasil != null ? indicadorAtualBD.rankbrasil : 0,
                    rankGlobal = indicadorAtualBD.rankglobal != null ? indicadorAtualBD.rankglobal : 0,
                    autoridadeDominio = indicadorAtualBD.autoridadedominio != null ? indicadorAtualBD.autoridadedominio : 0,
                    autoridadePagina = indicadorAtualBD.autoridadepagina != null ? indicadorAtualBD.autoridadepagina : 0,
                    visitasPesquisa = indicadorAtualBD.visitaspesquisa != null ? indicadorAtualBD.visitaspesquisa : 0,
                    taxaRejeicao = indicadorAtualBD.taxarejeicao != null ? indicadorAtualBD.taxarejeicao : 0,
                    pontuacaoSpam = indicadorAtualBD.pontuacaospam != null ? indicadorAtualBD.pontuacaospam : 0,
                    links = indicadorAtualBD.links != null ? indicadorAtualBD.links : 0,
                    novosLinks = indicadorAtualBD.novoslinks != null ? indicadorAtualBD.novoslinks : 0,
                    paginasVisitadas = indicadorAtualBD.paginasvisitadas != null ? indicadorAtualBD.paginasvisitadas : 0,
                    tempoVisita = indicadorAtualBD.tempovisita != null ? indicadorAtualBD.tempovisita : null
                };

            }

            anomes = anomes.AddMonths(-1);
            var mesAnterior = anomes.Month;
            var anoAnterior = anomes.Year;
            var indicadorAnteriorBD = (from ind in _bd.presenca_online_capturas
                                       where
                                           ind.idempresa == idEmpresa &&
                                           ind.datacaptura.Month == mesAnterior &&
                                           ind.datacaptura.Year == anoAnterior
                                       orderby ind.datacaptura descending
                                       select ind).FirstOrDefault();

            if (indicadorAnteriorBD != null)
            {
                presencaOn.indicadorAnterior = new PrensencaOnline.Indicadores()
                {

                    rankBrasil = indicadorAnteriorBD.rankbrasil != null ? indicadorAnteriorBD.rankbrasil : 0,
                    rankGlobal = indicadorAnteriorBD.rankglobal != null ? indicadorAnteriorBD.rankglobal : 0,
                    autoridadeDominio = indicadorAnteriorBD.autoridadedominio != null ? indicadorAnteriorBD.autoridadedominio : 0,
                    autoridadePagina = indicadorAnteriorBD.autoridadepagina != null ? indicadorAnteriorBD.autoridadepagina : 0,
                    visitasPesquisa = indicadorAnteriorBD.visitaspesquisa != null ? indicadorAnteriorBD.visitaspesquisa : 0,
                    taxaRejeicao = indicadorAnteriorBD.taxarejeicao != null ? indicadorAnteriorBD.taxarejeicao : 0,
                    pontuacaoSpam = indicadorAnteriorBD.pontuacaospam != null ? indicadorAnteriorBD.pontuacaospam : 0,
                    links = indicadorAnteriorBD.links != null ? indicadorAnteriorBD.links : 0,
                    novosLinks = indicadorAnteriorBD.novoslinks != null ? indicadorAnteriorBD.novoslinks : 0,
                    paginasVisitadas = indicadorAnteriorBD.paginasvisitadas != null ? indicadorAnteriorBD.paginasvisitadas : 0,
                    tempoVisita = indicadorAnteriorBD.tempovisita != null ? indicadorAnteriorBD.tempovisita : null
                };
            }

            //AUDIENCIA
            var graficoAudiencia = (from p in _bd.presenca_online_capturas
                                    where
                                    p.idempresa == idEmpresa &&
                                    p.datacaptura.Month == mesAtual &&
                                    p.datacaptura.Year == anoAtual
                                    select p).FirstOrDefault();

            if (graficoAudiencia != null)
            {
                presencaOn.audiencia = new PrensencaOnline.Audiencia()
                {

                    audi_feminino = graficoAudiencia.audi_feminino,
                    audi_masculino = graficoAudiencia.audi_masculino,
                    audi_semfaculdade = graficoAudiencia.audi_semfaculdade,
                    audi_algumafaculdade = graficoAudiencia.audi_algumafaculdade,
                    audi_posgraduacao = graficoAudiencia.audi_posgraduacao,
                    audi_faculdade = graficoAudiencia.audi_faculdade,
                    audi_casa = graficoAudiencia.audi_casa,
                    audi_escola = graficoAudiencia.audi_escola,
                    audi_trabalho = graficoAudiencia.audi_trabalho
                };
            }
            else
            {
                presencaOn.audiencia = new PrensencaOnline.Audiencia()
                {
                    audi_feminino = 0,
                    audi_masculino = 0,
                    audi_semfaculdade = 0,
                    audi_algumafaculdade = 0,
                    audi_posgraduacao = 0,
                    audi_faculdade = 0,
                    audi_casa = 0,
                    audi_escola = 0,
                    audi_trabalho = 0
                };
            }

            //PRINCIPAIS TERMOS e SITES QUE LINKAM SITES EMPRESA

            if (indicadorAtualBD != null)
            {
                if (indicadorAtualBD.presenca_online_palavraspesquisa.Count() > 0)
                {
                    //presencaOn.termos = indicadorAtualBD.presenca_online_palavraspesquisa.Take(7).Select(x => new ZRN.PrensencaOnline.Termos() { palavra = x.palavra, percentual = x.percentual}).ToList();


                    presencaOn.termos = (from pp in indicadorAtualBD.presenca_online_palavraspesquisa
                                         select new ZRN.PrensencaOnline.Termos()
                                         {
                                             palavra = pp.palavra,
                                             percentual = pp.percentual
                                         }).Take(7).ToList();


                }

                if (indicadorAtualBD.presenca_online_linkadopor.Count() > 0)
                {
                    presencaOn.links = indicadorAtualBD.presenca_online_linkadopor.Take(7).Select(x => new ZRN.PrensencaOnline.Links() { link = x.link, site = x.site }).ToList();
                }

            }


            //var listaTermosBD = (
            //                    from t in _bd.presenca_online_palavraspesquisa
            //                    join c in _bd.presenca_online_capturas on t.idcaptura equals c.idcaptura
            //                    where
            //                        !t.excluido &&
            //                        c.idempresa == idEmpresa &&
            //                        c.idcaptura == 
            //                        (from cap in _bd.presenca_online_capturas
            //                            where
            //                            cap.datacaptura.Month == mesAtual &&
            //                            cap.datacaptura.Year == anoAtual
            //                         select cap).Max(x => x.idcaptura)
            //                    select new ZRN.PrensencaOnline.Termos()
            //                    {
            //                        palavra = t.palavra,
            //                        percentual = t.percentual
            //                    }
            //                ).Take(7).ToList();

            //if (listaTermosBD != null)
            //{
            //    presencaOn.termos = listaTermosBD;
            //}
            //else
            //{

            //    presencaOn.termos = null;


            //}

            //var listaLinksBD = (
            //                        from li in _bd.presenca_online_linkadopor
            //                        join c in _bd.presenca_online_capturas on li.idcaptura equals c.idcaptura
            //                        where
            //                            !li.excluido &&
            //                            c.idempresa == idEmpresa &&
            //                            c.datacaptura.Month == mesAtual &&
            //                            c.datacaptura.Year == anoAtual

            //                        orderby c.datacaptura descending
            //                        select new ZRN.PrensencaOnline.Links()
            //                        {
            //                            site = li.site,
            //                            link = li.link
            //                        }
            //                    ).Take(7).ToList();

            //if (listaLinksBD != null)
            //{

            //    presencaOn.links = listaLinksBD;

            //}
            //else
            //{


            //    presencaOn.links = null;

            //}



            return presencaOn;
        }

        public List<Graficos.Area> RetornaPeriodoRankBrasil(int idEmpresa, int qtdeMeses)
        {
            var db = new ZBD.Model.zeengEntities();

            var dtInicial = DateTime.Now.AddMonths(-qtdeMeses).ToString("yyyy-MM-dd");
            var dtFinal = DateTime.Now.ToString("yyyy-MM-dd");

            string query = "CALL GetTotalRankBrasilSemana('" + idEmpresa + "', '" + dtInicial + "', '" + dtFinal + "');";

            List<RankBrasil> rankins = db.Database.SqlQuery<RankBrasil>(query).ToList();

            var lista = new List<Graficos.Area>();

            lista.AddRange((
                        from r in rankins
                        select new Graficos.Area
                        {
                            date = r.data.ToString("yyyy-MM-dd"),
                            valor = Convert.ToInt32(r.rankbrasil)
                        }).ToList());

            return lista;
        }

        public List<Graficos.Area> RetornaPeriodoRankBrasilPorMes(int idEmpresa)
        {
            var db = new ZBD.Model.zeengEntities();

            var dataInicial = DateTime.Now.AddMonths(-11);
            var dtInicial = dataInicial.ToString("yyyy-MM-dd");

            var dataFinal = DateTime.Now;
            var dtFinal = dataFinal.ToString("yyyy-MM-dd");


            string query = "CALL GetTotalRankBrasilMes('" + idEmpresa + "', '" + dtInicial + "', '" + dtFinal + "');";

            List<RankBrasil> rankins = db.Database.SqlQuery<RankBrasil>(query).ToList();
            //----------------
            var dataGrafico = dataInicial.AddDays(1 - dataInicial.Day).Date;

            var dadosGraf = new List<Graficos.Area>();


            var i = 0;
            if (rankins.Any())
            {
                while (dataGrafico.Date < dataFinal.Date)
                {

                    var itemGrafico = new Graficos.Area();

                        if (rankins.Count > i && rankins[i].data == dataGrafico  )
                        {

                            itemGrafico.date = rankins[i].data.ToString("yyyy-MM-dd");

                            if (rankins[i].rankbrasil != null)
                                itemGrafico.valor = rankins[i].rankbrasil.Value;
                            else
                                itemGrafico.valor = 0;

                            i++;

                            dadosGraf.Add(itemGrafico);
                            dataGrafico = dataGrafico.AddMonths(1);
                        }
                        else
                        {

                            itemGrafico.date = dataGrafico.ToString("yyyy-MM-dd");
                            itemGrafico.valor = 0;

                            dadosGraf.Add(itemGrafico);
                            dataGrafico = dataGrafico.AddMonths(1);
                        }

                    }

                

                return dadosGraf;
            }
            else
            {
                return null;
            }


            //----------------------------------


            //int numeroMes = Convert.ToDateTime(dtInicial).Month;

            //var dataGrafico = Convert.ToDateTime(dtInicial).AddMonths(0 - numeroMes);
            //var dadosGraf = new List<Graficos.Area>();
            //while (dataGrafico <= Convert.ToDateTime(dtFinal))
            //{
            //    var valorLinha = new Graficos.Area();
            //    valorLinha.date = dataGrafico.ToString("yyyy-MM-dd");
            //    valorLinha.valor = 0;

            //    var dadosRank = rankins.Where(x =>
            //                                    Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
            //                                    Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year
            //                                ).FirstOrDefault();
            //    if (dadosRank != null)
            //        valorLinha.valor = Convert.ToInt32(dadosRank.rankbrasil);

            //    dadosGraf.Add(valorLinha);
            //    dataGrafico = dataGrafico.AddMonths(1);
            //}

            //return dadosGraf;



        }
    }
}
