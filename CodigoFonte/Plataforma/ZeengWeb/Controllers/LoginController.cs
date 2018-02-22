using System.Web.Mvc;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class LoginController : PlataformaController
    {


        // GET: Login/Create
        public ActionResult Index()
        {

            //var BRtoUTC = Utils.FuncoesDatas.ConvertDataHoraTimeZoneBrasiltoUTC(System.DateTime.Now);
            //var UTCtoBR = Utils.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(new System.DateTime(2017, 03, 03, 18, 49, 00));

            var msg = new ZRN.Mensagem.Mensagem();
            msg.Codigo = 1;

            return View(msg);
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            Session["AcessoRedeSocial"] = false;

            try
            {
                var RNUsuario = new ZRN.Usuarios.Usuarios();
                var RNEmail = new ZRN.Email.Email();

                Usuario usuario = null;

                var resultado = RNUsuario.ValidaLogin(collection["login"], collection["senha"], out usuario);
                Session["usuario"] = usuario;
                
                // Verifica se o cliente do usuário logado tem empresas associadas
                // ==================================================================================
                var cliente = new ZRN.Clientes.Clientes();

                if (cliente.VerificarDataAcessoTrial(usuario.idCliente) == true) {

                    var ClienteUsuario = cliente.retornarUsuarioCliente(usuario.idUsuario);

                    RNEmail.EnviarMensagemAcessoTrial(ClienteUsuario);

                    return View("index", new ZRN.Mensagem.Mensagem() { Codigo = 0, Texto = "Acesso expirado."});

                }

                if (cliente.VerificaEmpresasDoCliente(usuario.idCliente) == false)
                {
                    return RedirectToAction("ConfiguracoesCliente","Empresa", new { idCliente = usuario.idCliente });
                }

                // Verifica se o cliente do usuário logado tem acesso a vertente Redes Socias
                // ==================================================================================                
                var vertentes = cliente.RetornaVertentesAssociada(usuario.idCliente);
                if (vertentes.redessociais == true)
                    Session["AcessoRedeSocial"] = true;

                if (resultado.Codigo == 1){
                    //this.GerarLog("INFO");
                    return RedirectToAction("Index", "Empresa");
                }
                                
                //Se ocorrer erro na senha/usuario ou se os campo estiverem vazios
                if (string.IsNullOrEmpty(collection["login"]) && string.IsNullOrEmpty(collection["senha"])){

                    return View("index", new ZRN.Mensagem.Mensagem() { Codigo = 0, Texto = "E-mail e senha não informados." });
                }

                if (string.IsNullOrEmpty(collection["login"])){

                    return View("index", new ZRN.Mensagem.Mensagem() { Codigo = 0, Texto = "E-mail não informado." });
                }

                if (string.IsNullOrEmpty(collection["senha"])){
                    return View("index", new ZRN.Mensagem.Mensagem() { Codigo = 0, Texto = "Senha não informada." });
                }
                
                return View("index", resultado);
            }
            catch (System.Exception e)
            {
                //this.GerarLog("ERRO", "Erro ao efetuar login - " + e.Message + " - " + e.StackTrace);
                return View("index", new ZRN.Mensagem.Mensagem() { Codigo = 0, Texto = "Erro ao efetuar login", MensagemException = e.Message + " - " + e.StackTrace});
            }
        }
        

        // POST: Login/Create
        [HttpGet]
        public ActionResult logout()
        {
            try
            {
                //this.GerarLog("INFO","Usuário deslogou do sistema");

                Session["usuario"] = null;
                // TODO: Add insert logic here
                return RedirectToAction("Index", "login");
            }
            catch
            {
                return View();
            }
        }
    }
}
