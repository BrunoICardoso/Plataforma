using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Twitter
{
    public class Comentario
    {
        private DateTime? DataPost;

        public string imagem { get; set; }
        public string usuarioPost { get; set; }
        public long? idPost { get; set; }
        public long? idmencao { get; set; }
        public int? idPerfil { get; set; }
        public string idTwitterPost { get; set; }
        public DateTime? dataPost {

            get
            {
                return DataPost;
            }

            set
            {
                DataPost = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time");
            }
        }
        public long? totalFavoritadas { get; set; }
        public long? totalRetweets { get; set; }
        public long? totalMencoes { get; set; }
        public long? totalReplies { get; set; }
        public string postagem { get; set; }
     
        public IEnumerable<Post> postsReply;



    }



}
