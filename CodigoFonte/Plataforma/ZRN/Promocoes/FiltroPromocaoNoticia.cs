using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class FiltroPromocaoNoticia
    {
        public int idPromocao { get; set; }
        public int idEmpresa { get; set; }
        public int qtdeRegistros { get; set; }
        public int pagina { get; set; }
        public string expressao { get; set; }
    }
}
