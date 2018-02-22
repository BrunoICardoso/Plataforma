using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.PrensencaOnline
{
    public class PresencaOnlineCaptura
    {

        public Indicadores indicadorAtual { get; set; }
        public Indicadores indicadorAnterior { get; set; }

        //INDICADORES

        public int? diferencaRankBrasil
        {
            get
            {

                if (indicadorAnterior == null || indicadorAtual == null)
                    return 0;
                
                if (indicadorAnterior.rankBrasil != null  && indicadorAtual.rankBrasil != null)
                {
                    return indicadorAtual.rankBrasil - indicadorAnterior.rankBrasil;
                }
                else
                {
                    return 0;
                }

            }
        }
        
        public int? diferencaRankGlobal
        {
            get
            {
                if (indicadorAnterior != null && indicadorAtual != null && indicadorAnterior.rankGlobal != null && indicadorAtual.rankGlobal != null)
                {
                    return indicadorAtual.rankGlobal - indicadorAnterior.rankGlobal;
                }
                else
                {
                    return 0;
                }

            }
        }

        //public int? diferencaAutoridadeDominio
        //{
        //    get
        //    {
        //        if (indicadorAnterior != null)
        //        {
        //            return indicadorAtual.autoridadeDominio - indicadorAnterior.autoridadeDominio;
        //        }
        //        else
        //        {
        //            return null;
        //        }                
        //    }
        //}

        //public int? diferencaAutoridadePagina
        //{
        //    get
        //    {
        //        if (indicadorAnterior != null)
        //        {
        //            return indicadorAtual.autoridadePagina - indicadorAnterior.autoridadePagina;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        public string diferencaVisitasPesquisa
        {
            get
            {
                if (indicadorAnterior != null && indicadorAtual != null && indicadorAnterior.visitasPesquisa != 0 && indicadorAnterior.visitasPesquisa!=null && indicadorAtual.visitasPesquisa!=0 && indicadorAtual.visitasPesquisa != null)
                {
                    return ((((Convert.ToDecimal(indicadorAtual.visitasPesquisa) / Convert.ToDecimal(indicadorAnterior.visitasPesquisa)) - 1) * 100)).ToString("0,00");
                }
                else
                {
                    return "0";
                }

            }
        }

        public Decimal? diferencaTaxaRejeicao
        {

            get
            {
                if (indicadorAnterior != null && indicadorAtual != null && indicadorAtual.taxaRejeicao!=null && indicadorAnterior.taxaRejeicao != null)
                {
                    return indicadorAtual.taxaRejeicao - indicadorAnterior.taxaRejeicao;
                }
                else
                {
                    return 0;
                }

            }

        }

        //public string diferencaPontuacaoSpam
        //{
        //    get
        //    {
        //        if (indicadorAnterior != null)
        //        {
        //            return (((Convert.ToDouble(indicadorAtual.pontuacaoSpam) / Convert.ToDouble(indicadorAnterior.pontuacaoSpam)) - 1) * 100).ToString("0,00");
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //}

        //public string diferencaLinks
        //{
        //    get
        //    {
        //        if (indicadorAnterior != null)
        //        {
        //            return (((Convert.ToDouble(indicadorAtual.links) / Convert.ToDouble(indicadorAnterior.links)) - 1) * 100).ToString("0,00");
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //}

        //public string diferencaNovosLinks
        //{
        //    get
        //    {
        //        if (indicadorAnterior != null)
        //        {
        //            return (((Convert.ToDouble(indicadorAtual.novosLinks) / Convert.ToDouble(indicadorAnterior.novosLinks)) - 1) * 100).ToString("0,00");
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        public string diferencaPaginasVisitadas
        {
            get
            {
                if (indicadorAnterior != null && indicadorAtual != null &&  indicadorAtual.paginasVisitadas!= null && indicadorAnterior.paginasVisitadas != null)
                {
                    return ((((Convert.ToDecimal(indicadorAtual.paginasVisitadas) / Convert.ToDecimal(indicadorAnterior.paginasVisitadas)) - 1) * 100)).ToString("0.00");
                }
                else
                {
                    return "0";
                }
            }
        }

        public string diferencaTempoVisita
        {
            get
            {
                if (indicadorAnterior != null && indicadorAtual != null && indicadorAtual.tempoVisita != null && indicadorAnterior.tempoVisita != null)
                {


                    if(indicadorAtual.tempoVisita != null && indicadorAnterior.tempoVisita != null)
                    {
                        TimeSpan t1 = (TimeSpan)indicadorAtual.tempoVisita;
                        TimeSpan t2 = (TimeSpan)indicadorAnterior.tempoVisita;

                        return (((Convert.ToDouble(t1.Ticks) / Convert.ToDouble(t2.Ticks)) - 1) * 100).ToString("0.00");
                    }
                    else
                    {
                        return "0";
                    }
                        

                }
                else
                {
                    return "0";
                }


            }
        }


        //AUDIENCIA
        public Audiencia audiencia { get; set; }

        //PRINCIPAIS TERMOS e SITES QUE LINKAM SITES EMPRESA
        public List<Termos> termos { get; set; }
        public List<Links> links { get; set; }


    }
}
