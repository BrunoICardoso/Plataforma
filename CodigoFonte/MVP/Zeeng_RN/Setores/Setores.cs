using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeeng_RN.Setores
{
    public class Setores
    {

        public List<Setor> RetornaSetores()
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var setores = (from s in db.setoresempresa
                          select new Setor()
                          {
                              idSetorEmpresa = s.idsetoresempresa,
                              nome = s.nome
                          }).ToList();

            return setores;

        }
    }
}
