using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Cliente
{
    public class ClienteConfiguracao
    {
        public ZRN.Clientes.Cliente cliente { get; set; }
        public int totalEmpresas { get; set; }
    }
}