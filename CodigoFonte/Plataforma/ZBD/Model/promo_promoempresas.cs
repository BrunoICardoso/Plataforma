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
    
    public partial class promo_promoempresas
    {
        public int idpromoempresas { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> idpromocao { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual promo_promocoes promo_promocoes { get; set; }
    }
}
