using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class PerfilController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Redefinirsenha()
        {
            ViewBag.idUsuario = ((Usuario)Session["usuario"]).idUsuario;
            return View();
        }

        [HttpPost]
        public ActionResult redefinirsenha(FormCollection usuario)
        {
            var idusuarioatual = ((Usuario)Session["usuario"]).idUsuario;

            var rnusuario = new ZRN.Usuarios.Usuarios();

            var msg = rnusuario.AlterarSenha(idusuarioatual, usuario["senhaantiga"], usuario["senhanova"], usuario["confirmasenha"]);


            return View("redefinirsenha", msg);

        }

    }
}