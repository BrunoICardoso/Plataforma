using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Youtube
{
    public class Video
    {
        private DateTime DataVideo;

        public long idVideo { get; set; }
        public DateTime dataVideo {

            get
            {

                return DataVideo;
            }

            set
            {

                DataVideo = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time");
            }
        }

        public long qtdVisualizacoes { get; set; }
        public long qtdCurtidas { get; set; }
        public long qtdDescurtidas { get; set; }
        public long qtdFavoritados { get; set; }
        public long qtdComentarios { get; set; }
        //qtdComentariosReal -> serve apenas p usar no botao ver mais em detalhes do post ou linha do tempo.
        public int qtdComentariosReal { get; set; }
        public string postagem { get; set; }
        public string usuariopost { get; set; }
        public string urlImagem { get; set; }
        public string nomeImagem { get; set; }

        public IEnumerable<Comentario> comentarios { get; set; }


    }
}
