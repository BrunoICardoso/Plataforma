using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class FiltroRelatorio
    {

        public int idCliente { get; set; }
        public int idUsuario { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }        
        public string pesquisa { get; set; }
        public List<int> empresas { get; set; }
        public int qtdeRegistros { get; set; }
        public int pagina { get; set; }
    }
}
