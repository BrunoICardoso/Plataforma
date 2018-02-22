using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZRN.LancamentoProdutos;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaLancamento
    {
        public ZRN.Empresas.Empresa Empresa { get; set; }

        public IEnumerable<LancamentoProduto> Lancamentos { get; set; }
        public int TotalLancamentos { get; set; }

    }
}