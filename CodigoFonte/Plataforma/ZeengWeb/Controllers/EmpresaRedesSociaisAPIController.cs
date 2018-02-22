using System;
using System.Collections.Generic;
using System.Web.Http;
using ZeengWeb.ViewModel.Empresa;
using ZeengWeb.Utils;

namespace ZeengWeb.Controllers
{
    public class EmpresaRedesSociaisAPIController : ApiController
    {
        public string _caminhoImagem = Configuracoes.DiretorioImagens;

        //FB
        public List<ZRN.Graficos.Linha> GetGraficoFaceCrescimentoFas(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var dadosGraf = RN.RetornaGraficoCrescimentoFas(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public List<ZRN.RedesSociais.Facebook.Interacoes> GetFacebookInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var dadosGraf = RN.RetornaGraficoQuantidadeInteracoes(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }
        
        public EmpresaFacebookPosts GetFacebookPostsMaisEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina)
        {
            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var facePost = new EmpresaFacebookPosts()
            {
                Posts = RN.RetornaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, postsPagina, pagina, _caminhoImagem),
                TotalPosts = RN.RetornaTotalPosts(idEmpresa, dtInicial, dtFinal)
             
            };

            return facePost;
        }

        public EmpresaFacebookPosts GetFacebookPostsEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina, string ordenacao, string palavra, string PostComentario)
        {
            
            var RNRedes = new ZRN.RedesSociais.Facebook.Facebook();

            var facePost = new EmpresaFacebookPosts()
            {
                Posts = RNRedes.RetornaPostsEmpresa(idEmpresa, dtInicial, dtFinal, postsPagina, pagina, ordenacao, _caminhoImagem,palavra,PostComentario),
                //TotalPosts = RNRedes.RetornaTotalPosts(idEmpresa, dtInicial, dtFinal)
                TotalPosts = RNRedes.RetornaTotalPosts()
            };

            

            return facePost;
        }

        public ZRN.RedesSociais.Facebook.Post GetFacebookPost(int idPost) {

            var RNRedes = new ZRN.RedesSociais.Facebook.Facebook();

            var post = RNRedes.RetornaDadosPost(idPost, _caminhoImagem);

            return post;
        }


        public List<ZRN.RedesSociais.Facebook.Comentario> GetFacebookComentariosPost(int idPost, int inicial, int quantidade) {

            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var comentarios = RN.RetornaComentariosPost(idPost, inicial, quantidade);

            return comentarios;

        }

        public List<ZRN.RedesSociais.Facebook.Resposta> GetFacebookRespostasComentario(string idfacebookcomentario, int inicial, int quantidade)
        {

            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var respostas = RN.RetornaRespostasComentario(idfacebookcomentario, inicial, quantidade);

            return respostas;

        }

