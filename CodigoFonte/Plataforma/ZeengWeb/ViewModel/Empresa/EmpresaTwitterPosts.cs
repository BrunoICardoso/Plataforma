using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaTwitterPosts
    {
        public int TotalPosts { get; set; }
        public List<ZRN.RedesSociais.Twitter.Post> Posts { get; set; }
    }
}