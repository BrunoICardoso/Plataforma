using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZeengWeb.ViewModel.Empresa;
using ZRN.LancamentoProdutos;

namespace ZeengWeb.Controllers
{
    public class LancamentoProdutosAPIController : ApiController
    {

        public int TotalLancamentos { get; set; }

        public IEnumerable<LancamentoProduto> GetLancamentosEmpresa(int idEmpresa) {
            var RN = new ZRN.LancamentoProdutos.LancamentoProdutos();

            var dtInicial = DateTime.Now.AddYears(-100);
            var dtFinal = DateTime.Now;

            return RN.retornaLancamentosEmpresa(idEmpresa, dtInicial, dtFinal);
        }

        //[System.Web.Http.AcceptVerbs("GET", "POST")]
        //[System.Web.Http.HttpGet]
        //[Route("MethodFruit")]
        public EmpresaLancamento GetLancamentosEmpresa(string filtro)
        {
            var filtroLanc = JsonConvert.DeserializeObject<FiltroLancamentoProdutos>(filtro);
     
            var RN = new ZRN.LancamentoProdutos.LancamentoProdutos();
            var lancamentos = RN.retornaLancamentosEmpresa(filtroLanc);


            var empLancamento = new EmpresaLancamento();
            empLancamento.Lancamentos = lancamentos;
            empLancamento.TotalLancamentos = RN.TotalLancamentos;
            
            //var dtInicial = DateTime.Now.AddYears(-1);
            //var dtFinal = DateTime.Now;

            return empLancamento;
        }

        public ItensFiltroLancamentoProdutos GetFiltrosLancamentos(int idEmpresa) {
            var RN = new ZRN.LancamentoProdutos.LancamentoProdutos();

            var dtInicial = DateTime.Now.AddYears(-100);
            var dtFinal = DateTime.Now;

            return RN.RetornaFiltros(idEmpresa, dtInicial, dtFinal);
        }
    }
}
