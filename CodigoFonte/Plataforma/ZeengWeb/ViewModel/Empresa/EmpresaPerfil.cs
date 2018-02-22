using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaPerfil
    {
        public ZRN.Empresas.Empresa Empresa{ get; set; }
        public ZRN.Clientes.ClienteVertentes VertentesCliente { get; set; }        
    }
}