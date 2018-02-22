using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Youtube
{
    public class Interacoes
    {
        public DateTime data { get; set; }
        //public string dataFormatada { get; set; }
        public long interacoes { get; set; }
        public long visualizacoes { get; set; }
        public long curtidas { get; set; }
        public long comentarios { get; set; }
        public long videos { get; set; }
    }
}
