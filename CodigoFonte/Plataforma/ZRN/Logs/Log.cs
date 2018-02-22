using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Logs
{
    public class Log
    {
        public int idlogs { get; set; }
        public int idusuario { get; set; }
        public DateTime datahora { get; set; }
        public string url { get; set; }
        public string controle { get; set; }
        public string acao { get; set; }
        public string descricao { get; set; }
        public string nivel { get; set; }
    }
}
