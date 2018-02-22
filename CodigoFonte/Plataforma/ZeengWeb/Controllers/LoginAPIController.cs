using System.Web;
using System.Web.Http;
using System.Web.Services;
using System.Web.SessionState;
using ZeengWeb.ViewModel.Login;
using ZRN.Mensagem;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class LoginAPIController : ApiController
    {

        [HttpPost]
      
        public Mensagem Login([FromBody]Login login)
        {
          
            var RNUsuario = new ZRN.Usuarios.Usuarios();
            Usuario usuario = null;

            var resultado = RNUsuario.ValidaLogin(login.login, login.senha, out usuario);

            if (resultado.Codigo == 1) {

                var context = Request.Properties["MS_HttpContext"] as HttpContextWrapper;

                context.Session["usuario"] = usuario;
            }

            return resultado;
        }


        public Mensagem Novasenha([FromBody]string email){

            var RNUsuario = new ZRN.Usuarios.Usuarios();

            var resultado = RNUsuario.NovaSenhaPorEmail(email);

            return resultado;

        }


    }
}
