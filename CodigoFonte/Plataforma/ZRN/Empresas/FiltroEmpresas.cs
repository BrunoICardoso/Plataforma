using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class FiltroEmpresas
    {
        public string pesquisa { get; set; }
        public int pagina { get; set; }
        public int qtdregistros { get; set; }
        public int regInicial { get; set; }
        public int idcliente { get; set; }
    }
}
