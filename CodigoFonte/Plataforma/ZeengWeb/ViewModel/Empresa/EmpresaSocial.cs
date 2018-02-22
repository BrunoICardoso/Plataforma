using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaSocial
    {
        public ZRN.Empresas.Empresa Empresa { get; set; }

        public ZRN.RedesSociais.Seguidores TotalSeguidoresFace{ get; set; }
        public ZRN.RedesSociais.Seguidores TotalSeguidoresTw { get; set; }
        public ZRN.RedesSociais.Seguidores TotalSeguidoresInsta { get; set; }
        public ZRN.RedesSociais.Seguidores TotalSeguidoresYoutube { get; set; }

        public ZRN.RedesSociais.EmpresaRedeSocial temRedesSocias { get; set; }
    }
}