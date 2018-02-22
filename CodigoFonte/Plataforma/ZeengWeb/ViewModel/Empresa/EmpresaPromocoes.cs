using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaPromocoes
    {
        public ZRN.Empresas.Empresa Empresa { get; set; }
        public IEnumerable<ZRN.Promocoes.Situacao> Situacoes { get; set; }
        
    }
}