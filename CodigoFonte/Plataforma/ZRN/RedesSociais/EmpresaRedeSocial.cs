using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais
{
    public class EmpresaRedeSocial
    {
        public DadosFacebook dadosFacebook { get; set; }
        public DadosTwitter dadosTwitter { get; set; }
        public DadosInstagram dadosInstagram { get; set; }
        public DadosYoutube dadosYoutube { get; set; }
    }


    public class DadosFacebook
    {
        public string Nome = "Facebook";
        public string Icone = "fa-facebook";
        public int Id { get; set; }
        public string Url { get; set; }            
    }
    
    public class DadosTwitter
    {
        public string Nome = "Twitter";
        public string Icone = "fa-twitter";
        public int Id { get; set; }
        public string Url { get; set; }

    }

    public class DadosInstagram
    {
        public string Nome = "Instagram";
        public string Icone = "fa-instagram";
        public int Id { get; set; }
        public string Url { get; set; }
    }

    public class DadosYoutube
    {
        public string Nome = "Youtube";
        public string Icone = "fa-youtube-play";
        public int Id { get; set; }
        public string Url { get; set; }
    }

}
