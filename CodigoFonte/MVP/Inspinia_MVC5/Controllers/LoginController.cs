using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string senha)
        {

            //if (usuario == "Andre" && senha == "123")
            //{
                Session["logado"] = "true";
            //return RedirectToAction("Index", "Empresa");
            return RedirectToAction("Dashboard_2", "Dashboards");
            
            //}
            //else {
            //    ViewBag.Mensagem = "Usuário e senha inválido!";
            //    return View();
            //}
        }
    }
}