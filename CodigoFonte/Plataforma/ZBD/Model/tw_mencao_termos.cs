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
    
    public partial class tw_mencao_termos
    {
        public int idmencaotermo { get; set; }
        public Nullable<int> idmencao { get; set; }
        public string termo { get; set; }
        public int frequencia { get; set; }
    
        public virtual tw_mencoes tw_mencoes { get; set; }
    }
}
