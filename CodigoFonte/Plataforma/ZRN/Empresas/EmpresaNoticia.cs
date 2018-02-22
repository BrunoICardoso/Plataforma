using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Noticias;

namespace ZRN.Empresas
{
    public class EmpresaNoticia
    {

        public int idNoticia { get; set; }
        public int? idEmpresa { get; set; }
        public int idNoticiaEmpresa { get; set; }
            
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string nomeFonte { get; set; }
        public DateTime datapublicacao { get; set; }
        public string conteudo { get; set; }
        public string autor { get; set; }             
        public string url { get; set; }
        public string nomeEmpresa { get; set; }

        public IEnumerable<Empresa> Empresas { get; set; }
        public IEnumerable<NoticiaImagem> Imagens { get; set; }

    }


    

}

