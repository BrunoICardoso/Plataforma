using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais
{
    public class Seguidores
    {
        /// <summary>
        /// Retorna ou define o total de seguidores
        /// </summary>
        public long? totalAtual { get; set; }
        public long? totalAnterior { get; set; }

        /// <summary>
        /// Variação percentural na quantidade total de seguidores no período especificado
        /// </summary>
        public double variacao
        {
            get
            {
                return ((Convert.ToDouble(totalAtual) / Convert.ToDouble(totalAnterior)) - 1) * 100;
            }
        }

        public DateTime? dataVarAtual { get; set; }
        public DateTime? dataVarAnterior { get; set; }

        public double TotalDias
        {
            get
            {
                return (dataVarAtual - dataVarAnterior).Value.TotalDays;
            }
        }

    }
}
