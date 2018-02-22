using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Promoções
{
    public class NoticiasPromocoesRecentes
    {
        public DateTime dataPublicacao { get; set; }
        public string titulo { get; set; }
        public string fonte { get; set; }
        public string conteudo { get; set; }
        public string autor { get; set; }
        public int idNoticia { get; set; }
        public List<tagPromo> tags { get; set; }
    }

    public class tagPromo
    {
        public string nome { get; set; }
        public int idTag { get; set; }
    }
    
}