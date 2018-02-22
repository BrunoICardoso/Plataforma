using System;
using System.Collections.Generic;
using System.Web.Http;
using ZeengWeb.Utils;
using ZeengWeb.ViewModel.Empresa;
using ZRN.Empresas;

namespace ZeengWeb.Controllers
{
    public class EmpresaAPIController : ApiController
    {
        public EmpresaListar RetornaEmpresas(FiltroEmpresas filtro)
        {
            var RNEmpresa = new ZRN.Empresas.Empresas(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            filtro.regInicial = (filtro.pagina - 1) * filtro.qtdregistros;
            
            var empListar = new EmpresaListar();
            empListar.Empresas = RNEmpresa.RetornaEmpresas(filtro, ZRN.Empresas.Empresas.CampoOrdenacao.Nome, ZRN.Empresas.Empresas.SentidoOrdenacao.Crescente);
            empListar.TotalEmpresas = RNEmpresa.TotalEmpresas;

            return empListar;

        }
        
        public List<ZRN.Graficos.Linha> GetGraficoLancamentoProdutos(int idEmpresa, int QtdeMeses = 12)
        {

            var RN = new ZRN.Empresas.Empresas();

            return RN.RetornaGraficoLancamentoProdutos(idEmpresa, QtdeMeses);
        }

        public List<ZRN.Graficos.Linha> GetGraficoInteracoesRedesSociais(int idEmpresa, int qtdeMeses)
        {
            var RN = new ZRN.RedesSociais.RedesSociais();

            //  var dtIni = DateTime.Now.AddMonths(-qtdeMeses);
            var dataAnoAnterior = DateTime.Now.AddMonths(-qtdeMeses);
            dataAnoAnterior = dataAnoAnterior.AddMonths(1);
            var dtIni = new DateTime(dataAnoAnterior.Year, dataAnoAnterior.Month, 1);

            var dtFim = DateTime.Now;

            return RN.RetornaGraficoInteracoesEmpresa(idEmpresa, dtIni, dtFim);
        }

        public int RetornaQuantidadeNoticias(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var RNEmpresa = new ZRN.Empresas.Noticias();

            return RNEmpresa.RetornaQuantidadeNoticias(idEmpresa, dtInicial, dtFinal);

        }

        public List<ZRN.Graficos.Linha> GetNoticiasUltimosMeses(int idEmpresa, int qtdeMeses)
        {

            var dtInicial = DateTime.Now.AddMonths(1 - qtdeMeses).AddDays(1 - DateTime.Now.Day);
            var dtFinal = DateTime.Now;
            
            var ZRNEmpresa = new ZRN.Empresas.Noticias();

            return ZRNEmpresa.RetornaNoticiasPorMes(idEmpresa, dtInicial, dtFinal);

        }


        public List<NoticiasEmpresa> GetEmpresasNoticia(int idNoticia)
        {
            var empresas = new ZRN.Empresas.Empresas();
            return empresas.RetornaEmpresasNoticia(idNoticia);
        }

        public List<TagNoticia> GetTagsNoticia(int idNoticia)
        {
            var empresas = new ZRN.Empresas.Empresas();
            return empresas.RetornaTagsNoticia(idNoticia);
        }
        
        public List<ZRN.Graficos.TagCloud.Termo> RetornaPrincipaisTermosNoticias(FiltroNoticiasEmpresa filtro)
        {
            var RNNoticiaEmpresa = new ZRN.Empresas.Noticias(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            return RNNoticiaEmpresa.RetornaPrincipaisTermosNoticias(filtro);
        }

        public ZRN.Empresas.NoticiasEmpresa PesquisaNoticiasEmpresa(ZRN.Empresas.FiltroNoticiasEmpresa filtro)
        {
            var RNEmpresaNoticias = new Noticias(ZeengWeb.Utils.Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            var x = RNEmpresaNoticias.Pesquisar(filtro);

            return x;
        }

        public List<ZRN.Graficos.Linha> PesquisaNoticiasPorDia(FiltroNoticiasEmpresa filtro)
        {
            var ZRNEmpresaNoticias = new Noticias(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            return ZRNEmpresaNoticias.PesquisaNoticiasPorDia(filtro);
        }

        public List<ZRN.Empresas.NoticiasFonte> PesquisaTopFontesNoticias(FiltroNoticiasEmpresa filtro)
        {

            var ZRNEmpresa = new ZRN.Empresas.Noticias(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            return ZRNEmpresa.PesquisaTopFontes(filtro);

        }

        public ZRN.PrensencaOnline.PresencaOnlineCaptura GetPesquisaPresencaOnline(int idEmpresa, DateTime Data)
        {
            var PresencaOnline = new ZRN.Empresas.PresencaOnline();
            return PresencaOnline.RetornaPresencaOnline(idEmpresa, Data);
        }

        public List<ZRN.Graficos.Area> GetRankBrasil(int idEmpresa, int qtdeMeses)
        {
            var rank = new ZRN.Empresas.PresencaOnline();
            //return rank.RetornaPeriodoRankBrasil(idEmpresa, qtdeMeses);
            return rank.RetornaPeriodoRankBrasilPorMes(idEmpresa);

        }

        public List<ZRN.Promocoes.DadosAbrangencia> RetornaGraficoAbrangenciaPromocao(ZRN.Promocoes.FiltroProcessoSeae filtro)
        {
            var dados = new ZRN.Promocoes.Processos(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return dados.RetornaDadosAbrangencia(filtro);
            
        }

        public List<ZRN.Promocoes.ItemGraficoSituacoes> GraficoSituacoes(ZRN.Promocoes.FiltroProcessoSeae filtro)
        {
            var ZRNPromocoes = new ZRN.Empresas.PromocoesElastic(ZeengWeb.Utils.Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return ZRNPromocoes.RetornaItensGraficoSituacoes(filtro);
            
        }

        // public List<ZRN.Graficos.Linha> GraficoPromocoesAtivas()
        public List<ZRN.Graficos.Linha> GraficoPromocoesAtivas(ZRN.Promocoes.FiltroProcessoSeae filtro)
        {
            var ZRNPromocoes = new ZRN.Empresas.PromocoesElastic(ZeengWeb.Utils.Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return ZRNPromocoes.RetornaGraficoPromocoesAtivasSemana(filtro);

        }

        [HttpGet]
        public List<ZRN.Graficos.ItemRosca> RetornaGraficoModalidadePerfilEmpresa(int idEmpresa) {

            var ZRNPromocoes = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            return ZRNPromocoes.RetornaGraficoModalidadePerfilEmpresa(idEmpresa);
        }


        public EmpresaPromocoesTimeLine RetornaLinhadoTempo(ZRN.Promocoes.FiltroProcessoSeae filtro ) {

            var ZRNPromocoes = new ZRN.Promocoes.Processos(ZeengWeb.Utils.Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            var ProcessosTimeline = new EmpresaPromocoesTimeLine()
            {

                ListaProcessos = ZRNPromocoes.RetornaProcessosTimeLine(filtro),
                TotalProcessos = ZRNPromocoes.RetornaTotalProcessosTimeLine()

            }; 
            
            return ProcessosTimeline;
        }

        public List<ZRN.Combos.ItemCombo> GetEmpresasCliente(int idCliente)
        {
            var ZRNEmpresas = new ZRN.Empresas.Empresas();
            return ZRNEmpresas.RetornaListaEmpresas(idCliente);
        }

    }
}
