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
    
    public partial class mapa_dadoscaptura
    {
        public mapa_dadoscaptura()
        {
            this.mapa_statusdadoscaptura = new HashSet<mapa_statusdadoscaptura>();
        }
    
        public int iddadoscaptura { get; set; }
        public Nullable<int> idmapacaptura { get; set; }
        public Nullable<System.DateTime> datahoracaptura { get; set; }
        public string uf { get; set; }
        public string area { get; set; }
        public string especie { get; set; }
        public string subespecie { get; set; }
        public string @base { get; set; }
        public string caracteristica { get; set; }
        public string atributo { get; set; }
        public string complemento { get; set; }
        public string estabelecimento { get; set; }
        public string cnpj { get; set; }
        public string produto { get; set; }
        public string dataconcessao { get; set; }
        public Nullable<System.DateTime> dtdataconcessao { get; set; }
        public string registro { get; set; }
        public string marca { get; set; }
        public string origem { get; set; }
        public string modoaplicacao { get; set; }
        public string status { get; set; }
        public Nullable<bool> excluido { get; set; }
    
        public virtual mapa_capturas mapa_capturas { get; set; }
        public virtual ICollection<mapa_statusdadoscaptura> mapa_statusdadoscaptura { get; set; }
    }
}