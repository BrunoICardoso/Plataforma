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
    
    public partial class insta_perfis_stats
    {
        public long idperfilstats { get; set; }
        public int idperfil { get; set; }
        public System.DateTime datahora { get; set; }
        public Nullable<long> qtdmidias { get; set; }
        public Nullable<long> qtdseguidores { get; set; }
        public Nullable<long> qtdseguidos { get; set; }
    
        public virtual insta_perfis insta_perfis { get; set; }
    }
}
