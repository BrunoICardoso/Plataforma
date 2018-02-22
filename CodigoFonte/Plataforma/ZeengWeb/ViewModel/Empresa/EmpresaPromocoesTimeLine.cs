using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaPromocoesTimeLine
    {
        public int TotalProcessos { get; set; }
        public IEnumerable<ZRN.Promocoes.Processos_Seae> ListaProcessos { get; set; }
    }
}