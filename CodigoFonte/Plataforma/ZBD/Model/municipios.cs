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
    
    public partial class municipios
    {
        public municipios()
        {
            this.seae_abrang_municipio = new HashSet<seae_abrang_municipio>();
            this.promo_municipiosabrangencia = new HashSet<promo_municipiosabrangencia>();
        }
    
        public int idmunicipio { get; set; }
        public Nullable<int> idestado { get; set; }
        public string nome { get; set; }
        public string codibge { get; set; }
        public Nullable<bool> atualizado { get; set; }
    
        public virtual ICollection<seae_abrang_municipio> seae_abrang_municipio { get; set; }
        public virtual ICollection<promo_municipiosabrangencia> promo_municipiosabrangencia { get; set; }
        public virtual estados estados { get; set; }
    }
}