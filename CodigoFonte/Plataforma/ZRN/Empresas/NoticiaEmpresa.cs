using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class NoticiaEmpresa
    {

        public int idnoticia { get; set; }
        public int idnoticiaknewin { get; set; }
        public int idfonte { get; set; }

        public int idnoticiaempresa { get; set; }
        public int idempresa { get; set; }
        public string nomeEmpresa { get; set; }


        public string nomefonte { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string dominio { get; set; }
        public string autor { get; set; }
        public string conteudo { get; set; }
        public string url { get; set; }
        public string localidade { get; set; }
        public string linguagem { get; set; }

        public string datapublicacao { get; set; }
        public string datacaptura_knewin { get; set; }
        public string datacaptura_zeeng { get; set; }

        public List<ImagemNoticia> imagens { get; set; }
        public List<TagNoticia> tags { get; set; }


    }

    public class ImagemNoticia {
        public string titulo { get; set; }
        public string url { get; set; }
        public string creditos { get; set; }
    }

    public class TagNoticia {
        public int valor { get; set; }
        public string termo { get; set; }
    }
    public class TagNoticiaDetalhe {

       public string nomeempresas { get; set; }
       public int idempresa { get; set; }
    }
}
