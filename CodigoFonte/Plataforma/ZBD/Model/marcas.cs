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
    
    public partial class marcas
    {
        public marcas()
        {
            this.cnpjmarca = new HashSet<cnpjmarca>();
            this.marcaredessociais = new HashSet<marcaredessociais>();
            this.produtos = new HashSet<produtos>();
            this.tw_perfis = new HashSet<tw_perfis>();
            this.insta_perfis = new HashSet<insta_perfis>();
            this.fb_perfis = new HashSet<fb_perfis>();
            this.noticias_marca = new HashSet<noticias_marca>();
            this.yt_canais = new HashSet<yt_canais>();
        }
    
        public int idmarca { get; set; }
        public Nullable<int> idempresa { get; set; }
        public string nome { get; set; }
        public string urlsite { get; set; }
        public string descricao { get; set; }
        public Nullable<bool> excluido { get; set; }
        public string imagem { get; set; }
    
        public virtual ICollection<cnpjmarca> cnpjmarca { get; set; }
        public virtual empresas empresas { get; set; }
        public virtual ICollection<marcaredessociais> marcaredessociais { get; set; }
        public virtual ICollection<produtos> produtos { get; set; }
        public virtual ICollection<tw_perfis> tw_perfis { get; set; }
        public virtual ICollection<insta_perfis> insta_perfis { get; set; }
        public virtual ICollection<fb_perfis> fb_perfis { get; set; }
        public virtual ICollection<noticias_marca> noticias_marca { get; set; }
        public virtual ICollection<yt_canais> yt_canais { get; set; }
    }
}
