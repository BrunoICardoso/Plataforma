using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Graficos.TabelaCalor
{
    public class ItemTabelaCalor
    {
        public long Valor { get; set; }
        public int Hora { get; set; }
        public int DiaSemana { get; set; }
        public long compartilhamentos { get; set; }
        public long reacoes { get; set; }
        public long comentarios { get; set; }
        public long retweets { get; set; }
        public long favoritados { get; set; }
    }
}
