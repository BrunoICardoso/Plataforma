using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaYoutubeVideos
    {
        public int TotalDeVideos { get; set; }
        public List<ZRN.RedesSociais.Youtube.Video> Videos { get; set; }
    }
}