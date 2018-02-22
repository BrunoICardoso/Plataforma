using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.LancamentoProdutos
{
    public class LancamentoProduto
    {
        public int idregistro { get; set; }
        public Nullable<int> idestado { get; set; }
        public string nomeEstado { get; set; }
        public Nullable<int> idarea { get; set; }
        public string nomeArea { get; set; }
        public Nullable<int> idespecie { get; set; }
        public string nomeEspecie { get; set; }
        public Nullable<int> idsubespecie { get; set; }
        public string nomeSubEspecie { get; set; }
        public Nullable<int> idbase { get; set; }
        public string nomeBase { get; set; }
        public Nullable<int> idcaracteristica { get; set; }
        public string nomeCaracteristica { get; set; }
        public Nullable<int> idatributo { get; set; }
        public string nomeAtributo { get; set; }
        public Nullable<int> idcomplemento { get; set; }
        public string nomeComplemento { get; set; }
        public Nullable<int> idorigem { get; set; }
        public string nomeOrigem { get; set; }
        public Nullable<int> idempresa { get; set; }
        public string nomeEmpresa { get; set; }
        public Nullable<int> idmarca { get; set; }
        //public string nomeMarca { get; set; }

        private string _nomeMarca;

        public string nomeMarca
        {
            get {
                if (string.IsNullOrEmpty(_nomeMarca))
                    return "Não especificado";
                else
                    return _nomeMarca;
            }

            set { _nomeMarca = value; }
        }

        public string nomeProduto { get; set; }
        public Nullable<System.DateTime> dataconcessao { get; set; }
        public int? ano
        {
            get
            {
                if (dataconcessao.HasValue)
                {
                    return dataconcessao.Value.Year;
                }
                else {
                    return null;
                }
            }
        }
        public string numregistro { get; set; }
        public string modoaplicacao { get; set; }
        public string Status { get; set; }
    }
}
