using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class PromocoesElastic
    {

        string _server = "";
        string _indexElastic = "";

        public PromocoesElastic(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        public List<ZRN.Promocoes.ItemGraficoSituacoes> RetornaItensGraficoSituacoes(ZRN.Promocoes.FiltroProcessoSeae filtro)
        {
            
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            var response = client.Search<ZRN.Promocoes.Processos_Seae>(s => s
                            .Type("processos_seae")
                            .Query(q =>
                                     ( filtro!= null && filtro.idEmpresa != 0 ? q.Term("empresas.idempresa", filtro.idEmpresa) : null) &&

                                       (q.QueryString(mm => mm
                                           .Fields(f => f
                                               .Field("comoparticipar")
                                               .Field("interessados")
                                               .Field("modalidade")
                                               .Field("nome")
                                               .Field("premios")
                                               .Field("situacaoatual")
                                               .Field("solicitantes")
                                               .Field("arquivos.textoarquivo")
                                           )
                                           .Query((filtro != null ? filtro.pesquisa : ""))
                                           .DefaultOperator(Operator.And)
                                         ))

                                         &&

                                       (   filtro != null ? q.Terms(t => t.Field(f => f.situacaoatual).Terms(filtro.situacoes)) : null ) &&
                                       (
                                           filtro != null ?
                                               q.DateRange(d => d
                                                .Field(f => f.dtprocesso)
                                                .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataInicial))
                                                .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFinal))
                                                )
                                                : null
                                        )
                                    )                                    
                            .Aggregations(x => x.Terms("group_by_situacaoatual", gp => gp.Field(ff => ff.situacaoatual.Suffix("raw"))))
                                    );

            var resultadoGrupBySituacaoatual = response.Aggs.Terms("group_by_situacaoatual").Buckets;

            var resultado = (from item in resultadoGrupBySituacaoatual
                             select new ZRN.Promocoes.ItemGraficoSituacoes()
                             {
                                 situacao = item.Key,
                                 total = Convert.ToInt32(item.DocCount)
                             }).ToList();
            
            return resultado;
        }
               
        public List<ZRN.Graficos.Linha> RetornaGraficoPromocoesAtivasSemana(ZRN.Promocoes.FiltroProcessoSeae filtro)
        {
                       
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            var response = client.Search<ZRN.Promocoes.Processos_Seae>(s => s
                            .Type("processos_seae")
                            .Query(q =>
                                       (filtro.idEmpresa != 0 ? q.Term("empresas.idempresa", filtro.idEmpresa) : null) &&

                                       (q.QueryString(m => m
                                           .Fields(f => f
                                               .Field("comoparticipar")
                                               .Field("interessados")
                                               .Field("modalidade")
                                               .Field("nome")
                                               .Field("premios")
                                               .Field("situacaoatual")
                                               .Field("solicitantes")
                                               .Field("arquivos.textoarquivo")
                                           )
                                           .Query((filtro != null ? filtro.pesquisa : ""))
                                           .DefaultOperator(Operator.And)
                                         ))
                                         
                                         &&
                                       (q.DateRange(d => d
                                        .Field(f => f.dtprocesso)
                                        .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataInicial))
                                        .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFinal))
                                        )))
                            .Aggregations(a => a
                                    .DateHistogram("processos_semana", hs => hs.Field(f => f.dtprocesso)
                                    .Interval(DateInterval.Week)
                                    ))
                            );


            var resultado = response.Aggs.DateHistogram("processos_semana").Buckets.ToList();
            
            var primeiraSegunda = filtro.dataInicial.AddDays(1 - Convert.ToInt32(filtro.dataInicial.DayOfWeek)).Date;
            var linhas = new List<ZRN.Graficos.Linha>();
            var i = 0;

            while (primeiraSegunda < filtro.dataFinal)
            {

                var totalItens = 0;
                if (resultado.Count > i && resultado[i].Date == primeiraSegunda)
                {
                    totalItens = Convert.ToInt32(resultado[i].DocCount);
                    i++;
                }

                var item = new Graficos.Linha()
                {
                    categoria = "Processos",
                    data = primeiraSegunda.ToString("yyyy-MM-dd"),
                    valor = totalItens
                };

                linhas.Add(item);

                primeiraSegunda = primeiraSegunda.AddDays(7);
            };
            
            return linhas;


        }
       
    }
}
