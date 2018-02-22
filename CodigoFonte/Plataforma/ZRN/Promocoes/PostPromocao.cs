using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class PostPromocao
    {
        public string redeSocial { get; set; }
        public DateTime? dataPostagem { get; set; }
        public int? curtidas { get; set; }
        public int? comentarios { get; set; }
        public int? compartilhamentos { get; set; }
        public int? visualizacoes { get; set; }
        public string postagem { get; set; }
        public int? retweets { get; set; }
        public string imagem { get; set; }
        public List<tagPromo> promocoes { get; set; }
    }

    public class tagPromo
    {
        public string nome { get; set; }
    }

}
