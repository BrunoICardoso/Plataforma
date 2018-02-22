using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeengWeb.Utils;
using ZeengWeb.ViewModel.Empresa;
using ZRN.Usuarios;

namespace ZeengWeb.Controllers
{
    public class EmpresaController : PlataformaController
    {

        // GET: Empresa
        public ActionResult Index()
        {
            //this.GerarLog("INFO");

            return View();
        }

        // GET: Empresa/Perfil/5
        public ActionResult Perfil(int id)
        {

            int idcliente = ((Usuario)Session["usuario"]).idCliente;

            var RN = new ZRN.Empresas.Empresas();
            var RNclientes = new ZRN.Clientes.Clientes();

            if (RNclientes.VerificaAcessoEmpresa(id, idcliente) == true)
            {

                var empresaPerfil = new ViewModel.Empresa.EmpresaPerfil();
                empresaPerfil.Empresa = RN.RetornaPerfilEmpresa(id);
                empresaPerfil.VertentesCliente = RNclientes.RetornaVertentesAssociada(idcliente);

                return View(empresaPerfil);
            }
            else
            {

                return RedirectToAction("SemcessoPerfil", "Empresa");

            }



        }

        public ActionResult SemcessoPerfil()
        {

            return View();

        }



        // GET: Empresa/Perfil/5
        public ActionResult LancamentoProduto(int id)
        {


            int idcliente = ((Usuario)Session["usuario"]).idCliente;

            var RNCliente = new ZRN.Clientes.Clientes();

            var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.Produtos, id, idcliente);

            if (verificarAcesso)
            {
                var RN = new ZRN.Empresas.Empresas();
                var empresaView = new ViewModel.Empresa.EmpresaLancamento();
                empresaView.Empresa = RN.RetornaPerfilEmpresa(id);

                return View(empresaView);
            }
            else
            {

                return RedirectToAction("produtos_semAcesso", "Empresa");
            }
        }

        public ActionResult produtos_semAcesso()
        {
            return View();
        }

        // GET: Empresa/RedesSociais/5
        public ActionResult RedesSociais(int id)
        {


            int idcliente = ((Usuario)Session["usuario"]).idCliente;

            var RNCliente = new ZRN.Clientes.Clientes();

            var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.RedesSociais, id, idcliente);

