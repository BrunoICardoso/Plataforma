using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaNoticias
    {

        public int idEmpresa { get; set; }
        public ZRN.Empresas.Empresa empresa { get; set; }
        public List<ZRN.Empresas.NoticiaEmpresaFonte> noticiafonteempresa { get; set; }
        
    }
}