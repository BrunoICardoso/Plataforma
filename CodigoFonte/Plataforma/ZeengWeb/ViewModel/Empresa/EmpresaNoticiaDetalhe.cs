using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Empresa
{
    public class EmpresaNoticiaDetalhe
    {
        public ZRN.NoticiasElastic.NoticiaElastic noticia { get; set; }        
        public ZRN.Empresas.Empresa empresa { get; set; }
    }
}