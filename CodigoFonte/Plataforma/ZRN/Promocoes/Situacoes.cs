using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class Situacoes
    {

        private ZBD.Model.zeengEntities _bd;

        public Situacoes()
        {
            _bd = new ZBD.Model.zeengEntities();
        }


        public IEnumerable<Situacao> RetornaSituacoes()
        {
            return (
                    from s in _bd.seae_situacoes
                    orderby s.descricao ascending
                    select new Situacao()
                    {
                        idsituacao = s.idsituacao,
                        descricao = s.descricao
                    }
                    ).ToList();
        }


        public List<ItemGraficoSituacoes> RetornaItensGraficoSituacoes(FiltroPromocoes filtro)
        {

            //string listaIdsSituacao;

            //if (filtro.pesquisa == null)
            //{
            //    filtro.pesquisa = "";
            //}

            //string dataIni = filtro.dataInicial.ToString("yyyy-MM-dd");
            //string dataFim = filtro.dataFinal.ToString("yyyy-MM-dd");

            //List<int> listaIds = new List<int>();

            //if (filtro.situacoes != null) { 
            //    foreach (var e in filtro.situacoes)
            //    {
            //        listaIds.Add(e);
            //    }

            //    listaIdsSituacao = String.Join(",", listaIds);
            //}
            //else
            //{
            //    listaIdsSituacao = "0";
            //}

            //string query = "CALL GetGraficoSituacoes(" + filtro.idEmpresa + ", '" + listaIdsSituacao + "', '" + filtro.pesquisa + "', '" + dataIni + "', '" + dataFim + "');";

            //         List<ItemGraficoSituacoes> situacoes = _bd.Database.SqlQuery<ItemGraficoSituacoes>(query).ToList();
            
            //return situacoes;

            return null;
        }



    }
}
