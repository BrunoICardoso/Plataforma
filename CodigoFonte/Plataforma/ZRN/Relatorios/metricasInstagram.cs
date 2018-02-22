using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class metricasInstagram
    {
        public int idEmpresa { get; set; }
        public string nome { get; set; }
        public int? qtdSeguidoresAtual { get; set; }
        public int? qtdSeguidoresAnterior { get; set; }
        public int? crescimentoFas { get; set; }
        public int? posts { get; set; }
        public int? curtidas { get; set; }       
        public int? comentarios { get; set; }
        public int? interacoes { get; set; }
        //public float? mediaInteracoesPost { get; set; }
        public float? engajamento { get; set; }
    }
}
