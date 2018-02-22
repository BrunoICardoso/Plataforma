using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeeng_RN.Empresas
{
    public class Empresas
    {

        public List<Empresa> RetornarEmpresas()
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var empresas = (from e in db.empresas
                            select new Empresa(e)).ToList();

            return empresas;

        }

        public Empresa RetornaEmpresa(int idEmpresa)
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var empresa = (from e in db.empresas
                           where e.idempresa == idEmpresa
                           select new Empresa()
                           {
                               idempresa = e.idempresa,
                               idsetor = e.idsetor,
                               nome = e.nome,
                               descricao = e.descricao,
                               urlsite = e.urlsite,
                               NomeSetor = e.setoresempresa.nome,
                               CNPJs = (from c in e.cnpjempresa
                                       select new CNPJEmpresa() {
                                           idEmpresa = c.idempresa,
                                           idCNPJEmpresa = c.idcnpjempresa,
                                           CNPJ = c.cnpj
                                       })

                           }).FirstOrDefault();

            return empresa;

        }

        public void Cadastrar(Empresa emp)
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var empresaDB = new Zeeng_DB.EF.empresas();
            empresaDB.idsetor = emp.idsetor;
            empresaDB.nome = emp.nome;
            empresaDB.urlsite = emp.urlsite;


            foreach (var cnpj in emp.CNPJs)
            {
                if (cnpj.idCNPJEmpresa == null)
                {

                    var c = new Zeeng_DB.EF.cnpjempresa();
                    c.cnpj = cnpj.CNPJ;
                    empresaDB.cnpjempresa.Add(c);
                }
                else
                {

                }
            }


            db.empresas.Add(empresaDB);

            db.SaveChanges();

        }

        public void Editar(Empresa emp)
        {

            var db = new Zeeng_DB.EF.zeeng_Entities();

            var empresaDB = (from e in db.empresas
                             where e.idempresa == emp.idempresa
                             select e).FirstOrDefault();

            empresaDB.idsetor = emp.idsetor;
            empresaDB.nome = emp.nome;
            empresaDB.urlsite = emp.urlsite;

            db.SaveChanges();
        }

        public void Delete(int idEmpresa)
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var emp = (from e in db.empresas
                       where e.idempresa == idEmpresa
                       select e).SingleOrDefault();

            db.empresas.Remove(emp);

            db.SaveChanges();


        }

        public List<SetorEmpresa> RetornaSetores()
        {
            var db = new Zeeng_DB.EF.zeeng_Entities();

            var setores = (from s in db.setoresempresa
                           select new SetorEmpresa()
                           {
                               idSetor = s.idsetoresempresa,
                               nome = s.nome
                           }).ToList();

            return setores;

        }

    }
}
