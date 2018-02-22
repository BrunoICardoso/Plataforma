using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class FiltroProcessoSeae
    {
        public int pag { get; set; }
        public int quantidade { get; set; }
        public List<int> situacoes { get; set; }

        public int idEmpresa { get; set; }

        public string numproc { get; set; }
        //public int estados { get; set; }

        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }

        public bool ordenacao { get; set; }

        public string pesquisa { get; set; }
    }

    //public class FiltroTeste
    //{
    //    public int pag { get; set; }
    //    public DateTime dataInicial { get; set; }
    //    public DateTime dataFinal { get; set; }
    //}
}
