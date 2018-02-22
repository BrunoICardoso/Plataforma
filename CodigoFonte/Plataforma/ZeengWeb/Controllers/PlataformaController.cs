using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class PlataformaController : Controller
    {

        // Níveis de logo: INFO, ERRO

        public void GerarLog(string nivel = "INFO", string mensagem = "")
        {
            var controllerNome = this.ControllerContext.RouteData.Values["controller"].ToString();
            var actionNome = this.ControllerContext.RouteData.Values["action"].ToString();
            var descricao = mensagem == "" ? DescricaoPadrao(controllerNome + "/" + actionNome) : mensagem;

            ZRN.Logs.Log Log = new ZRN.Logs.Log();
            Log.datahora = DateTime.Now;
            Log.acao = actionNome;
            Log.controle = controllerNome;
            Log.descricao = descricao;
            Log.idusuario = ((Usuario)Session["usuario"]).idUsuario;
            Log.url = HttpContext.Request.Url.ToString();
            Log.nivel = nivel;

            ZRN.Logs.Logs rnLog = new ZRN.Logs.Logs();
            rnLog.GravarLog(Log);
        }

        public string DescricaoPadrao(string ControllerAction)
        {
            Dictionary<string, string> Descricao = new Dictionary<string, string>();
            Descricao["login/login"] = "Usuário logou no sistema";
            Descricao["Empresa/Perfil"] = "Este é um texto da padrão de perfil de empresa";
            Descricao["Empresa/Index"] = "Este é um texto da padrão de INDEX de empresa";            
            Descricao["Empresa/Noticias"] = "Acesso a tela de Notícias da Empresa";

            if (Descricao.ContainsKey(ControllerAction))
                return Descricao[ControllerAction];
            else
                return "Texto padrão";
        }
    }
}