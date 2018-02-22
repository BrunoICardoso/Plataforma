using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Vertentes;

namespace ZRN.Clientes
{
    public class Clientes
    {
        ZBD.Model.zeengEntities db;
        public int totalEmpresa;

        public Clientes()
        {
            db = new ZBD.Model.zeengEntities();
        }

        public bool VerificaEmpresasDoCliente(int idCliente)
        {

            if (AcessoTodasEmpresas(idCliente) == true)
            {

                return true;

            }

            var temClientes = db.cliente_empresas.Where(x => x.idcliente == idCliente && x.excluido == false).FirstOrDefault();
            return temClientes != null ? true : false;
        }

        public bool VerificaAcessoEmpresaVertente(enumVertentes vertente, int idempresa, int idcliente)
        {
            var acessoEmpresa = VerificaAcessoEmpresa(idempresa, idcliente);

            if (!acessoEmpresa)
                return false;

            var acessoVertente = VerificaAcessoVertente(vertente, idcliente);
            if (!acessoVertente)
                return false;

            return acessoVertente && acessoEmpresa;

        }

        public bool VerificaAcessoVertente(enumVertentes vertente, int idcliente)
        {

            var vert = RetornaVertentesAssociada(idcliente);

            switch (vertente)
            {
                case enumVertentes.RedesSociais:
                    return vert.redessociais;

                case enumVertentes.Noticias:
                    return vert.noticias;

                case enumVertentes.Produtos:
                    return vert.produtos;

                case enumVertentes.PresencaOnline:
                    return vert.presencaonline;

                case enumVertentes.Promocoes:
                    return vert.promocoes;

                default:
                    return false;

            }
        }

        public bool VerificaAcessoEmpresa(int idempresa, int idcliente)
        {


            if (AcessoTodasEmpresas(idcliente) == true)
            {

                return true;

            }



            var temAcesso = (from e in db.cliente_empresas
                             where
                             e.idcliente == idcliente &&
                             e.idempresa == idempresa &&
                             e.excluido == false
                             select e).Count() > 0;


            return temAcesso;
        }

        public bool AcessoTodasEmpresas(int idcliente)
        {

            var acessoTodasEMpresas = db.clientes.Where(w => w.idclientes == idcliente && w.vertodasempresas == true && w.excluido == false).Select(s => s).Count() > 0;

            return acessoTodasEMpresas;

        }

        public ClienteVertentes RetornaVertentesAssociada(int idcliente)
        {

            var cv = (from v in db.cliente_vertentes
                      where v.idcliente == idcliente
                      select new ClienteVertentes
                      {
                          noticias = v.noticias.Value,
                          presencaonline = v.presencaonline.Value,
                          promocoes = v.promocoes.Value,
                          produtos = v.produtos.Value,
                          redessociais = v.redessociais.Value
                      }).First();


            return cv;

        }

        public List<Listas.Item> RetornaSetores()
        {
            var setores = (from s in db.setoresempresa
                           select new Listas.Item()
                           {
                               id = s.idsetoresempresa,
                               nome = s.nome
                           }
                        );
            return setores.ToList();
        }

        public List<Empresas.Empresa> RetornaTodasEmpresas(int paginaAtual, int qtdeRegistros, int idCliente, int idSetor, string expressao)
        {
            //paginaAtual = paginaAtual > 0 ? (paginaAtual - 1) : paginaAtual;

            var emnpresas = (from e in db.empresas
                             join s in db.setoresempresa on e.idsetor equals s.idsetoresempresa
                             where e.excluido == false &&
                             (expressao != null && expressao != "" ? e.nome.Contains(expressao) : e.nome != null) &&
                             (idSetor != 0 ? e.idsetor == idSetor : e.idsetor != 0)
                             orderby e.nome ascending
                             select new Empresas.Empresa
                             {
                                 idempresa = e.idempresa,
                                 nome = e.nome,
                                 imagem = e.imagem,
                                 nomeSetor = s.nome,
                                 urlsite = e.urlsite,
                                 clienteAssociado = false
                             }
                );

            var lista = emnpresas.Skip(qtdeRegistros * (paginaAtual - 1)).Take(qtdeRegistros).ToList();

            var listaEmpresasReturn = new List<Empresas.Empresa>();
            foreach (var emp in lista)
            {
                var existeAssociacao = db.cliente_empresas.Where(x => x.idcliente == idCliente && x.idempresa == emp.idempresa && x.excluido == false).FirstOrDefault();

                var objEmpresa = new Empresas.Empresa();
                objEmpresa.idempresa = emp.idempresa;
                objEmpresa.nome = emp.nome;
                objEmpresa.imagem = emp.imagem;
                objEmpresa.nomeSetor = emp.nomeSetor;
                objEmpresa.urlsite = emp.urlsite;
                objEmpresa.clienteAssociado = existeAssociacao != null ? true : false;

                listaEmpresasReturn.Add(objEmpresa);
            }

            return listaEmpresasReturn;

        }

        public List<Empresas.Empresa> RetornaTodasEmpresasAssociadas(int idCliente)
        {
            var emnpresas = (from e in db.empresas
                             join s in db.setoresempresa on e.idsetor equals s.idsetoresempresa
                             join ce in db.cliente_empresas on e.idempresa equals ce.idempresa
                             where ce.idcliente == idCliente && ce.excluido == false && e.excluido == false
                             orderby e.nome ascending
                             select new Empresas.Empresa
                             {
                                 idempresa = e.idempresa,
                                 nome = e.nome,
                                 imagem = e.imagem,
                                 nomeSetor = s.nome,
                                 urlsite = e.urlsite,
                                 clienteAssociado = false
                             }
                );
            var retorno = emnpresas.ToList();

            return retorno;
        }

