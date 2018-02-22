using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Instagram
{
    public class Comentario
    {
        private DateTime DataComentario;

        public int idPost { get; set; }
        public int idcomentario { get; set; }
        public string postagem { get; set; }
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
        public string nomeUsuario { get; set; }
        public int totalcomentario { get; set; }
    }
}
