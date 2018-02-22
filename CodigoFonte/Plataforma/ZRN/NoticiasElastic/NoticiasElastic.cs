using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.NoticiasElastic
{
    public class NoticiasElastic
    {

        string _server = "";
        string _indexElastic = "";

        public NoticiasElastic(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        //,List<int> fontes
        public List<NoticiaElastic> Pesquisar(DateTime dtInicial, DateTime dtFinal, string titulo, string conteudo, List<int> listaFontes)
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var response = client.Search<NoticiaElastic>(s => s
                            .Query(q =>
                                    (q.Term(p => p.conteudo, conteudo) ||
                                     q.Term(p => p.titulo, titulo)) &&
                                     q.Terms(p => p.Field(f => f.idfonte).Terms(listaFontes)) &&
                                     q.DateRange(d => d
                                        .Field(f => f.datapublicacao)
                                        .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dtInicial))
                                        .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", dtFinal))
                                        )));

            var resultado = response.Documents.ToList<NoticiaElastic>();

            return response.Documents.ToList<NoticiaElastic>();
        }
                
        public NoticiaElastic GetNoticia(int idNoticia)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var response = client.Search<NoticiaElastic>(s => s
                            .Query(q => q.Term(p => p.idnoticia, idNoticia))
                            );

            return response.Documents.First();

        }

        public NoticiaElastic GetNoticiaPromocao(int idNoticia)
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            var response = client.Search<ZRN.Promocoes.Promocao>(s => s
                            .Type("promocao")
                            .Query(q => q.Term("noticias.idnoticia", idNoticia))
                            );

            var noticia = new NoticiaElastic();
            foreach (var n in response.Documents.ToList())
            {
                noticia = n.Noticias.Where(v => v.idnoticia == idNoticia).FirstOrDefault();
            }

            return noticia;

        }

        public NoticiaElastic RetornaNoticiaEmpresa(int idNoticiaEmpresa, string textoPesquisado = "")
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            var response = client.Search<NoticiaElastic>(s => s
                             .Query(q =>
                                        q.Bool(b => b.Must(m => m.Term("empresas.idnoticiaempresa", idNoticiaEmpresa))) &&
                                        q.MultiMatch(m => m
                                                        .Fields(f => f.Field(p => p.titulo)
                                                                        .Field(p => p.subtitulo)
                                                                        .Field(p => p.conteudo)
                                                        )
                                                        .Query(textoPesquisado)
                                                        .Operator(Operator.And)
                                        )
                                    )
                            .Highlight(h => h
                                .Fields(f => f.Field("*").PreTags("<span class='highlight'>").PostTags("</span>").NumberOfFragments(0).ForceSource(true))
                            )
                        );

            //return response.Documents.ToList<noticias>();
                        
            NoticiaElastic Noticia = response.Documents.First();

            if(textoPesquisado != "" && textoPesquisado != null)
            {
                foreach (var not in response.Hits)
                {
                    var _keys = not.Highlights;
                    foreach (var k in _keys)
                    {
                        if (k.Key == "conteudo")
                            Noticia.conteudo = k.Value.Highlights.FirstOrDefault();

                        if (k.Key == "titulo")
                            Noticia.titulo = k.Value.Highlights.FirstOrDefault();

                        if (k.Key == "subtitulo")
                            Noticia.subtitulo = k.Value.Highlights.FirstOrDefault();
                    }
                }
            }
            
            //return response.Documents.First();
            return Noticia;
        }        

    }
}