        public Cliente RetornaCliente(int idCliente)
        {
            return (from c in db.clientes
                    where c.idclientes == idCliente
                    select new Cliente()
                    {
                        idcliente = c.idclientes,
                        nome = c.nome,
                        qtdempresas = (c.qtdempresas != null ? c.qtdempresas : 0)
                    }).FirstOrDefault();
        }

        public List<ZRN.Empresas.Empresa> RetornaPesquisa(ZRN.Clientes.Filtros filtros)
        {
            var emnpresas = (from e in db.empresas
                                 join s in db.setoresempresa on e.idsetor equals s.idsetoresempresa
                                 orderby e.nome ascending
                             where !e.excluido &&
                             (
                             filtros.idSetor != 0 ? e.idsetor == filtros.idSetor : e.idsetor != 0
                             )
                             &&
                             (
                             e.nome.ToLower() == filtros.expressao.ToLower() ||
                             e.nome.Contains(filtros.expressao.ToLower()) ||
                             filtros.expressao == "" ||
                             filtros.expressao == null
                             )

                   

                             select new Empresas.Empresa()
                             {
                                 idempresa = e.idempresa,
                                 nome = e.nome,
                                 imagem = e.imagem,
                                 nomeSetor = s.nome,
                                 urlsite = e.urlsite,
                                 clienteAssociado = false
                             }
                );
            totalEmpresa = emnpresas.ToList().Count;

            var listaEmpresas = emnpresas.Skip(filtros.qtdregistros * (filtros.pagina -1)).Take(filtros.qtdregistros).ToList();

            var listaEmpresasReturn = new List<Empresas.Empresa>();
            foreach (var emp in listaEmpresas)
            {
                var existeAssociacao = db.cliente_empresas.Where(x => x.idcliente == filtros.idCliente && x.idempresa == emp.idempresa && x.excluido == false).FirstOrDefault();

                var objEmpresa = new Empresas.Empresa();
                objEmpresa.idempresa = emp.idempresa;
                objEmpresa.nome = emp.nome;
                objEmpresa.imagem = emp.imagem;
                objEmpresa.nomeSetor = emp.nomeSetor;
                objEmpresa.urlsite = emp.urlsite;
                objEmpresa.clienteAssociado = existeAssociacao != null ? true : false;

                listaEmpresasReturn.Add(objEmpresa);
            }

            return listaEmpresasReturn;
        }

        public bool AssociarEmpresaCliente(int idCliente, int idEmpresa)
        {
            try
            {
                var jaExisteAsociacao = db.cliente_empresas.Where(x => x.idcliente == idCliente && x.idempresa == idEmpresa && x.excluido == false).FirstOrDefault();
                if (jaExisteAsociacao == null)
                {
                    var associa = new ZBD.Model.cliente_empresas();
                    associa.idcliente = idCliente;
                    associa.idempresa = idEmpresa;
                    associa.dtcadastro = DateTime.Now;
                    associa.excluido = false;

                    db.cliente_empresas.Add(associa);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DesassociarEmpresaCliente(int idCliente, int idEmpresa)
        {

            try
            {
                var jaExisteAsociacao = db.cliente_empresas.Where(x => x.idcliente == idCliente && x.idempresa == idEmpresa && x.excluido == false).FirstOrDefault();
                if (jaExisteAsociacao != null)
                {
                    //db.cliente_empresas.Attach(jaExisteAsociacao);
                    //db.cliente_empresas.Remove(jaExisteAsociacao);
                    jaExisteAsociacao.excluido = true;
                    //db.cliente_empresas.Add(jaExisteAsociacao);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int VerificarLimiteEmpresasCliente(int idCliente)
        {
            int qtdEmpresas = 0;                
            var obj = db.clientes.Where(x => x.idclientes == idCliente ).FirstOrDefault();
            qtdEmpresas = obj.qtdempresas != null ? obj.qtdempresas.Value : 0;

            return qtdEmpresas;
        }

        public int TotalEmpresasAtivas()
        {
            return db.empresas.Where(x => x.excluido == false).ToList().Count;
        }

        public bool VerificarDataAcessoTrial(int idcliente)
        {

            var acessoTrial = (from c in db.clientes
                               where c.idclientes == idcliente && c.excluido == false

                               select new
                               {
                                   data = c.dtexpiracaolicenca

                               }).First();

            if (acessoTrial.data == null)
            {

                return false ;

            }

            if (acessoTrial.data >= DateTime.Now.Date)
            {

                return false;

            }
            else {

                return true;
            }

           

        }

        public AcessoTrial retornarUsuarioCliente(int idusuario)
        {

            var ClienteUsuario = (from c in db.clientes
                                  join u in db.usuarios on c.idclientes equals u.idcliente
                                  where u.idusuario == idusuario && u.excluido == false
                                  select new AcessoTrial
                                  {

                                      cliente = new Cliente()
                                      {
                                          idcliente = c.idclientes,
                                          nome = c.nome
                                      },

                                      usuario = new Usuarios.Usuario()
                                      {
                                          idUsuario = u.idusuario,
                                          nome = u.nome,
                                          email = u.email
                                      }

                                  }).First();

            return ClienteUsuario;
        }

    }

    public class Lista_TotalEmpresas
    {
        public List<ZRN.Empresas.Empresa> empresas { get; set; }
        public int total { get; set; }
    }
}
