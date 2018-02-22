using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeeng_RN.Empresas;

namespace Inspinia_MVC5.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa
        public ActionResult Index()
        {
            return View();
        }

        // GET: Empresa
        public ActionResult Perfil()
        {
            return View();
        }

        // GET: Empresa
        public ActionResult Inserir()
        {
            var setores = new Zeeng_RN.Setores.Setores();

            ViewBag.Setores = setores.RetornaSetores();


            return View();
        }
        [HttpPost]
        public ActionResult Inserir(Empresa empresa)
        {
            var empresas = new Zeeng_RN.Empresas.Empresas();

            empresas.Cadastrar(empresa);

            return View();
        }
        // GET: Empresa
        public ActionResult Noticias()
        {
            return View();
        }

        // GET: Empresa
        public ActionResult Noticia_Detalhe()
        {
            return View();
        }

        public ActionResult LancamentoProduto()
        {
            return View();
        }

        public ActionResult Social()
        {
            return View();
        }

        public ActionResult PresencaOnline()
        {
            return View();
        }

        public ActionResult Promocoes()
        {
            return View();
        }

        public ActionResult Editar(int id)
        {

            var empresas = new Zeeng_RN.Empresas.Empresas();

            var emp = empresas.RetornaEmpresa(id);
            ViewBag.Setores = empresas.RetornaSetores();


            return View(emp);
        }   

        [HttpPost]

        public ActionResult Editar(Empresa emp)
        {

            return View(emp);
        }
    }
}