using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Twitter
{
    public class Interacoes
    {
        public DateTime data { get; set; }
        //public string dataFormatada { get; set; }
        public long interacoes { get; set; }
        public long retweets { get; set; }
        public long favoritados { get; set; }
        public long posts { get; set; }
        
        //public long totalLikes { get; set; }     
    }
}
