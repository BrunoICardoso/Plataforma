using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;

namespace ZRN.Empresas
{
    public class NoticiasFonte
    {
        public int idFonte { get; set; }
        public string nomeFonte { get; set; }
        public int total { get; set; }
        public int totalPeriodoAnterior { get; set; }
        public List<Linha> totalNoticias { get; set; }
    


    }
}
