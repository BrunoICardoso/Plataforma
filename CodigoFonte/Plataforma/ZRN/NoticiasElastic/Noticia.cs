using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.NoticiasElastic
{

    [ElasticsearchType(Name="noticias")]
    public class NoticiaElastic
    {
        public int idnoticia { get; set; }
        public int idnoticiaknewin { get; set; }
        public int idfonte { get; set; }

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

        public List<imagensnoticias> imagens { get; set; }      
        public List<noticiaempresa> empresas { get; set; }
        public List<noticiaPromocao> promocoes { get; set; }

    }

    public class imagensnoticias {
        public string titulo { get; set; }
        public string url { get; set; }
        public string creditos { get; set; }
    }

    public class noticiaempresa
    {
        public int idnoticiaempresa { get; set; }
        public int idempresa { get; set; }
        public string nomeempresa { get; set; }
        public string descricaoempresa { get; set; }
    }

    public class noticiaPromocao {
        public int idPromocao { get; set; }
        public string nome { get; set; }

    }
}
