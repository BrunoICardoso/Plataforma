using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.PrensencaOnline
{
    public class Indicadores
    {

        public int? rankBrasil { get; set; }
        public int? rankGlobal { get; set; }
        public int? autoridadeDominio { get; set; }
        public int? autoridadePagina { get; set; }
        public Decimal? visitasPesquisa { get; set; }
        public Decimal? taxaRejeicao { get; set; }
        public int? pontuacaoSpam { get; set; }
        public int? links { get; set; }
        public int? novosLinks { get; set; }
        public Decimal? paginasVisitadas { get; set; }
        public TimeSpan? tempoVisita { get; set; }

    }
}
