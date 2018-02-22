using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Youtube
{
    public class Comentario
    {
        private DateTime DataComentario;


        public int idcomentario { get; set; }
        public int idvideo { get; set; }
        public string postagem { get; set; }
        public string nomeUsuario { get; set; }
        public int totalCurtidas { get; set; }
        public DateTime dataComentario {

            get
            {

                return DataComentario;
            }

            set
            {

                DataComentario = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time");
            }
        }
        public int? totalRespostas { get; set; }
        public IEnumerable<Resposta> respostas { get; set; }
    }

    public class Resposta
    {
        public int idcomentario { get; set; }
        public int? idcomentarioresposta { get; set; }
        public int idvideo { get; set; }
        public string postagem { get; set; }
        public string nomeUsuario { get; set; }
        public int totalCurtidas { get; set; }
        public DateTime dataComentario { get; set; }
    }



}
