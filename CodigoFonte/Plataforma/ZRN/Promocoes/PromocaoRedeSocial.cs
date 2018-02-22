using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class PromocaoRedeSocial
    {
        public List<fb_post> PostsFacebook { get; set; }
        public List<tw_post> PostsTwitter { get; set; }
    }

    public class fb_post
    {
        public int idpost { get; set; }
        public int idperfil { get; set; }
        public int idempresa { get; set; }
        public int compartilhamentos { get; set; }
        public int reacoes { get; set; }        
        public int qtdcomentarios { get; set; }
        public int totalposts { get; set; }
        public string datahora { get; set; }        
        public string postagem { get; set; }
        public string nomeimagem { get; set; }
        public List<ZRN.PromocoesElastic.PromocaoElastic> promocoes { get; set; }
        public List<ZRN.Promocoes.comentariosFacebook> comentarios { get; set; }
    }

    public class comentariosFacebook
    {
        //public DateTime datahora { get; set; }
        public string datahora { get; set; }
        public int curtidas { get; set; }
        public string nomeusuario { get; set; }        
        public string postagem { get; set; }
        public string urlimagem { get; set; }
        public List<respostasComentarioFacebook> respostas { get; set; }
    }

    public class respostasComentarioFacebook
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        //DateTime? datahora { get; set; }
        public string nomeusuario { get; set; }
        public string postagem { get; set; }
        public string urlimagem { get; set; }
    }

    public class tw_post
    {
        public int idpost { get; set; }
        public int idperfil { get; set; }
        public int idempresa { get; set; }        
        public int retweets { get; set; }
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public string nomeimagem { get; set; }
        public List<ZRN.PromocoesElastic.PromocaoElastic> promocoes { get; set; }        
    }

    public class insta_post
    {
        public int idpost { get; set; }
        public int idperfil { get; set; }
        public int idempresa { get; set; }        
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public string nomeimagem { get; set; }
        public List<ZRN.PromocoesElastic.PromocaoElastic> promocoes { get; set; }
        public List<ZRN.Promocoes.comentariosInstagram> comentarios { get; set; }
    }

    public class comentariosInstagram
    {
        public string datahora { get; set; }        
        public string postagem { get; set; }        
        public string nomeusuario { get; set; }     
    }


    public class yt_video
    {
        public int idpost { get; set; }
        public int idperfil { get; set; }
        public int idempresa { get; set; }
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string descricao { get; set; }
        public string nomeimagem { get; set; }
        public List<ZRN.PromocoesElastic.PromocaoElastic> promocoes { get; set; }
        public List<ZRN.Promocoes.comentariosInstagram> comentarios { get; set; }
    }

    public class comentariosYoutube
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public string nomeusuario { get; set; }
        public string urlimagem { get; set; }
        public List<respostasComentarioYoutube> respostas { get; set; }
    }

    public class respostasComentarioYoutube
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string nomeusuario { get; set; }
        public string postagem { get; set; }        
    }
}
