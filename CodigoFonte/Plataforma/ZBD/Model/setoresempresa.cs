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
    
    public partial class setoresempresa
    {
        public setoresempresa()
        {
            this.empresas = new HashSet<empresas>();
        }
    
        public int idsetoresempresa { get; set; }
        public string nome { get; set; }
        public bool excluido { get; set; }
    
        public virtual ICollection<empresas> empresas { get; set; }
    }
}
