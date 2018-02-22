using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZeengWeb.Utils;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class EmailAPIController : ApiController
    {

        [HttpPost]
        public ZRN.Mensagem.Mensagem Post()
        {
            
            var httpPostedFile = HttpContext.Current.Request.Files["arquivosEmail"];
            string jsonEmail = HttpContext.Current.Request.Form[0];

            ZRN.Email.EmailModeloFormulario emailModelo = JsonConvert.DeserializeObject<ZRN.Email.EmailModeloFormulario>(jsonEmail);

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                
                if (httpPostedFile != null)
                {
                    var nomeArquivo = emailModelo.idUsuario + Path.GetExtension(httpPostedFile.FileName);
                    emailModelo.nomeArquivo = nomeArquivo;
                    emailModelo.anexo = httpPostedFile.InputStream;
                    emailModelo.tamanhoAnexo = httpPostedFile.ContentLength;

                    //string arquivo = Path.GetExtension(httpPostedFile.FileName);
                    //if (arquivo == ".doc" || arquivo == ".png" || arquivo == ".jpg" || arquivo == ".jpeg" || arquivo == ".pdf" || arquivo == ".gif")
                    //{

                    //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ArquivosEmailContato"), nomeArquivo);
                    //    httpPostedFile.SaveAs(fileSavePath);

                    //}
                    
                }

            }

            var resposta = ZRN.Email.Email.EnviaMensagemComAnexo(emailModelo);
             
            return resposta;
        }

      
        }
}
