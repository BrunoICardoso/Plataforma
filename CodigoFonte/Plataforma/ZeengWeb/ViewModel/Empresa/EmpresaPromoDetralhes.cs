using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaPromoDetralhes
    {
        public ZRN.Promocoes.Promocao promocao { get; set; }
        public int idEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string empresas { get; set; } // string com empresas empresa a, empresa b, empresa c
    }
}