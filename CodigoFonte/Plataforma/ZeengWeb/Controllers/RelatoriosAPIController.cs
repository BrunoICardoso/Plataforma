using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZeengWeb.ViewModel.Relatorios;
using ZRN.Mensagem;

namespace ZeengWeb.Controllers
{
    public class RelatoriosAPIController : ApiController
    {

        [HttpGet]
        public relatorioFacebook RetornaMetricasFacebook(int idRelatorio)
        {
            var relatorioFacebook = new relatorioFacebook();
            var RNRelatorios = new ZRN.Relatorios.Relatorios();

            relatorioFacebook.metricasFacebook = RNRelatorios.RetornaMetricasFacebook(idRelatorio);
            relatorioFacebook.metricasRelatorioFacebook = RNRelatorios.retornaMetricasRelatorioFacebook(idRelatorio);

            return relatorioFacebook;
        }

        [HttpGet]
        public relatorioTwitter RetornaMetricasTwitter(int idRelatorio)
        {
            var relatorioTwitter = new relatorioTwitter();
            var RNRelatorios = new ZRN.Relatorios.Relatorios();

            relatorioTwitter.metricasTwitter = RNRelatorios.RetornaMetricasTwitter(idRelatorio);
            relatorioTwitter.metricasRelatorioTwitter = RNRelatorios.retornaMetricasRelatorioTwitter(idRelatorio);

            return relatorioTwitter;
        }

        [HttpGet]
        public relatorioInstagram RetornaMetricasInstagram(int idRelatorio)
        {
            var relatorioInstagram = new relatorioInstagram();
            var RNRelatorios = new ZRN.Relatorios.Relatorios();

            relatorioInstagram.metricasInstagram = RNRelatorios.RetornaMetricasInstagram(idRelatorio);
            relatorioInstagram.metricasRelatorioInstagram = RNRelatorios.retornaMetricasRelatorioInstagram(idRelatorio);

            return relatorioInstagram;
        }

        [HttpGet]
        public relatorioYoutube RetornaMetricasYoutube(int idRelatorio)
        {
            var relatorioYoutube = new relatorioYoutube();
            var RNRelatorios = new ZRN.Relatorios.Relatorios();

            relatorioYoutube.metricasYoutube = RNRelatorios.RetornaMetricasYoutTube(idRelatorio);
            relatorioYoutube.metricasRelatorioYoutube = RNRelatorios.retornaMetricasRelatorioYoutube(idRelatorio);

            return relatorioYoutube;
        }

        [HttpGet]
        public ZRN.Relatorios.Relatorio RetornaRelatorio(int idRelatorio)
        {
            var RNRelatorios = new ZRN.Relatorios.Relatorios();
            return RNRelatorios.RetornaRelatorio(idRelatorio);
        }

        [HttpPut]
        public ZRN.Mensagem.Mensagem Editar(ZRN.Relatorios.Relatorio relatorio)
        {
            var RNRelatorios = new ZRN.Relatorios.Relatorios();
            return RNRelatorios.Editar(relatorio); ;
        }


        [HttpPost]
        public ZRN.Relatorios.ListaRelatorio Pesquisar(ZRN.Relatorios.FiltroRelatorio filtro)
        {
            var rn = new ZRN.Relatorios.Relatorios();
            return rn.RetornaRelatorios(filtro);
        }

        [HttpGet]
        public bool Excluir(int idRelatorio)
        {
            var rn = new ZRN.Relatorios.Relatorios();
            return rn.excluir(idRelatorio);
        }

        public void GerarCSV(int idRelatorio, string nomeRedeSocial)
        {
            var rn = new ZRN.Relatorios.Relatorios();
            //rn.gerarCSV(idRelatorio, nomeRedeSocial, Utils.Configuracoes.);
		}

        public Mensagem CriarRelatorio(ZRN.Relatorios.CadastroRelatorio relatorio)
        {
            var RelatorioRN = new ZRN.Relatorios.Relatorios();
            return RelatorioRN.CriarRelatorio(relatorio);
        }

        [HttpGet]
        public relatorioPresencaOnlineNoticia RetornaMetricasPrersencaOnlineNoticia(int idRelatorio)
        {
            var relatorioPresencaNoticia = new relatorioPresencaOnlineNoticia();
            var RNrelatorios = new ZRN.Relatorios.Relatorios();

            relatorioPresencaNoticia.metricasPresencaNoticia = RNrelatorios.RetornaMetricasPresencaOnlineNoticia(idRelatorio);
            relatorioPresencaNoticia.metricasRelatorioPresencaNoticia = RNrelatorios.retornaMetricasRelatorioPresencaOnlineNoticia(idRelatorio);

            return relatorioPresencaNoticia;
        }
        
    }
}
