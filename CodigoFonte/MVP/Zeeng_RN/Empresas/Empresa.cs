using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeeng_RN.Empresas
{
    public class Empresa
    {
        public int idempresa { get; set; }
        public int idsetor { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string urlsite { get; set; }

        public string NomeSetor { get; set; }

        public string UrlFacebook { get; set; }
        public string UrlInstagram { get; set; }
        public string UrlYouTube { get; set; }
        public string UrlTwitter { get; set; }
        public string UrlKlout { get; set; }

        public IEnumerable<CNPJEmpresa> CNPJs{ get; set; }

        public Empresa()
        {
         

        }

        public Empresa(Zeeng_DB.EF.empresas emp)
        {

            this.idempresa = emp.idempresa;
            this.idsetor = emp.idsetor;
            this.nome = emp.nome;
            this.descricao = emp.descricao;
            this.urlsite = emp.urlsite;

            this.NomeSetor = emp.setoresempresa.nome;

            this.CNPJs = emp.cnpjempresa.Select(x=> new CNPJEmpresa(x)).ToList();

        }
    }
}
