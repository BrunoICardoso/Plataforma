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
    
    public partial class mapa_origens
    {
        public mapa_origens()
        {
            this.mapa_registros = new HashSet<mapa_registros>();
        }
    
        public int idorigem { get; set; }
        public string nome { get; set; }
        public Nullable<bool> excluido { get; set; }
    
        public virtual ICollection<mapa_registros> mapa_registros { get; set; }
    }
}
