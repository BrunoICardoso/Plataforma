using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZeengWeb.Utils;
using ZeengWeb.ViewModel.Promoções;

namespace ZeengWeb.Controllers.Empresa
{
    public class PromocoesAPIController : ApiController
    {

        public List<PromocoesRecentes> RetornaPromocoesRecentes(ZRN.Promocoes.FiltroPromocoes filtro) { 
        
            var RNPromos = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic,Configuracoes.IndexElastic);
            var promocoes = RNPromos.RetornaPromocoes(filtro);

            List<PromocoesRecentes> listaPromos = new List<PromocoesRecentes>();

            foreach (var promocao in promocoes)
            {
                var promo = new PromocoesRecentes() {
                    idPromocao = promocao.idPromocao,
                    nomePromocao = promocao.nomePromocao,
                    dataVigenciaInicial = promocao.dtVigenciaIni.Date,
                    dataVigenciaFinal = promocao.dtVigenciaFim.Date,
                    modalidade = promocao.NomeModalidade,
                    abrangencia = (promocao.abrangencia_nacional == true ? "Nacional" :                                                             
                                                                     (promocao.abrangestados.Any() ?
                                                                             string.Join(", ",promocao.abrangestados.Select(x => x.uf).ToList()) +
                                                                                        (promocao.abrangmunicipios.Any() ? " " + string.Join(", ",promocao.abrangmunicipios.Select(x => x.uf + "/" + x.nome).ToList()) : "")
                                                                     : (promocao.abrangmunicipios.Any() ? string.Join(", ", promocao.abrangmunicipios.Select(x => x.uf + "/" + x.nome).ToList()) : ""))
                                  )
                };
                listaPromos.Add(promo);
            }
            return listaPromos.OrderBy(x => x.dataVigenciaInicial).ToList();
        }

        public List<ZRN.PromocoesElastic.Noticia> RetornaNoticiasPromocoes(ZRN.Promocoes.FiltroPromocoes filtro)
        {
            var RNPromos = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);                        
            return RNPromos.RetornaNoticias(filtro); 
        }
        
        [HttpGet]
        public PromocoesCombos RetornaCombos()
        {
            var RNPromos = new ZRN.Promocoes.Promocoes();
            var combos = new PromocoesCombos();

            combos.modalidades = RNPromos.RetornaItensModalidades();
            combos.abrangencia = RNPromos.RetornaItensAbrangencia();

            return combos;
        }


        public List<ZRN.Graficos.ItemRosca> RetornaGraficoModalidade(ZRN.Promocoes.FiltroPromocoes filtro)
        {
            var RNPromos = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNPromos.RetornaGraficoModalidade(filtro);			
		}
		
        [HttpPost]
        public List<ZRN.Graficos.Barra> GetGraficoVigentes(ZRN.Promocoes.FiltroPromocoes filtro)
        {
            var rn = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return rn.RetornaGraficoVigencia(filtro);
        }

        [HttpPost]
        public ZRN.Graficos.Mapas.Brasil GetGraficoBrasil(ZRN.Promocoes.FiltroPromocoes filtro)
        {

            var RNPromos = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            var GraficoBrasil = RNPromos.RetornaPromocoesPorEstado(filtro);
            return GraficoBrasil;
        }


        public void GetPostagensRecentes(ZRN.Promocoes.FiltroPromocoes filtro)
        {

        }

        public void GetNoticiasRecentes(ZRN.Promocoes.FiltroPromocoes filtro)
        {

        }

        public List<ZRN.Promocoes.PostPromocao> RetornaPostagensRecentesPromocoes(ZRN.Promocoes.FiltroPromocoes filtro)
        {
            var RNPromos = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNPromos.RetornaPostagensPromocao(filtro);
        }

        public List<ZRN.Promocoes.fb_post> GetPostsFacebook(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            var RNface = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNface.retornaPostsFacebook(idPromocao, idEmpresa, qtdeRegistros, pagina);
        }

        public int GetTotalPostsFacebook(int idPromocao, int idEmpresa)
        {
            var RNface = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNface.retornaTotalPostsFacebook(idPromocao, idEmpresa);
        }

        public List<ZRN.Promocoes.tw_post> GetPostsTwitter(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            var RNtw = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNtw.retornaPostsTwitter(idPromocao, idEmpresa, qtdeRegistros, pagina);
        }

        public int GetTotalPostsTwitter(int idPromocao, int idEmpresa)
        {
            var RNface = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNface.retornaTotalPostsTwitter(idPromocao, idEmpresa);
        }

        public List<ZRN.Promocoes.insta_post> GetPostsInstagram(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            var RN = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.retornaPostsInstagram(idPromocao, idEmpresa, qtdeRegistros, pagina);
        }

        public int GetTotalPostsInstagram(int idPromocao, int idEmpresa)
        {
            var RN = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.retornaTotalPostsInstagram(idPromocao, idEmpresa);
        }

        public List<ZRN.Promocoes.yt_video> GetPostsYoutube(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            var RN = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.retornaPostsYoutube(idPromocao, idEmpresa, qtdeRegistros, pagina);
        }

        public int GetTotalPostsYoutube(int idPromocao, int idEmpresa)
        {
            var RN = new ZRN.Promocoes.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.retornaTotalPostsYoutube(idPromocao, idEmpresa);
        }

        [HttpPost]
        public List<ZRN.PromocoesElastic.Noticia> GetNoticias(ZRN.Promocoes.FiltroPromocaoNoticia filtro)
        {
            var RN = new ZRN.Promocoes.PromocaoNoticias(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.RetornaNoticias(filtro);
        }

        [HttpPost]
        public int GetTotalNoticias(ZRN.Promocoes.FiltroPromocaoNoticia filtro)
        {
            var RN = new ZRN.Promocoes.PromocaoNoticias(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.RetornaTotalNoticias(filtro);
        }

        [HttpPost]
        public ZRN.Promocoes.PesquisaPromocoes RetornaTimelinePromocoes(ZRN.Promocoes.FiltroPromocoes filtro) {

            var RN = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            return RN.RetornaPromocoesTimeLine(filtro);
        }

    }
}
