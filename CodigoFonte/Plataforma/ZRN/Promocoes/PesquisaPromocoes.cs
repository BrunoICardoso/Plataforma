using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class PesquisaPromocoes
    {

        public int pagina { get; set; }
        public int qtdRegistro { get; set; }
        public int totalRegistros { get; set; }

        public List<Promocao> Promocoes { get; set; }

    }
}
