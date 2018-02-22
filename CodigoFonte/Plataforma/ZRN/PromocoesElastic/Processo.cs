using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.PromocoesElastic
{
    public class Processo
    {        
        public string situacaoatual { get; set; }
        public DateTime dtprocesso { get; set; }
        public int idsituacaoatual { get; set; }
        public string formacontemplacao { get; set; }
        public string premios { get; set; }
        public string interessados { get; set; }
        public string modalidade { get; set; }
        public string nome { get; set; }

    }
}
