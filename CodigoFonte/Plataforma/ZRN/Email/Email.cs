using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZRN.Usuarios;

namespace ZRN.Email
{
    public class Email
    {

        public static void EnviaMensagem(string destinatario, string assunto, string mensagem)
        {

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("noreply@zeeng.com.br", "zeeng12345#");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("noreply@zeeng.com.br");
            mail.From = new MailAddress("noreply@zeeng.com.br");
            mail.To.Add(new MailAddress(destinatario));
            mail.Subject = assunto;
            mail.Body = mensagem;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
                // Console.WriteLine("enviou");
            }
            catch (System.Exception e)
            {
                //Console.WriteLine("erro");
                //  //trata erro
            }
            finally
            {
                mail = null;
            }

        }

        public void EnviarMensagemAcessoTrial(ZRN.Clientes.AcessoTrial mensagem)
        {

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("noreply@zeeng.com.br", "zeeng12345#");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("noreply@zeeng.com.br");
            mail.From = new MailAddress("noreply@zeeng.com.br");
            mail.To.Add(new MailAddress("contato@zeeng.com.br"));
            //mail.To.Add(new MailAddress("matheus@zeeng.com.br"));
            //mail.To.Add(new MailAddress("bruno.cardoso@zeeng.com.br"));

            var assunto = "Cliente "+mensagem.cliente.nome + " tentou acesso com o usuario " + mensagem.usuario.nome + " " + DateTime.Now.ToString();

            mail.Subject = assunto;

            var CorpoMensagem = "========== Tentativa de Acesso ==============<br /><br />" +
                                 "ID Usuario: " + mensagem.usuario.idUsuario + "<br />" +
                                  "Nome: " + mensagem.usuario.nome + "<br />" +
                                  "Email: " + mensagem.usuario.email + "<br /><br />" +
                                  "=========================================<br /><br />" +
                                  "ID Cliente: " + mensagem.cliente.idcliente + "<br />" +
                                  "Nome: " + mensagem.cliente.nome + "<br />" +
                                  "=========================================<br /><br />" +
                                  "E-mail enviado " + DateTime.Now.ToString();

            mail.Body = CorpoMensagem;

            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);

                mail.Dispose();

                var respostaMensagem = new Mensagem.Mensagem()
                {
                    Codigo = 1,
                    Texto = "E-mail Enviado."
                };

             
            }
            catch (System.Exception e)
            {
                //
                var respostaMensagem = new Mensagem.Mensagem()
                {
                    Codigo = 0,
                    Texto = "Erro ao enviar sua mensagem."
                };

             

            }
            finally
            {
                mail = null;
            }




        }

        public static Mensagem.Mensagem EnviaMensagemComAnexo(EmailModeloFormulario email)
        {

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("noreply@zeeng.com.br", "zeeng12345#");
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress("noreply@zeeng.com.br");
            mail.From = new MailAddress("noreply@zeeng.com.br");
            mail.To.Add(new MailAddress("contato@zeeng.com.br"));
            
            var assunto = email.assunto + " / " + email.nomeUsuario + " / " + DateTime.Now.ToString();

            mail.Subject = assunto;

            var mensagem = "========== Contato do cliente ==============<br /><br />" +
                           "ID: " + email.idUsuario + "<br />" +
                           "Nome: " + email.nomeUsuario + "<br />" +
                           "Email: " + email.emailUsuario + "<br /><br />" +
                           "=========================================<br /><br />" +
                           "Página atual: " + email.urlAtual + "<br />" +
                           "Assunto: " + email.assunto + "<br />" +
                           "Mensagem: " + email.mensagem + "<br /><br />" +
                           "=========================================<br /><br />" +
                           "E-mail enviado " + DateTime.Now.ToString();

            mail.Body = mensagem;

            if (email.nomeArquivo != null)
            {

                // var caminho = System.AppDomain.CurrentDomain.BaseDirectory + "ArquivosEmailContato\\" + email.nomeArquivo;


                string arquivo = Path.GetExtension(email.nomeArquivo);



                if (arquivo == ".doc" ||
                    arquivo == ".png" ||
                    arquivo == ".jpg" ||
                    arquivo == ".jpeg" ||
                    arquivo == ".pdf" ||
                    arquivo == ".gif"
                    )
                {

                    //long tamanho = new System.IO.FileInfo(caminho).Length;
                    //if (tamanho < 15000000) { 

                    //    var attachment = new Attachment(caminho);
                    //    mail.Attachments.Add(attachment);

                    //}

                    //long tamanho = new System.IO.FileInfo(caminho).Length;
                    if (email.tamanhoAnexo < 7000000)
                    {

                        var attachment = new Attachment(email.anexo, email.nomeArquivo);
                        mail.Attachments.Add(attachment);

                    }

                }
                else
                {

                    mail.Dispose();

                    // File.Delete(caminho);

                    var respostaMensagem = new Mensagem.Mensagem()
                    {
                        Codigo = 0,
                        Texto = "O arquivo selecionado é inválido ou muito grande. Entre em contato através do e-mail contato@zeeng.com.br"
                    };

                    return respostaMensagem;

                }
            }


            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);

                mail.Dispose();


                //if(email.nomeArquivo != null)
                //{
                //    var caminho = System.AppDomain.CurrentDomain.BaseDirectory + "ArquivosEmailContato\\" + email.nomeArquivo;

                //    File.Delete(caminho);
                //}

                var respostaMensagem = new Mensagem.Mensagem()
                {
                    Codigo = 1,
                    Texto = "Sua mensagem foi enviada com sucesso. Entraremos em contato o mais breve possível. Obrigado! :)"
                };

                return respostaMensagem;

            }
            catch (System.Exception e)
            {
                //
                var respostaMensagem = new Mensagem.Mensagem()
                {
                    Codigo = 0,
                    Texto = "Erro ao enviar sua mensagem. Por favor entrar em contato com a nossa equipe através do e-mail contato@zeeng.com.br"
                };

                return respostaMensagem;

            }
            finally
            {
                mail = null;
            }





        }


    }
}
