using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class Promocoes
    {

        private ZBD.Model.zeengEntities _bd;
        public int TotalAbrangenciaBR;

        public Promocoes()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public int RetornaQtdPromocoesUltimosDozeMeses(int idEmpresa)
        {

            DateTime dtFinal = DateTime.Now;
            var dataFinal = dtFinal.AddDays(1 - dtFinal.Day).Date;
            DateTime dtInicial = dataFinal.AddMonths(-11);
        
            var resultado = (from promocao in _bd.promo_promocoes
                             join empPromo in _bd.promo_promoempresas on promocao.idpromocao equals empPromo.idpromocao
                             where empPromo.idempresa == idEmpresa && promocao.dtcadastro >= dtInicial && promocao.dtcadastro <= dtFinal
                             select promocao).Count();
            
            return resultado;
        }

        public List<ZRN.Promocoes.ItemGraficoSituacoes> RetornaGraficoSituacoesPromocoesDozeMeses(int idEmpresa)
        {

            DateTime dtFinal = DateTime.Now;
            var dataFinal = dtFinal.AddDays(1 - dtFinal.Day).Date;
            DateTime dtInicial = dataFinal.AddMonths(-11);

            string dtFim = dtFinal.ToString("yyyy-MM-dd");
            string dtIni = dtInicial.ToString("yyyy-MM-dd");

            string query = "CALL GetGraficoPerfilSituacoesPromocoes('" + idEmpresa + "', '" + dtIni + "', '" + dtFim + "');";

            List<ZRN.Promocoes.ItemGraficoSituacoes> situacoes = _bd.Database.SqlQuery<ZRN.Promocoes.ItemGraficoSituacoes>(query).ToList();

            return situacoes;

        }
        
        public ZRN.Empresas.DadosAbrangencia RetornaDadosAbrangencia(int idEmpresa, DateTime vigenciaIni, DateTime vigenciaFinal)
        {
            var dados = (
                            from pro in _bd.seae_processos
                            join ep in _bd.seae_empresa_processos on pro.idprocesso equals ep.idprocesso
                            join e in _bd.seae_abrang_estado on ep.idprocesso equals e.idprocesso
                            join es in _bd.estados on e.idestado equals es.idestado
                            where ep.idempresa == idEmpresa &&
                                    pro.dtvigenciaini >= vigenciaIni &&
                                    pro.dtvigenciafim <= vigenciaFinal
                            group es by es.uf into grupoEstados
                            select new ZRN.Promocoes.GraficoAbrangencia()
                            {
                                uf = grupoEstados.Key.ToLower(),
                                total = grupoEstados.Select(x => x.uf).Count()
                            }

                        ).OrderBy(x => x.total).ToList();

            this.TotalAbrangenciaBR = (
                                            from p in _bd.seae_processos
                                            join ep in _bd.seae_empresa_processos on p.idprocesso equals ep.idprocesso
                                            where ep.idempresa == idEmpresa &&
                                                    p.dtvigenciaini >= vigenciaIni &&
                                                    p.dtvigenciafim <= vigenciaFinal &&
                                                    p.abrangencia_nacional == true
                                            select p
                                        ).ToList().Count();

            DadosAbrangencia objDados = new DadosAbrangencia();
            objDados.dados = dados;
            objDados.TotalAbrangenciaNacional = this.TotalAbrangenciaBR;

            return objDados;
        }

        public List<ZRN.Graficos.Linha> RetornaGraficoPromocoesAtivas(ZRN.Promocoes.FiltroPromocoes filtro)
        {

            ////var datainicial = new DateTime(2016, 1, 1);
            ////var datafinal = DateTime.Now;

            ////TimeSpan diferença = datafinal - datainicial;
            ////int quantidadeDias = diferença.Days;

            //TimeSpan diferença = filtro.dataFinal - filtro.dataInicial;
            //int quantidadeDias = diferença.Days;

            //var segundas = new List<DateTime> { };

            //var linhas = new List<ZRN.Graficos.Linha>();

            //for (var x = 0; x <= quantidadeDias; x++)
            //{
            //    //var tempData = datainicial.AddDays(x);
            //    var tempData = filtro.dataInicial.AddDays(x);
            //    switch (tempData.DayOfWeek)
            //    {
            //        case DayOfWeek.Sunday:

            //            linhas.Add(new Graficos.Linha() {
            //                categoria = "Promo",
            //                data = tempData.ToString("yyyy-MM-dd"),
            //                datahora = tempData,
            //                valor = (from p in _bd.seae_empresa_promocoes
            //                         where
            //                            p.seae_promocoes.vigencia_ini <= tempData &&
            //                            p.seae_promocoes.vigencia_fim >= tempData &&
            //                            p.empresas.idempresa == filtro.idEmpresa
            //                          //  &&
            //                          //  (p.seae_promocoes.resumo.ToLower().Contains(filtro.pesquisa.ToLower()) 
            //                          //|| p.seae_promocoes.comoparticipar.ToLower().Contains(filtro.pesquisa.ToLower()) 
            //                          //|| p.seae_promocoes.interessados.ToLower().Contains(filtro.pesquisa.ToLower())
            //                          //|| filtro.pesquisa)
            //                         select p.idpromocao).Count()
            //            });

            //            segundas.Add(tempData);
            //            break;
            //    }

            //}


            ////var promocoes = (from promocao in _bd.seae_promocoes
            ////                 join empresa in _bd.seae_empresa_promocoes on promocao.idpromocao equals empresa.idpromocao
            ////                 where empresa.idempresa == filtro.idEmpresa &&

            ////                       (promocao.vigencia_ini >= filtro.dataInicial && promocao.vigencia_ini <= filtro.dataFinal) ||
            ////                       (promocao.vigencia_fim >= filtro.dataInicial && promocao.vigencia_fim <= filtro.dataFinal) ||
            ////                       (promocao.vigencia_ini <= filtro.dataInicial && promocao.vigencia_fim >= filtro.dataFinal)
            ////                 select promocao).ToList();


            ////var resultado = (from segunda in segundas
            ////                 where promocoes.Where(x => x.vigencia_ini >= segunda).First()
            ////                 select new ZRN.Graficos.Linha()
            ////                 {


            ////                 });




            return null;

        }

    }


    public class DadosAbrangencia
    {
        public List<ZRN.Promocoes.GraficoAbrangencia> dados { get; set; }
        public int TotalAbrangenciaNacional { get; set; }
    }
}
