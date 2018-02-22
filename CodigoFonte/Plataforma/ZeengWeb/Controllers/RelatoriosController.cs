using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZeengWeb.Utils;
using SelectPdf;

namespace ZeengWeb.Controllers
{
    public class RelatoriosController : Controller
    {

        public ActionResult Index()
        {
            var cliente = new ZRN.Clientes.Clientes();
            var vertentes = cliente.RetornaVertentesAssociada(Configuracoes.idClienteLogado);
            if (vertentes.redessociais == false)
            {
                Session["AcessoRedeSocial"] = false;
                return RedirectToAction("Index", "Empresa");
            }

            Session["AcessoRedeSocial"] = true;

            return View();
        }

        public ActionResult Cadastrar()
        {

            var cliente = new ZRN.Clientes.Clientes();
            var vertentes = cliente.RetornaVertentesAssociada(Configuracoes.idClienteLogado);
            if (vertentes.redessociais == false)
            {
                Session["AcessoRedeSocial"] = false;
                return RedirectToAction("Index", "Empresa");
            }

            Session["AcessoRedeSocial"] = true;
            
            int IdClienteLogado = Configuracoes.idClienteLogado;
            int IdUsuarioLogado = Configuracoes.idUsuarioLogado;        

            ViewBag.idCliente = IdClienteLogado;
            ViewBag.idUsuario = IdUsuarioLogado;
            
            return View();

        }

        public ActionResult Visualizar(int idRelatorio)
        {
            var cliente = new ZRN.Clientes.Clientes();
            var vertentes = cliente.RetornaVertentesAssociada(Configuracoes.idClienteLogado);
            if (vertentes.redessociais == false)
            {
                Session["AcessoRedeSocial"] = false;
                return RedirectToAction("Index", "Empresa");
            }

            Session["AcessoRedeSocial"] = true;

            ViewBag.idRelatorio = idRelatorio;

            return View();
        }

        public ActionResult Editar(int idRelatorio)
        {

            var cliente = new ZRN.Clientes.Clientes();
            var vertentes = cliente.RetornaVertentesAssociada(Configuracoes.idClienteLogado);
            if (vertentes.redessociais == false)
            {
                Session["AcessoRedeSocial"] = false;
                return RedirectToAction("Index", "Empresa");
            }

            Session["AcessoRedeSocial"] = true;

            int IdClienteLogado = Configuracoes.idClienteLogado;

            ViewBag.idCliente = IdClienteLogado;
            ViewBag.idRelatorio = idRelatorio;

            return View();
        }

        [HttpGet]
        public void GerarCSV(int idRelatorio, string nomeRedeSocial, string conteudo)
        {
            var rn = new ZRN.Relatorios.Relatorios();

            var nomeArquivo = nomeRedeSocial + "_" + idRelatorio + ".csv";

            if (conteudo != null)
            {
                //byte[] tratar = System.Text.Encoding.Default.GetBytes(conteudo);
                //var conteudoTratado = System.Text.Encoding.UTF8.GetString(tratar);

                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine("\uFEFF" + conteudo);
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename="+ nomeArquivo);
                //Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                Response.BinaryWrite(bytes);
                Response.End();
            }

        }

        [HttpGet]
        public void GerarPDF(int idRelatorio, string conteudo)
        {
            string html = @"<html>
                             <body style='background-color: gray;'>
                              Hello World from selectpdf.com.
                             </body>
                            </html>
                            ";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                "A4", true);

            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                "Portrait", true);

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;

            PdfDocument doc = converter.ConvertHtmlString(html);
          
            MemoryStream memoryStream = new MemoryStream();

            doc.Save(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=arquivoPDF.pdf");
            Response.BinaryWrite(bytes);
            Response.End();

            doc.Close();
        }
    }
}