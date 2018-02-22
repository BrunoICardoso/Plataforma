using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Facebook
{
    public class Comentario
    {
        private DateTime Datahora; 

        //Conversão do TimeZone
        //Datahora = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time"); 
        
            //https://msdn.microsoft.com/en-us/library/gg154758.aspx?f=255&MSPPError=-2147217396 Site das "TimeZoneNome"

        public long idcomentario { get; set; }
        public string idfacebook { get; set; }
        public long idpost { get; set; }
        public string nomeusuario { get; set; }
        public string postagem { get; set; }
        public string nomeimagem { get; set; }
        public string urlimagem { get; set; }

        public DateTime datahora{
            get {

                return Datahora;
            }

            set {

                Datahora = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()),"E. South America Standard Time");
            }
        }
        public long? totalCurtidas { get; set; }
        public long? totalRespostas { get; set; }
        public long? totalRespostasPost{ get; set;}
        public IEnumerable<Resposta> respostas;
        

    }

    public class Resposta
    {

        private DateTime Datahora;

        public long idcomentario { get; set; }
        public string idfacebook { get; set; }
        public string idrespostafacebook { get; set; }
        public long idpost { get; set; }
        public string nomeusuario { get; set; }
        public string postagem { get; set; }
        public string nomeimagem { get; set; }
        public DateTime datahora
        {
            get{
                 return Datahora;
            }

            set{
                 Datahora = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time");
            }
        }

        public long? totalCurtidas { get; set; }


    }
}
