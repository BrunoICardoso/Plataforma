using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaInstaPosts
    {
        public int TotalPosts { get; set; }
        public List<ZRN.RedesSociais.Instagram.Post> Posts { get; set; }
    }
}