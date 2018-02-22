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
    
    public partial class promo_promocoes
    {
        public promo_promocoes()
        {
            this.promo_arquivos = new HashSet<promo_arquivos>();
            this.promo_estadosabrangencia = new HashSet<promo_estadosabrangencia>();
            this.promo_fb_posts = new HashSet<promo_fb_posts>();
            this.promo_insta_posts = new HashSet<promo_insta_posts>();
            this.promo_municipiosabrangencia = new HashSet<promo_municipiosabrangencia>();
            this.promo_promoempresas = new HashSet<promo_promoempresas>();
            this.promo_promonoticias = new HashSet<promo_promonoticias>();
            this.promo_tw_posts = new HashSet<promo_tw_posts>();
            this.promo_yt_videos = new HashSet<promo_yt_videos>();
            this.promo_regulamentoarquivos = new HashSet<promo_regulamentoarquivos>();
        }
    
        public int idpromocao { get; set; }
        public Nullable<int> idorgaoregulador { get; set; }
        public Nullable<int> idmodalidade { get; set; }
        public string nome { get; set; }
        public string certificadoautorizacao { get; set; }
        public string outrosinteressados { get; set; }
        public Nullable<bool> abrangencianacional { get; set; }
        public Nullable<System.DateTime> dtcadastro { get; set; }
        public Nullable<System.DateTime> dtvigenciaini { get; set; }
        public Nullable<System.DateTime> dtvigenciafim { get; set; }
        public Nullable<decimal> valorpremios { get; set; }
        public string linksitepromocao { get; set; }
        public string linkfacebook { get; set; }
        public string linkinstagram { get; set; }
        public string linktwitter { get; set; }
        public string linkyoutube { get; set; }
        public string mecanicapromo { get; set; }
        public string produtosparticipantes { get; set; }
        public string premiospromo { get; set; }
        public string linkregulamento { get; set; }
        public string textoregulamento { get; set; }
        public bool excluido { get; set; }
    
        public virtual ICollection<promo_arquivos> promo_arquivos { get; set; }
        public virtual ICollection<promo_estadosabrangencia> promo_estadosabrangencia { get; set; }
        public virtual ICollection<promo_fb_posts> promo_fb_posts { get; set; }
        public virtual ICollection<promo_insta_posts> promo_insta_posts { get; set; }
        public virtual promo_modalidades promo_modalidades { get; set; }
        public virtual ICollection<promo_municipiosabrangencia> promo_municipiosabrangencia { get; set; }
        public virtual promo_orgaosreguladores promo_orgaosreguladores { get; set; }
        public virtual ICollection<promo_promoempresas> promo_promoempresas { get; set; }
        public virtual ICollection<promo_promonoticias> promo_promonoticias { get; set; }
        public virtual ICollection<promo_tw_posts> promo_tw_posts { get; set; }
        public virtual ICollection<promo_yt_videos> promo_yt_videos { get; set; }
        public virtual ICollection<promo_regulamentoarquivos> promo_regulamentoarquivos { get; set; }
    }
}
