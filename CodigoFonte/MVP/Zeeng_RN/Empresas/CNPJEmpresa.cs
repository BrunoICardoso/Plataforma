using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeeng_RN.Empresas
{
    public class CNPJEmpresa
    {
        public int idEmpresa { get; set; }
        public int? idCNPJEmpresa { get; set; }
        public string CNPJ { get; set; }

        public CNPJEmpresa()
        {

        }

        public CNPJEmpresa(Zeeng_DB.EF.cnpjempresa c)
        {
            this.idCNPJEmpresa = c.idcnpjempresa;
            this.idEmpresa = c.idempresa;
            this.CNPJ = c.cnpj;
        }
    }
}
