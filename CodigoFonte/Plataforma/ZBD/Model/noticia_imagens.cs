//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZBD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class noticia_imagens
    {
        public int idimagem { get; set; }
        public int idnoticia { get; set; }
        public string url { get; set; }
        public string titulo { get; set; }
        public string creditos { get; set; }
    
        public virtual noticias noticias { get; set; }
    }
}
