using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.PromocoesElastic
{
    public class Noticia
    {

        public string conteudo { get; set; }
        public string autor { get; set; }
        public string titulo { get; set; }
        public string nomefonte { get; set; }
        public string link { get; set; }
        public string url { get; set; }
        public int idnoticia { get; set; }
        public List<Promocao> promocoes { get; set; }
        //public DateTime datapublicacao { get; set; }
        public string datapublicacao { get; set; }
        public List<ZRN.Empresas.EmpresaNoticia> empresas { get; set; }
        public int idnoticiaempresa { get; set; }

    }

    public class Promocao
    {
        public int idpromocao { get; set; }
        public string nomepromocao { get; set; }
    }

    

}
