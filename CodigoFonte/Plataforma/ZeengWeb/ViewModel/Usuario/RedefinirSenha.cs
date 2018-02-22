using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Usuario
{
    public class RedefinirSenha
    {
        public string usuario { get; set; }
        public string senhaAntiga { get; set; }
        public string novasenha { get; set; }
        public string confirmasenha { get; set; }
    }
}