            if (verificarAcesso)
            {

                var RN = new ZRN.Empresas.Empresas();

                var RNSocial = new ZRN.RedesSociais.Facebook.Facebook();
                var RNSocialTw = new ZRN.RedesSociais.Twitter.Twitter();
                var RNSocialInsta = new ZRN.RedesSociais.Instagram.Instagram();
                var RNSocialYoutube = new ZRN.RedesSociais.Youtube.Youtube();
                var RNtemRedesSociais = new ZRN.RedesSociais.RedesSociais();

                var empresaView = new ViewModel.Empresa.EmpresaSocial();
                empresaView.Empresa = RN.RetornaPerfilEmpresa(id);
                empresaView.TotalSeguidoresFace = RNSocial.RetornaSeguidores(id);
                empresaView.TotalSeguidoresTw = RNSocialTw.RetornaSeguidores(id);
                empresaView.TotalSeguidoresInsta = RNSocialInsta.RetornaSeguidoresInsta(id);
                empresaView.TotalSeguidoresYoutube = RNSocialYoutube.RetornaSeguidoresYoutube(id);

                empresaView.temRedesSocias = RNtemRedesSociais.VerificaRedesSociaisEmpresas(id);

                return View(empresaView);
            }
            else
            {

                return RedirectToAction("redessociais_semAcesso", "Empresa");
            }

        }


        public ActionResult redessociais_semAcesso()
        {
            return View();
        }

        // GET: Empresa
        public ActionResult Noticias(int id)
        {
            var RNE = new ZRN.Empresas.Empresas();
            int idcliente = ((Usuario)Session["usuario"]).idCliente;

            var RNCliente = new ZRN.Clientes.Clientes();

            var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.Noticias, id, idcliente);

            if (verificarAcesso)
            {
                ViewBag.idEmpresa = id;
                var rn = new ZRN.Empresas.Noticias();
                var rnempresas = new ZRN.Empresas.Empresas();
                var noticiaView = new ViewModel.Empresa.EmpresaNoticias();

                noticiaView.noticiafonteempresa = rn.RetornaFonteNotciasEmpresa(id);
                noticiaView.empresa = rnempresas.RetornaDescricaoEmpresa(id);

                //this.GerarLog("INFO");

                return View(noticiaView);
            }
            else
            {

                return RedirectToAction("noticias_semAcesso", "Empresa");

            }
        }

        public ActionResult noticias_semAcesso()
        {
            return View();
        }


        public ActionResult Noticia_Detalhe(int id, string textoPesquisado = "")
        {
            var rn = new ZRN.NoticiasElastic.NoticiasElastic(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            var rnempresas = new ZRN.Empresas.Empresas();

            var view = new ViewModel.Empresa.EmpresaNoticiaDetalhe();

            view.noticia = rn.RetornaNoticiaEmpresa(id, textoPesquisado);

            var idEmpresa = (from e in view.noticia.empresas
                             where e.idnoticiaempresa == id
                             select e.idempresa).FirstOrDefault();

            view.empresa = rnempresas.RetornaDescricaoEmpresa(idEmpresa);

            ViewBag.idEmpresa = idEmpresa;

            ViewBag.LayoutTopo = "EmpresaNoticiaDetalhe";

            //ViewBag.NomeEmpresa = view.noticia.empresas.FirstOrDefault().nomeempresa;
            //ViewBag.IdEmpresa = view.noticia.empresas.FirstOrDefault().idempresa;
            ViewBag.NomeEmpresa = view.noticia.empresas.Where(x => x.idempresa == idEmpresa).Select(x => x.nomeempresa).FirstOrDefault();
            ViewBag.IdEmpresa = idEmpresa;
            return View(view);
        }

        public ActionResult PresencaOnline(int id)
        {


            ViewBag.idEmpresa = id;
            var rn = new ZRN.Empresas.Noticias();
            var rnempresas = new ZRN.Empresas.Empresas();

            int idcliente = ((Usuario)Session["usuario"]).idCliente;
            var RNCliente = new ZRN.Clientes.Clientes();

            var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.PresencaOnline, id, idcliente);

            if (verificarAcesso)
            {

                var noticiaView = new ViewModel.Empresa.EmpresaNoticias();

                noticiaView.noticiafonteempresa = rn.RetornaFonteNotciasEmpresa(id);
                noticiaView.empresa = rnempresas.RetornaDescricaoEmpresa(id);

                return View(noticiaView);
            }

            else
            {

                return RedirectToAction("PresencaOnline_semAcesso", "Empresa");

            }

        }

        public ActionResult PresencaOnline_semAcesso()
        {
            return View();
        }

        //public ActionResult Promocoes(int id)
        //{
        //    var RNEmpresa = new ZRN.Empresas.Empresas();
        //    var RNSituacao = new ZRN.Promocoes.Situacoes();
        //    var empPromoView = new ViewModel.Empresa.EmpresaPromocoes();

        //    int idcliente = ((Usuario)Session["usuario"]).idCliente;

        //    var RNCliente = new ZRN.Clientes.Clientes();

        //    var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.Promocoes, id, idcliente);

        //    if (verificarAcesso)
        //    {

        //        empPromoView.Empresa = RNEmpresa.RetornaPerfilEmpresa(id);
        //        empPromoView.Situacoes = RNSituacao.RetornaSituacoes();

        //        return View(empPromoView);
        //    }
        //    else {

        //        return RedirectToAction("Perfil", "Empresa", new { id });

        //    }

        //}

        public ActionResult PromoDetalhes()
        {
            return View();
        }

        public ActionResult Promo(int id)
        {


            int idcliente = ((Usuario)Session["usuario"]).idCliente;

            var RNCliente = new ZRN.Clientes.Clientes();

            var verificarAcesso = RNCliente.VerificaAcessoEmpresaVertente(ZRN.Vertentes.enumVertentes.Promocoes, id, idcliente);

            if (verificarAcesso)
            {
                var RNEmpresa = new ZRN.Empresas.Empresas();

                ViewBag.idEmpresa = id;

                var empPromoView = new ViewModel.Empresa.EmpresaPromocoes();
                empPromoView.Empresa = RNEmpresa.RetornaPerfilEmpresa(id);
                return View(empPromoView);

            }
            else
            {

                return RedirectToAction("Promo_semAcesso", "Empresa");

            }
        }

        public ActionResult Promo_semAcesso()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PromoDetalhes(int idPromo, int idEmpresa)
        {
            var view = new ViewModel.Empresa.EmpresaPromoDetralhes();
            var RN = new ZRN.Promocoes.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            var dadosPromocao = RN.RetornaPromocaoDetalhe(idPromo, idEmpresa);

            view.promocao = dadosPromocao;

            var emppresa = new ZRN.Empresas.Empresas();
            var dadosEmp = emppresa.RetornaPerfilEmpresa(idEmpresa);
            view.idEmpresa = dadosEmp.idempresa;
            view.NomeEmpresa = dadosEmp.nome;

            if (dadosPromocao != null && dadosPromocao.empresas != null)
            {
                List<string> listaEmpresas = new List<string>();
                foreach (var emp in dadosPromocao.empresas)
                {
                    listaEmpresas.Add(emp.nome);
                }
                view.empresas = String.Join(", ", listaEmpresas);
            }

            if (dadosPromocao == null)
                view = null;

            return View(view);
        }

        [HttpGet]
        public ActionResult NoticiaDetalhes(int id, int idPromo)
        {
            var rn = new ZRN.NoticiasElastic.NoticiasElastic(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            var rnempresas = new ZRN.Empresas.Empresas();

            var view = new ViewModel.Empresa.EmpresaNoticiaDetalhe();

            view.noticia = rn.RetornaNoticiaEmpresa(id);

            var idEmpresa = (from e in view.noticia.empresas
                             where e.idnoticiaempresa == id
                             select e.idempresa).FirstOrDefault();

            view.empresa = rnempresas.RetornaDescricaoEmpresa(idEmpresa);

            ViewBag.idEmpresa = idEmpresa;

            ViewBag.LayoutTopo = "PromoNoticiaDetalhe";
            ViewBag.NomeEmpresa = view.noticia.empresas.FirstOrDefault().nomeempresa;
            ViewBag.IdEmpresa = view.noticia.empresas.FirstOrDefault().idempresa;
            ViewBag.IdPromocao = idPromo;

            return View("Noticia_Detalhe", view);
        }

        [HttpGet]
        public ActionResult NoticiaDetalhesPromo(int idNoticia, int idPromo, int idEmpresa)
        {
            var rn = new ZRN.NoticiasElastic.NoticiasElastic(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            var view = new ViewModel.Empresa.EmpresaNoticiaDetalhe();
            var rnempresas = new ZRN.Empresas.Empresas();

            view.noticia = rn.GetNoticiaPromocao(idNoticia);
            view.empresa = rnempresas.RetornaDescricaoEmpresa(idEmpresa);

            ViewBag.idEmpresa = idEmpresa;

            ViewBag.LayoutTopo = "PromoNoticiaDetalhe";
            ViewBag.NomeEmpresa = view.empresa.nome;
            ViewBag.IdEmpresa = idEmpresa;
            ViewBag.IdPromocao = idPromo;

            return View("Noticia_Detalhe", view);
        }

        [HttpGet]
        public ActionResult ConfiguracoesCliente(int idCliente)
        {
            var cliente = new ZRN.Clientes.Clientes();
            var clienteRN = new ZRN.Clientes.Clientes();
            var clienteView = new ViewModel.Cliente.ClienteConfiguracao();
            var empresasRN = new ZRN.Clientes.Clientes();

            clienteView.cliente = clienteRN.RetornaCliente(idCliente);
            clienteView.totalEmpresas = empresasRN.TotalEmpresasAtivas();

            return View(clienteView);
        }


    }
}
