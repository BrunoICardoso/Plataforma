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
    
    public partial class empresas
    {
        public empresas()
        {
            this.cnpjempresa = new HashSet<cnpjempresa>();
            this.empresaredessociais = new HashSet<empresaredessociais>();
            this.marcas = new HashSet<marcas>();
            this.tw_perfis = new HashSet<tw_perfis>();
            this.insta_perfis = new HashSet<insta_perfis>();
            this.fb_perfis = new HashSet<fb_perfis>();
            this.noticias_empresa = new HashSet<noticias_empresa>();
            this.yt_canais = new HashSet<yt_canais>();
            this.presenca_online_capturas = new HashSet<presenca_online_capturas>();
            this.seae_empresa_processos = new HashSet<seae_empresa_processos>();
            this.mapa_registro_empresa = new HashSet<mapa_registro_empresa>();
            this.promo_promoempresas = new HashSet<promo_promoempresas>();
            this.cliente_empresas = new HashSet<cliente_empresas>();
            this.cliente_relatorio_empresas = new HashSet<cliente_relatorio_empresas>();
        }
    
        public int idempresa { get; set; }
        public int idsetor { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string urlsite { get; set; }
        public bool excluido { get; set; }
        public string imagem { get; set; }
    
        public virtual ICollection<cnpjempresa> cnpjempresa { get; set; }
        public virtual ICollection<empresaredessociais> empresaredessociais { get; set; }
        public virtual setoresempresa setoresempresa { get; set; }
        public virtual ICollection<marcas> marcas { get; set; }
        public virtual ICollection<tw_perfis> tw_perfis { get; set; }
        public virtual ICollection<insta_perfis> insta_perfis { get; set; }
        public virtual ICollection<fb_perfis> fb_perfis { get; set; }
        public virtual ICollection<noticias_empresa> noticias_empresa { get; set; }
        public virtual ICollection<yt_canais> yt_canais { get; set; }
        public virtual ICollection<presenca_online_capturas> presenca_online_capturas { get; set; }
        public virtual ICollection<seae_empresa_processos> seae_empresa_processos { get; set; }
        public virtual ICollection<mapa_registro_empresa> mapa_registro_empresa { get; set; }
        public virtual ICollection<promo_promoempresas> promo_promoempresas { get; set; }
        public virtual ICollection<cliente_empresas> cliente_empresas { get; set; }
        public virtual ICollection<cliente_relatorio_empresas> cliente_relatorio_empresas { get; set; }
    }
}
