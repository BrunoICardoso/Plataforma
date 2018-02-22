using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaTweetDetalhe
    {
        public int totalRetweets { get; set; }
        public ZRN.RedesSociais.Twitter.Post Tweet { get; set; }
    }
}