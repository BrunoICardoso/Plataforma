using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class FiltroNoticiasEmpresa
    {
        public int idEmpresa { get; set; }
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public string expressao { get; set; }
        public string titulo { get; set; }
        public string conteudo { get; set; }
        public string subtitulo { get; set; }
        public List<int> fontes { get; set; }
        public int inicial { get; set; }
        public int qtdRegistros { get; set; }

    }
}