        public ZRN.Graficos.TabelaCalor.TabelaCalor GetFacebookEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal) {

            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var tab = RN.RetornaGraficoEngajamentoPorHora(idEmpresa, dtInicial, dtFinal);

            return tab;

        }

        public List<ZRN.Graficos.TagCloud.Termo> GetFacebookTermosPost(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var termos = RN.RetornaTermosPosts(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetFacebookTermosComentario(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Facebook.Facebook();

            var termos = RN.RetornaTermosComentarios(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        //TW

        public List<ZRN.Graficos.Linha> GetGraficoTwCrescimentoFas(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Twitter.Twitter();

            var dadosGraf = RN.RetornaGraficoCrescimentoSeguidores(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public List<ZRN.RedesSociais.Twitter.Interacoes> GetTwInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNTw = new ZRN.RedesSociais.Twitter.Twitter();

            var dadosGraficoTw = RNTw.RetornaGraficoQuantidadeDeInteracoesSemana(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraficoTw;

        }

        public ZRN.Graficos.TabelaCalor.TabelaCalor GetTwEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var RN = new ZRN.RedesSociais.Twitter.Twitter();

            var tab = RN.RetornaGraficoEngajamentoPorHora(idEmpresa, dtInicial, dtFinal);

            return tab;

        }
        
        public EmpresaTwitterPosts GetTwPostsMaisEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina)
        {

            var RN = new ZRN.RedesSociais.Twitter.Twitter();

            var twPosts = new EmpresaTwitterPosts()
            {
                Posts = RN.RetornaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, postsPagina, pagina),
                TotalPosts = RN.RetornaTotalDePosts(idEmpresa, dtInicial, dtFinal)
            };

            return twPosts;
        }

        public ZRN.RedesSociais.Twitter.Post GetTwPost(int idPost)
        {
            var RNTw = new ZRN.RedesSociais.Twitter.Twitter();

            var post = RNTw.RetornaDadosPostTw(idPost);

            return post;
        }

        public List<ZRN.RedesSociais.Twitter.Comentario> GetMaisRetweets(int idpost, int inicial, int quantidade)
        {
            var RNTw = new ZRN.RedesSociais.Twitter.Twitter();

            var retweets = RNTw.RetornaMaisRetweets(idpost, inicial, quantidade);

            return retweets;
        }


        public EmpresaTwitterPosts GetTwitterPostsEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina, string ordenacao,string palavra,string PostComentario)
        {
            string caminhoImagem = Configuracoes.DiretorioImagens;

            var RNRedes = new ZRN.RedesSociais.Twitter.Twitter();

            var twPost = new EmpresaTwitterPosts()
            {
                Posts = RNRedes.RetornaTwPostsEmpresa(idEmpresa, dtInicial, dtFinal, postsPagina, pagina, ordenacao, caminhoImagem, palavra,PostComentario),
                TotalPosts = RNRedes.RetornaTotalDePosts()
            };



            return twPost;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetTwitterTermosPost(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Twitter.Twitter();

            var termos = RN.RetornaTermosPosts(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetTwitterTermosMencoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Twitter.Twitter();

            var termos = RN.RetornaTermosMencoes(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        //sessao INSTAGRAM//
        public List<ZRN.Graficos.Linha> GetGraficoInstaCrescimentoSeguidores(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Instagram.Instagram();

            var dadosGraf = RN.RetornaGraficoInstaCrescimentoSeguidores(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public List<ZRN.RedesSociais.Instagram.Interacoes> GetInstagramInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Instagram.Instagram();

            var dadosGraf = RN.RetornaGraficoQuantidadeInteracoes(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public EmpresaInstaPosts GetInstagramPostsMaisEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina)
        {
            var RN = new ZRN.RedesSociais.Instagram.Instagram();

            var instaPost = new EmpresaInstaPosts()
            {
                Posts = RN.RetornaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, postsPagina, pagina, _caminhoImagem),
                TotalPosts = RN.RetornaTotalDePosts(idEmpresa, dtInicial, dtFinal)
            };

            return instaPost;
        }

        public ZRN.Graficos.TabelaCalor.TabelaCalor GetInstaEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var RN = new ZRN.RedesSociais.Instagram.Instagram();

            var tab = RN.RetornaGraficoEngajamentoPorHora(idEmpresa, dtInicial, dtFinal);

            return tab;

        }

        public ZRN.RedesSociais.Instagram.Post GetPostInsta(int idPost) {
            var RNInsta = new ZRN.RedesSociais.Instagram.Instagram();

            var post = RNInsta.RetornaPostIntagram(idPost, _caminhoImagem);

            return post;
        }

        public List<ZRN.RedesSociais.Instagram.Comentario> GetMaisComentariosDeUmPostInsta(int idPost, int inicial, int quantidade) {
            var RNInsta = new ZRN.RedesSociais.Instagram.Instagram();

            return RNInsta.RetornaMaisComentariosDeUmPost(idPost, inicial, quantidade);
        }
                
        public EmpresaInstaPosts GetTimelineInsta(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina, string ordenacao, string palavra, string PostComentario)
        {
            var RNinsta = new ZRN.RedesSociais.Instagram.Instagram();

            EmpresaInstaPosts instaPosts = new EmpresaInstaPosts() {
                TotalPosts = RNinsta.RetornaTotalDePosts(),
                Posts = RNinsta.RetornaPostsTimelineInstagram(idEmpresa, dtInicial, dtFinal, postsPagina, pagina, ordenacao, _caminhoImagem,palavra,PostComentario)
            };

            return instaPosts;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetInstagramTermosPosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNInsta = new ZRN.RedesSociais.Instagram.Instagram();

            var termos = RNInsta.RetornaTermosPostInstagram(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetInstagramTermosComentarios(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNInsta = new ZRN.RedesSociais.Instagram.Instagram();

            var termos = RNInsta.RetornaTermosComentarios(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        //sessao YOUTUBE//

        public List<ZRN.Graficos.Linha> GetGraficoYoutubeCrescimentoInscritos(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Youtube.Youtube();

            var dadosGraf = RN.RetornaGraficoYoutubeCrescimentoSeguidores(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public List<ZRN.RedesSociais.Youtube.Interacoes> GetYoutubeInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RN = new ZRN.RedesSociais.Youtube.Youtube();

            var dadosGraf = RN.RetornaGraficoYoutubeQuantidadeInteracoes(idEmpresa, dtInicial, dtFinal, ZRN.Graficos.Configuracoes.AgruparPor.semana);

            return dadosGraf;
        }

        public EmpresaYoutubeVideos GetYoutubeVideosMaisEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            var youtubeVideos = new EmpresaYoutubeVideos()
            {
                Videos = RNYoutube.RetornaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, postsPagina, pagina),
                TotalDeVideos = RNYoutube.RetornaTotalDePosts(idEmpresa, dtInicial, dtFinal)
            };
            return youtubeVideos;
        }

        public ZRN.Graficos.TabelaCalor.TabelaCalor GetYoutubeEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            var tabela = RNYoutube.RetornaGraficoYoutubeEngajamentoPorHora(idEmpresa, dtInicial, dtFinal);

            return tabela;
        }

        public ZRN.RedesSociais.Youtube.Video GetVideoYoutube(int id)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            return RNYoutube.RetornaDadosVideoYoutube(id);
        }

        public List<ZRN.RedesSociais.Youtube.Comentario> GetRetornaComentariosDeUmVideo(int id, int inicial, int quantidade)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            return RNYoutube.RetornaComentariosDeUmVideo(id, inicial, quantidade);
        }

        public List<ZRN.RedesSociais.Youtube.Resposta> GetRetornaRespostasComentarioVideo(int idComentario, int inicial, int quantidade)
        {
            var ZRNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            return ZRNYoutube.RetornaRespostasComentario(idComentario, inicial, quantidade);
        }

        public EmpresaYoutubeVideos GetTimelineYoutube(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int postsPagina, int pagina, string ordenacao)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            var YoutubeTimeline = new EmpresaYoutubeVideos()
            {
                TotalDeVideos = RNYoutube.RetornaTotalDePosts(idEmpresa, dtInicial, dtFinal),
                Videos = RNYoutube.RetornaVideosYoutubeEmpresa(idEmpresa,dtInicial,dtFinal, postsPagina, pagina,ordenacao)
            };

            return YoutubeTimeline;

        }

        public List<ZRN.Graficos.TagCloud.Termo> GetYoutubeTermosVideos(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            var termos = RNYoutube.RetornaTermosVideosYoutube(idEmpresa, dtInicial, dtFinal);

            return termos;
        }

        public List<ZRN.Graficos.TagCloud.Termo> GetYoutubeTermosComentarios(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var RNYoutube = new ZRN.RedesSociais.Youtube.Youtube();

            var termos = RNYoutube.RetornaTermosComentariosYoutube(idEmpresa, dtInicial, dtFinal);

            return termos;
        }
        
    }
}
     


