using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ZRN.Promocoes
{
    public class PromocaoRedesSociais
    {

        private string _servidorElastic;
        private string _indiceElastic;
        
        public PromocaoRedesSociais(string servidorElastic, string indiceElastic)
        {
            _servidorElastic = servidorElastic;
            _indiceElastic = indiceElastic;
        }

        public List<ZRN.Promocoes.fb_post> retornaPostsFacebook(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            pagina = pagina - 1;

            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "fb_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<fb_post>(s =>                                  
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                            .From(pagina)
                                            .Size(qtdeRegistros)
                                        );            
                        
            return resp.Documents.ToList();
        }

        public int retornaTotalPostsFacebook(int idPromocao, int idEmpresa)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "fb_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<fb_post>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                        );

            return Convert.ToInt32(resp.Total);
        }

        public List<ZRN.Promocoes.tw_post> retornaPostsTwitter(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            pagina = pagina - 1;

            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "tw_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<tw_post>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                            .From(pagina)
                                            .Size(qtdeRegistros)
                                        );

            return resp.Documents.ToList();
        }

        public int retornaTotalPostsTwitter(int idPromocao, int idEmpresa)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "tw_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<tw_post>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                        );

            return Convert.ToInt32(resp.Total);
        }

        public List<ZRN.Promocoes.insta_post> retornaPostsInstagram(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            pagina = pagina - 1;

            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "insta_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<insta_post>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                            .From(pagina)
                                            .Size(qtdeRegistros)
                                        );

            return resp.Documents.ToList();
        }

        public int retornaTotalPostsInstagram(int idPromocao, int idEmpresa)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "insta_posts");
            var client = new ElasticClient(senttings);

            var resp = client.Search<insta_post>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                        );

            return Convert.ToInt32(resp.Total);
        }

        public List<ZRN.Promocoes.yt_video> retornaPostsYoutube(int idPromocao, int idEmpresa, int qtdeRegistros, int pagina)
        {
            pagina = pagina - 1;

            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "yt_videos");
            var client = new ElasticClient(senttings);

            var resp = client.Search<yt_video>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                            .From(pagina)
                                            .Size(qtdeRegistros)
                                        );

            return resp.Documents.ToList();
        }

        public int retornaTotalPostsYoutube(int idPromocao, int idEmpresa)
        {
            var node = new Uri(_servidorElastic);
            var senttings = new ConnectionSettings(node);
            senttings.DisableDirectStreaming(true);
            senttings.DefaultIndex(_indiceElastic + "yt_videos");
            var client = new ElasticClient(senttings);

            var resp = client.Search<yt_video>(s =>
                                    s.Query(q =>
                                                q.Term("promocoes.idpromocao", idPromocao) &&
                                                q.Term("idempresa", idEmpresa)
                                            )
                                        );

            return Convert.ToInt32(resp.Total);
        }
    }
}
