using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Clientes
{
    public class Filtros
    {
        public int idSetor { get; set; }
        public int idCliente { get; set; }
        public int pagina { get; set; }
        public int qtdregistros { get; set; }
        public string expressao { get; set; }
    }
}
