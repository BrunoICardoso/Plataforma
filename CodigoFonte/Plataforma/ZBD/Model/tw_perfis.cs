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
    
    public partial class tw_perfis
    {
        public tw_perfis()
        {
            this.tw_mencoes = new HashSet<tw_mencoes>();
            this.tw_perfis_stats = new HashSet<tw_perfis_stats>();
            this.tw_posts = new HashSet<tw_posts>();
        }
    
        public int idperfil { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> idmarca { get; set; }
        public string idtwitter { get; set; }
        public string nome { get; set; }
        public string screenname { get; set; }
        public string descricao { get; set; }
        public string ultimotwcapturado { get; set; }
        public string ultimotwmencaocapturado { get; set; }
        public Nullable<System.DateTime> ultimaatualizacao { get; set; }
        public string urlimagem { get; set; }
        public string nomeimagem { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual marcas marcas { get; set; }
        public virtual ICollection<tw_mencoes> tw_mencoes { get; set; }
        public virtual ICollection<tw_perfis_stats> tw_perfis_stats { get; set; }
        public virtual ICollection<tw_posts> tw_posts { get; set; }
    }
}
