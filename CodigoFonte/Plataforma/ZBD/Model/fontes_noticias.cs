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
    
    public partial class fontes_noticias
    {
        public fontes_noticias()
        {
            this.noticias = new HashSet<noticias>();
        }
    
        public int idfonte { get; set; }
        public Nullable<int> idfonte_knewin { get; set; }
        public string nome { get; set; }
        public string url { get; set; }
        public string pais { get; set; }
        public string estado { get; set; }
        public string cidade { get; set; }
        public string tipo { get; set; }
        public bool ativo { get; set; }
        public bool excluido { get; set; }
        public bool manual { get; set; }
    
        public virtual ICollection<noticias> noticias { get; set; }
    }
}
