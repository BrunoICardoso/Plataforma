using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Facebook
{
    public class Interacoes
    {
        public DateTime data { get; set; }
        public long interacoes { get; set; }
        public long compartilhamentos { get; set; }
        public long reacoes { get; set; }
        public long comentarios { get; set; }
        public long posts { get; set; }
    }
}
