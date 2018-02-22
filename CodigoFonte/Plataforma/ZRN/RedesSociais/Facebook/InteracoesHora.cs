using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais.Facebook
{
    public class InteracoesHora
    {

        public int diaSemana { get; set; }
        public int hora { get; set; }
        public long interacoes { get; set; }
        public long compartilhamentos { get; set; }
        public long reacoes { get; set; }
        public long comentarios { get; set; }
    }
}
