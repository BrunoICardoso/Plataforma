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
    
    public partial class fb_perfis
    {
        public fb_perfis()
        {
            this.fb_perfis_stats = new HashSet<fb_perfis_stats>();
            this.fb_posts = new HashSet<fb_posts>();
        }
    
        public long idperfil { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> idmarca { get; set; }
        public string nome { get; set; }
        public string idfacebook { get; set; }
        public string username { get; set; }
        public string sobre { get; set; }
        public string descricao { get; set; }
        public string urlimagem { get; set; }
        public string nomeimagem { get; set; }
        public string website { get; set; }
        public Nullable<System.DateTime> datahoraultimopost { get; set; }
        public string link { get; set; }
        public Nullable<System.DateTime> ultimaatualizacao { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual marcas marcas { get; set; }
        public virtual ICollection<fb_perfis_stats> fb_perfis_stats { get; set; }
        public virtual ICollection<fb_posts> fb_posts { get; set; }
    }
}
