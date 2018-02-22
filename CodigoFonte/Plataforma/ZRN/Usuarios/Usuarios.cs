using System;
using System.Linq;
using ZRN.Seguranca;

namespace ZRN.Usuarios
{

    public class Usuarios
    {
        
        public Mensagem.Mensagem ValidaLogin(string login, string senha, out Usuario usuario) {

            var db = new ZBD.Model.zeengEntities();

            var senhaCriptografada = Criptografia.Criptografar(senha);

            usuario = (from userDB in db.usuarios
                             where 
                                (userDB.email.ToLower() == login.ToLower() ||
                                    userDB.login.ToLower() == login.ToLower()) &&
                                    userDB.senha == senhaCriptografada &&
                                    userDB.ativo == true
                             select new Usuario() {
                                 ativo = userDB.ativo,
                                 email = userDB.email,
                                 excluido = userDB.excluido, 
                                 idCliente = userDB.idcliente,
                                 idUsuario = userDB.idusuario,
                                 login = userDB.login,
                                 nome = userDB.nome
                             }
                             ).FirstOrDefault();

            var msg = new Mensagem.Mensagem();            
            if(usuario != null)
            {
                msg.Codigo = 1;
                msg.Texto = "Login efetuado com sucesso!";
                msg.Tipo = Mensagem.Mensagem.tipoMensagem.Informativa;
            }
            else{
                msg.Codigo = 0;
                msg.Texto = "E-mail e senha não conferem";
                msg.Tipo = Mensagem.Mensagem.tipoMensagem.Erro;
            }

            return msg;
        }

        public bool VerificaUsuarioAtivo(string login)
        {
            var resultado = false;

            var db = new ZBD.Model.zeengEntities();
            
            var usuarioDB = (from userDB in db.usuarios
                             where userDB.login.ToLower() == login.ToLower()
                             select userDB).FirstOrDefault();

            if (usuarioDB != null)
            {
                if (usuarioDB.ativo == true)
                {
                    resultado = true;
                }
               
            }

            return resultado;
            
        }
        
        public bool VerificaLoginExistente(string login)
        {
            var resultado = false;

            var db = new ZBD.Model.zeengEntities();

            var usuarioDB = (from userDB in db.usuarios
                             where userDB.login.ToLower() == login.ToLower()
                             select userDB).FirstOrDefault();

            if (usuarioDB != null)
            {
                resultado = true;
            }

            return resultado;
        }

        //public bool VerificaEmailUsuarioValido(string email)
        //{
        //    var db = new ZBD.Model.zeengEntities();

        //    bool resultado = false;

        //    var usuarioDB = (from userDB in db.usuarios
        //                     where userDB.email.ToLower() == email.ToLower()
        //                     select userDB).FirstOrDefault();


        //    return resultado;
        //}

        public Mensagem.Mensagem NovaSenhaPorEmail(string email) {

            var db = new ZBD.Model.zeengEntities();

            var usuarioDB = (from userDB in db.usuarios
                             where userDB.email.ToLower() == email.ToLower()
                             select userDB).FirstOrDefault();

            if(usuarioDB != null)
            {
                string novaSenha = novaSenhaAleatoria();

                string senhaCriptografada = Criptografia.Criptografar(novaSenha);

                usuarioDB.senha = senhaCriptografada;
                db.SaveChanges();

                string assunto = "Nova senha - Suporte Zeeng";
                string mensagem = "Prezado(a) " + usuarioDB.nome.ToString() + ".<br /><br />"
                                                + "Conforme solicitado foi gerado uma nova senha para acesso na plataforma Zeeng.<br /><br />"
                                                + "Sua nova senha é: " + novaSenha + "<br /><br /><br />"
                                                + "E-mail enviado "+DateTime.Now.ToString();
                string destinatario = usuarioDB.email;

                Email.Email.EnviaMensagem(destinatario, assunto, mensagem);


                return new Mensagem.Mensagem("E-mail enviado com a nova senha!", Mensagem.Mensagem.tipoMensagem.Informativa, 0);

            }
            else
            {
                return new Mensagem.Mensagem("E-mail não cadastrado ou incorreto!", Mensagem.Mensagem.tipoMensagem.Erro, 1);
            }
            
        }
        
        public Mensagem.Mensagem AlterarSenha(int idUsuario, string senhaantiga, string senhanova, string confirmasenha) {
            
            var db = new ZBD.Model.zeengEntities();
            Mensagem.Mensagem msg = new Mensagem.Mensagem();
        
            //Verifica se a nova senha é igual a confirma senha
            if (senhanova != confirmasenha)
            {
                msg.Tipo = Mensagem.Mensagem.tipoMensagem.Erro;
                msg.Codigo = 1;
                msg.Texto = "As novas senhas não conferem";

                return msg;
            }

            //Verifica se a senha antiga é igual a existente no banco de dados para esse usuário
            var senhaCriptografada = Criptografia.Criptografar(senhaantiga);

            var usuarioDB = (from userDB in db.usuarios
                             where userDB.idusuario == idUsuario &&
                                    userDB.senha == senhaCriptografada
                             select userDB).FirstOrDefault();

            if (usuarioDB != null)
            {
                var senhaNovaCriptografada = Criptografia.Criptografar(senhanova);
                usuarioDB.senha = senhaNovaCriptografada;
                db.SaveChanges();

                return new Mensagem.Mensagem("Senha alterada com sucesso!", Mensagem.Mensagem.tipoMensagem.Sucesso,0);
            }
            else
            {
                return new Mensagem.Mensagem("Erro ao alterar senha! Login ou Senha está incorreto!", Mensagem.Mensagem.tipoMensagem.Erro,2);
            }
        }
                
        public string novaSenhaAleatoria()
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string senhaAleatoria = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return senhaAleatoria;
        }
                
    }

}
