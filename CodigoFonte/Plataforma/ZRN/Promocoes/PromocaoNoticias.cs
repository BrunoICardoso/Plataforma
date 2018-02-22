using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ZRN.Promocoes
{
    public class PromocaoNoticias
    {
        private string _servidorElastic;
        private string _indiceElastic;

        public PromocaoNoticias(string servidorElastic, string indiceElastic)
        {
            _servidorElastic = servidorElastic;
            _indiceElastic = indiceElastic;
        }

        public List<ZRN.PromocoesElastic.Noticia> RetornaNoticias(ZRN.Promocoes.FiltroPromocaoNoticia filtro)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "noticias");
            var client = new ElasticClient(senttings);

            var resp = client.Search<ZRN.PromocoesElastic.Noticia>(s => s
                                   .Type("noticias")
                                   .Query(q =>
                                               q.Term("promocoes.idpromocao", filtro.idPromocao) &&
                                               q.Term("empresas.idempresa", filtro.idEmpresa)
                                               &&
                                               (
                                                   filtro.expressao != null && filtro.expressao != "" ?
                                                       q.MultiMatch(m => m
                                                                .Fields(f => f.Field("conteudo")
                                                                )
                                                                .Query(filtro.expressao)
                                                                .Operator(Operator.And)
                                                ) : null
                                            )
                                           )
                                       );            

            return resp.Documents.OrderByDescending(x => x.datapublicacao).Skip(filtro.pagina).Take(filtro.qtdeRegistros).ToList();
        }

        public int RetornaTotalNoticias(ZRN.Promocoes.FiltroPromocaoNoticia filtro)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "noticias");
            var client = new ElasticClient(senttings);

            var resp = client.Search<ZRN.PromocoesElastic.Noticia>(s => s
                                   .Type("noticias")
                                   .Query(q =>
                                               q.Term("promocoes.idpromocao", filtro.idPromocao) &&
                                               q.Term("empresas.idempresa", filtro.idEmpresa)
                                               &&
                                               (
                                                   filtro.expressao != null && filtro.expressao != "" ?
                                                       q.MultiMatch(m => m
                                                                .Fields(f => f.Field("conteudo")
                                                                )
                                                                .Query(filtro.expressao)
                                                                .Operator(Operator.And)
                                                ) : null
                                            )
                                           )
                                       );            
            return Convert.ToInt32(resp.Total);
        }
    }
}
