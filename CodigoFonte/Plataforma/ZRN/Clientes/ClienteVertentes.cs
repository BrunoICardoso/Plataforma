using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Clientes
{
   public class ClienteVertentes
    {

        public int idclientevertente { get; set; }
        public int idcliente { get; set; }
        public bool redessociais { get; set; }
        public bool presencaonline { get; set; }
        public bool noticias { get; set; }
        public bool produtos { get; set; }
        public bool promocoes { get; set; }
    }
}
