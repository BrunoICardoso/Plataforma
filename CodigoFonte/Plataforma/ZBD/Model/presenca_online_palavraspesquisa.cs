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
    
    public partial class presenca_online_palavraspesquisa
    {
        public int idpalavrapesquisa { get; set; }
        public int idcaptura { get; set; }
        public string palavra { get; set; }
        public Nullable<decimal> percentual { get; set; }
        public bool excluido { get; set; }
    
        public virtual presenca_online_capturas presenca_online_capturas { get; set; }
    }
}