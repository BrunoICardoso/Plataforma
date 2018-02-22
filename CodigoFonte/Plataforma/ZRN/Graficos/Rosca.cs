using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Graficos
{
    public class Rosca
    {
        public List<ItemRosca> Valores { get; set; }

        public float ValorTotal
        {
            get
            {
                return Valores.Sum(x => x.valor);
            }
        }
    }
}
