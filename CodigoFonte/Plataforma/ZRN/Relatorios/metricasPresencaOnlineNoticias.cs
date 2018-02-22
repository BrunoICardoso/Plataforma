using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class metricasPresencaOnlineNoticias
    {
        public int idEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public int? rankbrasilAtual { get; set; }
        public int? rankbrasilAnterior { get; set; }
        public int? rankglobalAtual { get; set; }
        public int? rankglobalAnterior { get; set; }
        public int? crescimentoRankbrasil { get; set; }
        public int? crescimentoRankglobal { get; set; }
        public int? visitasPesquisa { get; set; }
        public int? taxaRejeicao { get; set; }
        public int? paginasVisitadas { get; set; }
        public TimeSpan tempoVisitas { get; set; }
        public int? totalNoticias { get; set; }
    }
}
