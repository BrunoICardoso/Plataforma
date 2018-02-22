using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class metricasFacebook
    {
        public int idEmpresa { get; set; }
        public string nome { get; set; }
        public int? qtdLikesAtual { get; set; }
        public int? qtdLikesAnterior { get; set; }
        public int? crescimentoFas { get; set; }
        public int? posts { get; set; }
        public int? compartilhamentos { get; set; }
        public int? reacoes { get; set; }
        public int? comentarios { get; set; }
        public int? interacoes { get; set; }
        public float? mediaInteracoesPost { get; set; }
        public float? engajamento { get; set; }
    }
}
