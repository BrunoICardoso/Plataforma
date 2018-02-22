using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Noticias
{
    public class Noticia
    {

        public int idNoticia { get; set; }
        public int idFonte { get; set; }
        public int idNoticia_Knewin { get; set; }

        public int idnoticiaempresa { get; set; }

        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string dominio { get; set; }
        public string autor { get; set; }
        public string conteudo { get; set; }
        public string url { get; set; }
        public DateTime dataPublicacao { get; set; }
        public DateTime dataCapturaZeeng { get; set; }
        public string categoria { get; set; }
        public string localidade { get; set; }
        public string linguagem { get; set; }
        public bool excluido { get; set; }
        public List<NoticiaImagem> imagens { get; set; }
    }
}
