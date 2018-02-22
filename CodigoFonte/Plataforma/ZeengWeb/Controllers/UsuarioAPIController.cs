using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZeengWeb.ViewModel.Usuario;
using ZRN.Mensagem;

namespace ZeengWeb.Controllers
{
    public class UsuarioAPIController : ApiController
    {

        [HttpPost]
        public Mensagem alterarsenha([FromBody]RedefinirSenha redefinirSenha)
        {
            var rn = new ZRN.Usuarios.Usuarios();

            ZRN.Usuarios.Usuario usuario = (ZRN.Usuarios.Usuario)HttpContext.Current.Session["usuario"];

            var msg = rn.AlterarSenha(usuario.idUsuario, redefinirSenha.senhaAntiga, redefinirSenha.novasenha, redefinirSenha.confirmasenha);

            return msg;
        }

    }
}
