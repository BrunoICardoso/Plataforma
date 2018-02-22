using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ZeengWeb.Controllers
{
    public class ClienteAPIController : ApiController
    {
        public List<ZRN.Listas.Item> GetSetores()
        {
            var setoresRN = new ZRN.Clientes.Clientes();
            return setoresRN.RetornaSetores();
        }

        public List<ZRN.Empresas.Empresa> GetTodasEmpresas(int paginaAtual, int qtdeRegistros, int idCliente, int idSetor, string expressao = null)
        {
            var empresasRN = new ZRN.Clientes.Clientes();
            return empresasRN.RetornaTodasEmpresas(paginaAtual, qtdeRegistros, idCliente,idSetor, expressao);
        }

        public List<ZRN.Empresas.Empresa> GetTodasEmpresasAssociadas(int idCliente)
        {
            var empresasRN = new ZRN.Clientes.Clientes();
            return empresasRN.RetornaTodasEmpresasAssociadas(idCliente);
        }

        public ZRN.Clientes.Lista_TotalEmpresas Pesquisa(ZRN.Clientes.Filtros filtros)
        {
            var pesquisaRN = new ZRN.Clientes.Clientes();
            var listaEmpresas = pesquisaRN.RetornaPesquisa(filtros);

            var lista = new ZRN.Clientes.Lista_TotalEmpresas();
            lista.empresas = listaEmpresas;
            lista.total = pesquisaRN.totalEmpresa;

            return lista;
        }

        [HttpGet]
        public bool AssociarEmpresaCliente(int idCliente, int idEmpresa)
        {
            var associaRN = new ZRN.Clientes.Clientes();
            return associaRN.AssociarEmpresaCliente(idCliente, idEmpresa);
        }

        [HttpGet]
        public bool DesassociarEmpresaCliente(int idCliente, int idEmpresa)
        {
            var desassociaRN = new ZRN.Clientes.Clientes();
            return desassociaRN.DesassociarEmpresaCliente(idCliente, idEmpresa);
        }

        [HttpGet]
        public int VerificarLimiteEmpresasCliente(int idCliente)
        {
            var verificaRN = new ZRN.Clientes.Clientes();
            return verificaRN.VerificarLimiteEmpresasCliente(idCliente);
        }

    }
}