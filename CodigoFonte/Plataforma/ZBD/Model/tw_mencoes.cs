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
    
    public partial class tw_mencoes
    {
        public tw_mencoes()
        {
            this.tw_mencao_termos = new HashSet<tw_mencao_termos>();
        }
    
        public int idmencao { get; set; }
        public Nullable<int> idperfil { get; set; }
        public string idtwitter { get; set; }
        public string idtwrespondido { get; set; }
        public string postagem { get; set; }
        public DateTime datahora { get; set; }
        public Nullable<bool> is_retweet { get; set; }
        public Nullable<bool> is_reply { get; set; }
        public Nullable<int> qtdretweets { get; set; }
        public Nullable<int> qtdfavoritado { get; set; }
        public string urlimagem { get; set; }
        public string nomeimagem { get; set; }
        public string nomeusuario { get; set; }
        public string idusuario { get; set; }
        public bool termoscapturados { get; set; }
    
        public virtual tw_perfis tw_perfis { get; set; }
        public virtual ICollection<tw_mencao_termos> tw_mencao_termos { get; set; }
    }
}
