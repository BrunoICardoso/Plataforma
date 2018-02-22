using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.LancamentoProdutos
{
    public class LancamentoProdutos
    {
        private ZBD.Model.zeengEntities _bd;

        public LancamentoProdutos()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public int TotalLancamentos { get; set; }

        public IEnumerable<LancamentoProduto> retornaLancamentosEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var lancamentos = (from l in _bd.mapa_registros
                               join mapaEmpresa in _bd.mapa_registro_empresa on l.idregistro equals mapaEmpresa.idregistro
                               join empresa in _bd.empresas on mapaEmpresa.idempresa equals empresa.idempresa
                               where
                                    mapaEmpresa.idempresa == idEmpresa &&
                                    l.dataconcessao >= dtInicial.Date &&
                                    l.dataconcessao <= dtFinal.Date &&
                                    l.excluido == false
                               select new LancamentoProduto()
                               {
                                   dataconcessao = l.dataconcessao,
                                   idarea = l.idarea,
                                   nomeArea = l.mapa_areas.nome == null ? "Não especificado" : l.mapa_areas.nome,
                                   idatributo = l.idatributo,
                                   nomeAtributo = l.mapa_atributo.nome == null ? "Não especificado" : l.mapa_atributo.nome,
                                   idbase = l.idbase,
                                   nomeBase = l.mapa_base.nome == null ? "Não especificado" : l.mapa_base.nome,
                                   idcaracteristica = l.idcaracteristica,
                                   nomeCaracteristica = l.mapa_caracteristica.nome == null ? "Não especificado" : l.mapa_caracteristica.nome,
                                   idcomplemento = l.idcomplemento,
                                   nomeComplemento = l.mapa_complemento.nome == null ? "Não especificado" : l.mapa_complemento.nome,
                                   idempresa = mapaEmpresa.idempresa,
                                   nomeEmpresa = empresa.nome,
                                   idespecie = l.idespecie,
                                   nomeEspecie = l.mapa_especies.nome == null ? "Não especificado" : l.mapa_especies.nome,
                                   idestado = l.idestado,
                                   nomeEstado = l.estados.uf == null ? "Não especificado" : l.estados.uf,
                                   nomeMarca = l.nomeMarca,   
                                   idorigem = l.idorigem,
                                   nomeOrigem = l.mapa_origens.nome == null ? "Não especificado" : l.mapa_origens.nome,
                                   idregistro = l.idregistro,
                                   numregistro = l.numregistro == null ? "Não informado" : l.numregistro,
                                   idsubespecie = l.idsubespecie,
                                   nomeSubEspecie = l.mapa_subespecie.nome == null ? "Não especificado" : l.mapa_subespecie.nome,
                                   modoaplicacao = l.modoaplicacao,
                                   nomeProduto = l.nomeProduto,
                                   Status = l.status
                               });
            return lancamentos;            
        }

        public List<LancamentoProduto> retornaLancamentosEmpresa(FiltroLancamentoProdutos filtro)
        {

            var lancamentos = (from l in _bd.mapa_registros
                               join mapaEmpresa in _bd.mapa_registro_empresa on l.idregistro equals mapaEmpresa.idregistro
                               join empresa in _bd.empresas on mapaEmpresa.idempresa equals empresa.idempresa
                               where

                                    mapaEmpresa.idempresa == filtro.idEmpresa &&
                                    (filtro.dataInicial.HasValue == false || l.dataconcessao >= filtro.dataInicial.Value) &&
                                    (filtro.dataFinal.HasValue == false || l.dataconcessao <= filtro.dataFinal.Value) &&

                                    (string.IsNullOrEmpty(filtro.NomeMarca) || l.nomeMarca.Contains(filtro.NomeMarca)) &&
                                    (l.nomeProduto.ToLower() == filtro.NomeProduto.ToLower() || filtro.NomeProduto == null || filtro.NomeProduto == "")
                                    //|| l.marcas.nome.Contains(filtro.NomeMarca)) 
                                    &&
                                    (string.IsNullOrEmpty(filtro.NomeProduto) || l.nomeProduto.Contains(filtro.NomeProduto))
                                    &&
                                    l.excluido == false
                               orderby l.dataconcessao descending
                               select new LancamentoProduto()
                               {
                                   dataconcessao = l.dataconcessao,
                                   idarea = l.idarea,
                                   nomeArea = l.mapa_areas.nome == null ? "Não especificado" : l.mapa_areas.nome,
                                   idatributo = l.idatributo,
                                   nomeAtributo = l.mapa_atributo.nome == null ? "Não especificado" : l.mapa_atributo.nome,
                                   idbase = l.idbase,
                                   nomeBase = l.mapa_base.nome == null ? "Não especificado" : l.mapa_base.nome,
                                   idcaracteristica = l.idcaracteristica,
                                   nomeCaracteristica = l.mapa_caracteristica.nome == null ? "Não especificado" : l.mapa_caracteristica.nome,
                                   idcomplemento = l.idcomplemento,
                                   nomeComplemento = l.mapa_complemento.nome == null ? "Não especificado" : l.mapa_complemento.nome,
                                   idempresa = mapaEmpresa.idempresa,
                                   nomeEmpresa = empresa.nome,
                                   idespecie = l.idespecie,
                                   nomeEspecie = l.mapa_especies.nome == null ? "Não especificado" : l.mapa_especies.nome,
                                   idestado = l.idestado,
                                   nomeEstado = l.estados.uf == null ? "Não especificado" : l.estados.uf,
                                   nomeMarca = l.nomeMarca,
                                   idorigem = l.idorigem,
                                   nomeOrigem = l.mapa_origens.nome == null ? "Não especificado" : l.mapa_origens.nome,
                                   idregistro = l.idregistro,
                                   numregistro = l.numregistro == null ? "Não especificado" : l.numregistro,
                                   idsubespecie = l.idsubespecie,
                                   nomeSubEspecie = l.mapa_subespecie.nome == null ? "Não especificado" : l.mapa_subespecie.nome,
                                   modoaplicacao = l.modoaplicacao,
                                   nomeProduto = l.nomeProduto,
                                   Status = l.status

                               });

            TotalLancamentos = lancamentos.Count();

            var lancs = lancamentos.Skip(filtro.regPorPagina * (filtro.pagina - 1)).Take(filtro.regPorPagina).ToList();
            return lancs;
            
        }

        public ItensFiltroLancamentoProdutos RetornaFiltros(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var filtro = new ItensFiltroLancamentoProdutos();

            var anos = (from r in _bd.mapa_registros
                        join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                        where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal
                        group r by r.dataconcessao.Value.Year into grupoAno
                        orderby grupoAno.Key descending
                        select new
                        {
                            ano = grupoAno.Key
                        }).ToList();

            filtro.Anos = (from a in anos
                           orderby a.ano ascending
                           select new ItemFiltro()
                           {
                               texto = a.ano.ToString(),
                               valor = a.ano.ToString()
                           }).ToList();

            filtro.Areas = (from r in _bd.mapa_registros
                            join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                            where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_areas.excluido == false
                            group r by r.mapa_areas.nome into grupo
                            orderby grupo.Key ascending
                            select new ItemFiltro()
                            {
                                texto = grupo.Key,
                                valor = grupo.Key
                            }).ToList();

            filtro.Atributos = (from r in _bd.mapa_registros
                                join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                                where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                                group r by r.mapa_atributo.nome into grupo
                                orderby grupo.Key ascending
                                select new ItemFiltro()
                                {
                                    texto = grupo.Key,
                                    valor = grupo.Key
                                }).ToList();

            filtro.Bases = (from r in _bd.mapa_registros
                            join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                            where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                            group r by r.mapa_base.nome into grupo
                            orderby grupo.Key ascending
                            select new ItemFiltro()
                            {
                                texto = grupo.Key,
                                valor = grupo.Key
                            }).ToList();

            filtro.Caracteristicas = (from r in _bd.mapa_registros
                                      join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                                      where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                                      group r by r.mapa_caracteristica.nome into grupo
                                      orderby grupo.Key ascending
                                      select new ItemFiltro()
                                      {
                                          texto = grupo.Key,
                                          valor = grupo.Key
                                      }).ToList();

            filtro.Complementos = (from r in _bd.mapa_registros
                                   join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                                   where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                                   group r by r.mapa_complemento.nome into grupo
                                   orderby grupo.Key ascending
                                   select new ItemFiltro()
                                   {
                                       texto = grupo.Key,
                                       valor = grupo.Key
                                   }).ToList();

            filtro.Especies = (from r in _bd.mapa_registros
                               join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                               where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                               group r by r.mapa_especies.nome into grupo
                               orderby grupo.Key ascending
                               select new ItemFiltro()
                               {
                                   texto = grupo.Key,
                                   valor = grupo.Key
                               }).ToList();



            filtro.Estados = (from e in _bd.estados
                              where e.excluido == false
                              orderby e.uf ascending
                              select new ItemFiltro()
                              {
                                  texto = e.uf,
                                  valor = e.uf
                              }).ToList();


            //var marcas = (from r in _bd.mapa_registros
            //              join m in _bd.marcas on r.idmarca equals m.idmarca into joinmarca
            //              from marca in joinmarca.DefaultIfEmpty()
            //              where r.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false


            //              select new
            //              {
            //                  nomeMarca = (marca == null ? r.nomeMarca : marca.nome)

            //              }).GroupBy(x => x.nomeMarca).Select(z => z.Key).ToList();

            //var marcasRegistro = (from marca in _bd.mapa_registros
            //                      where marca.nomeMarca != null || marca.nomeMarca != ""
            //                      select new
            //                      {
            //                          nomeMarca = marca.nomeMarca

            //                      }).GroupBy(x => x.nomeMarca).Select(z => z.Key).ToList();

            var marcasRegistro = (from mpr in _bd.mapa_registros
                                  join mprempresa in _bd.mapa_registro_empresa on mpr.idregistro equals mprempresa.idregistro
                                  where mprempresa.idempresa == idEmpresa
                                  group mpr by mpr.nomeMarca into resultado
                                  select resultado.Key).ToList();
            
            filtro.Marcas = (from e in marcasRegistro
                             orderby e ascending
                             select new ItemFiltro()
                             {
                                 texto = e,
                                 valor = e
                             }).ToList();


            filtro.Produtos = (from mpr in _bd.mapa_registros
                               join mprempresa in _bd.mapa_registro_empresa on mpr.idregistro equals mprempresa.idregistro
                               where mprempresa.idempresa == idEmpresa
                               select new ItemFiltro() {
                                   texto = mpr.nomeProduto,
                                   valor = mpr.nomeProduto
                               }).ToList();



            filtro.Origens = (from r in _bd.mapa_registros
                              join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                              where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                              group r by r.mapa_origens.nome into grupo
                              orderby grupo.Key ascending
                              select new ItemFiltro()
                              {
                                  texto = grupo.Key,
                                  valor = grupo.Key
                              }).ToList();


            filtro.Status = (from r in _bd.mapa_registros
                             join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                             where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                             group r by r.status into grupo
                             orderby grupo.Key ascending
                             select new ItemFiltro()
                             {
                                 texto = grupo.Key,
                                 valor = grupo.Key
                             }).ToList();


            filtro.Subespecies = (from r in _bd.mapa_registros
                                  join e in _bd.mapa_registro_empresa on r.idregistro equals e.idregistro
                                  where e.idempresa == idEmpresa && r.dataconcessao >= dtInicial && r.dataconcessao <= dtFinal && r.mapa_caracteristica.excluido == false
                                  group r by r.mapa_subespecie.nome into grupo
                                  orderby grupo.Key ascending
                                  select new ItemFiltro()
                                  {
                                      texto = grupo.Key,
                                      valor = grupo.Key
                                  }).ToList();



            return filtro;

        }

        

    }
}
