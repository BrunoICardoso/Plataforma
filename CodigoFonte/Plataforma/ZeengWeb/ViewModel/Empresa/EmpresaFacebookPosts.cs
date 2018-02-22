using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaFacebookPosts
    {
        public int TotalPosts { get; set; }
        public List<ZRN.RedesSociais.Facebook.Post> Posts { get; set; }

    }
}