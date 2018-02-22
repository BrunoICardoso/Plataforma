using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaListar
    {
        public IEnumerable<ZRN.Empresas.Empresa> Empresas { get; set; }
        public int TotalEmpresas { get; set; }
    }
}