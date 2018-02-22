using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class Processos
    {
        public string _server = "";
        private string _indexElastic = "";

        public int TotalRegistros = 0;
        
        public Processos(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }


        public IEnumerable<Processos_Seae> RetornaProcessosTimeLine (FiltroProcessoSeae filtro) {

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");
            
            var client = new ElasticClient(senttings);

            //if (filtro.dataInicial == null)
            //{
            //    filtro.dataInicial = new DateTime(1900, 01, 01);
            //}

            //if (filtro.dataFinal == null)
            //{
            //    filtro.dataFinal = new DateTime(2200, 01, 01);
            //}

            //var response = client.Search<Processos_Seae>(x=>x.Type("processos_seae").Size(10));
            var response = client.Search<Processos_Seae>(s =>                                    
                                    s.Query(q =>
                                    (q.MultiMatch(m => m.
                                        Fields(f => f
                                                .Field("numprocesso")
                                                .Field("comoparticipar")
                                                .Field("interessados")
                                                .Field("modalidade")
                                                .Field("nome")
                                                .Field("premios")
                                                .Field("situacaoatual")
                                                .Field("solicitantes")
                                                .Field("numprocesso")

                                                ).Query((filtro != null ? filtro.pesquisa : ""))
                                                 .Operator(Operator.Or)
                                            )
                                        )
                                        &&
                                        (filtro != null && filtro.idEmpresa != 0 ? q.Term("empresas.idempresa", filtro.idEmpresa) : null) &&

                                              (
                                               filtro != null ?

                                                 q.DateRange(d => d.
                                                    Field(f => f.dtprocesso)
                                                    .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataInicial))
                                                    .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFinal))
                                                )
                                                : null
                                            )
                                        )
                                        //.Size(2000)
                                        .Sort(x => x.Ascending(c => c.dtprocesso)));

            this.TotalRegistros = Convert.ToInt32(response.Total);
            return response.Documents.ToList().Skip(filtro.pag).Take(filtro.quantidade);
        }

        public int RetornaTotalProcessosTimeLine()
        {
            return this.TotalRegistros;
        }

        public int RetornaTotalProcessosTimeLine2(FiltroPromocoes filtro) {

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);

            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<Processos_Seae>(x => x.Type("processos_seae").Size(2000));

            var total = response.Documents.Select(x => x.idprocesso).Count();

            return total;

        }

        public List<DadosAbrangencia> RetornaDadosAbrangencia(FiltroProcessoSeae filtro)
        {           

            var node = new Uri(_server);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(senttings);

            var response = client.Search<Processos_Seae>(s =>
                                    s.Query(q =>
                                    (q.MultiMatch(m => m.
                                        Fields(f => f
                                                .Field("numprocesso")
                                                .Field("comoparticipar")
                                                .Field("interessados")
                                                .Field("modalidade")
                                                .Field("nome")
                                                .Field("premios")
                                                .Field("situacaoatual")
                                                .Field("solicitantes")
                                                .Field("numprocesso")

                                                ).Query( ( filtro != null ? filtro.pesquisa : "" ) )
                                                 .Operator(Operator.Or)
                                            )
                                        )
                                        &&

                                        ( filtro != null && filtro.idEmpresa != 0 && filtro.idEmpresa != null ? q.Term("empresas.idempresa", filtro.idEmpresa) : null ) &&
                                        
                                        (
                                        filtro != null ?
                                            q.DateRange(d => d.
                                                Field(f => f.dtprocesso)
                                                .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataInicial))
                                                .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFinal))
                                            )
                                            : null

                                        )

                                        ));
            
            var estadosGraf = (from d in response.Documents
                               select d into e
                               from c in e.abrangestados
                               select new
                               {                                                                      
                                   c.uf
                               }).GroupBy(x => x.uf).Select(x => new DadosAbrangencia  { local = x.Key, total = x.Count() } ).ToList();



            var abragenciaTotal = (from d in response.Documents
                                   where d.abrangencia_nacional == true
                                   select d
                                   ).Count();

            //if(estadosGraf.Count > 0)
            //  estadosGraf.Add(new DadosAbrangencia() {local = "Nacional", total = abragenciaTotal });

            List<DadosAbrangencia> Dados = new List<DadosAbrangencia>();

            Dados.Add(new DadosAbrangencia() {local = "Nacional", total = abragenciaTotal });

            if (estadosGraf.Count > 0)
                Dados.AddRange(estadosGraf);

            return Dados;
        }
    }

    public class DadosAbrangencia
    {
        public string local { get; set; }
        public int total { get; set; }
    }

}
