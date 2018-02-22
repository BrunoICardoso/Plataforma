using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;
using ZRN.Graficos.TabelaCalor;

namespace ZRN.RedesSociais.Twitter
{
    public class Twitter
    {
        int TotalPost;

        ZBD.Model.zeengEntities _bd = new ZBD.Model.zeengEntities();


        public List<Linha> RetornaGraficoCrescimentoSeguidores(int idEmpresa, DateTime dataInicial, DateTime dataFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dtInicial = dataInicial.ToString("yyyy-MM-dd");
            string dtFinal = dataFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTwitterCrescimentoFas('" + idEmpresa + "', '" + dtInicial + "', '" + dtFinal + "');";

            List<CrescimentoFas> dadosTw = _bd.Database.SqlQuery<CrescimentoFas>(query).ToList();

            var linhas = (from dadoTw in dadosTw
                          select new Linha()
                          {
                              categoria = "Twitter",
                              data = dadoTw.datahora.ToString("dd/MM/yyyy"),
                              //valor = dadoTw.diferenca_seguidores == dadoTw.qtdseguidores ? 0 : dadoTw.diferenca_seguidores
                              valor = dadoTw.Saldo == dadoTw.qtdseguidores ? 0 : dadoTw.Saldo
                          }).ToList();

            return linhas;
        }

        public List<Interacoes> RetornaGraficoQuantidadeDeInteracoesSemana(int idEmpresa, DateTime dataInicial, DateTime dataFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dtInicial = dataInicial.ToString("yyyy-MM-dd");
            string dtFinal = dataFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetTwitterInteracoesSemana('" + idEmpresa + "', '" + dtInicial + "', '" + dtFinal + "');";

            List<Interacoes> interacoesTwitter = _bd.Database.SqlQuery<Interacoes>(query).ToList();

            //foreach (var x in interacoesTwitter)
            //{
            //    x.dataFormatada = x.data.ToString("dd/MM/yyyy");

            //}

            return interacoesTwitter;

        }

        public TabelaCalor RetornaGraficoEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");
           
            string query = "CALL GetTwitterInteracoesPorHora('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<InteracoesHora> interacoesTw = _bd.Database.SqlQuery<InteracoesHora>(query).ToList();

             
            var tab = new TabelaCalor()
            {
                Itens = (from i in interacoesTw
                         select new ItemTabelaCalor()
                         {
                             DiaSemana = i.diaSemana,
                             Hora = i.hora,
                             Valor = i.interacoes,
                             retweets = i.retweets,
                             favoritados = i.favoritados
                 

                         }).ToList(),
                valorMaximo = interacoesTw.Count==0?0:interacoesTw.Max(x=>x.interacoes)
            };
                return tab;

            

        }

        public Seguidores RetornaSeguidores(int idEmpresa)
        {

            var seguidoresAtual = (from s in _bd.tw_perfis_stats
                                   where s.tw_perfis.empresas.idempresa == idEmpresa
                                   orderby s.datahora descending
                                   select s).FirstOrDefault();

            if (seguidoresAtual == null)
                return null;

            var dataAnterior = new DateTime(DateTime.Now.Ticks);
            dataAnterior = dataAnterior.AddDays(-30);



            var seguidoresAntes = (from s in _bd.tw_perfis_stats
                                   where s.tw_perfis.empresas.idempresa == idEmpresa &&
                                         s.datahora < dataAnterior
                                   orderby s.datahora descending
                                   select s).FirstOrDefault();

            if(seguidoresAntes == null)
            {
                seguidoresAntes = (from f in _bd.tw_perfis_stats
                                   where
                                        f.tw_perfis.empresas.idempresa == idEmpresa
                                   orderby f.datahora ascending
                                   select f).FirstOrDefault();
            }


            var seg = new Seguidores()
            {
                totalAnterior = seguidoresAntes.qtdseguidores,
                dataVarAnterior = seguidoresAntes.datahora,
                totalAtual = seguidoresAtual.qtdseguidores,
                dataVarAtual = seguidoresAtual.datahora
            };

            return seg;
        }

        public int RetornaTotalDePosts() {

            return this.TotalPost;
                }

        public int RetornaTotalDePosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var totalDePosts = (from p in _bd.tw_posts
                                where p.tw_perfis.empresas.idempresa == idEmpresa &&
                                p.datahora >= dtInicial &&
                                p.datahora <= dtFinal
                                select p.idpost).Count();

            return totalDePosts;
        }

        public List<Post> RetornaPostsMaiorEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdPosts, int pagina)
        {

            if (pagina <= 0)
            {
                pagina = 1;
            }

            var lista = (from post in _bd.tw_posts
                         join perfil in _bd.tw_perfis on post.idperfil equals perfil.idperfil
                         join mencao in _bd.tw_mencoes on perfil.idperfil equals mencao.idperfil into mencoes
                         where perfil.idempresa == idEmpresa &&
                               post.datahora >= dtInicial &&
                               post.datahora <= dtFinal

                         select new Post()
                         {
                             idPost = post.idpost,
                             dataPost = post.datahora,
                             totalFavoritadas = post.qtdfavoritado,
                             totalRetweets = post.qtdretweets,
                             totalMencoes = mencoes.Count(m => m.idperfil == post.idperfil && m.idtwrespondido == post.idtwitter),
                             imagem = post.nomeimagem
                         }).OrderByDescending(x => x.totalRetweets + x.totalFavoritadas + x.totalMencoes).Skip((pagina - 1) * qtdPosts).Take(qtdPosts).ToList();
            
            var resultado = lista;


            //var lista = posts.OrderByDescending(x => x.totalRetweets).Skip((pagina - 1) * qtdPosts).Take(qtdPosts).ToList();

            return lista;
        }

        public Post RetornaDadosPostTw(int id)
        {
            
            var post = (from p in _bd.tw_posts
                        where p.idpost == id
                        select new Post()
                        {
                            idPost = p.idpost,
                            idTwitterPost = p.idtwitter,
                            usuarioPost = p.tw_perfis.empresas.nome,
                            dataPost = p.datahora,
                            postagem = p.postagem,
                            totalFavoritadas = p.qtdfavoritado,
                            idPerfil = p.idperfil,
                            totalRetweets = p.qtdretweets,
                            imagem = p.nomeimagem

                        }).SingleOrDefault();

            post.totalrespostas = _bd.tw_mencoes.Where(x => x.idperfil == post.idPerfil && x.idtwrespondido == post.idTwitterPost).Count();
            
            post.postsReply = _bd.tw_mencoes.Where(x => x.idperfil == post.idPerfil && x.idtwrespondido == post.idTwitterPost).Select(z => new Post() {
                idPost = z.idmencao,
                postagem = z.postagem,
                dataPost = z.datahora,
                totalFavoritadas = z.qtdfavoritado,
                totalRetweets = z.qtdretweets
            }).OrderByDescending(x => x.dataPost).Take(3).ToList();

            return post;
        }
                
        public List<Comentario> RetornaMaisRetweets(int idpost , int inicial, int quantidade)
        {

            var retweets = (from tp in _bd.tw_posts
                            join tm in _bd.tw_mencoes on tp.idtwitter equals tm.idtwrespondido
                            where tp.idpost == idpost
                        
                            select new Comentario() {

                                idPost = tp.idpost,
                                idmencao = tm.idmencao,
                                usuarioPost = tm.nomeusuario,
                                postagem = tm.postagem,
                                dataPost = tm.datahora,
                                totalFavoritadas = tm.qtdfavoritado,
                                totalRetweets = tm.qtdretweets,
                                idTwitterPost = tm.idtwitter,
                                idPerfil = tm.idperfil

                            }).OrderByDescending(tm => tm.dataPost).Skip(inicial); 

            if (quantidade > 0)
                retweets = retweets.Take(quantidade);

            return retweets.ToList();
        }

        public List<Post> RetornaTwPostsEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdpagina, int pagina, string ordenacao, string caminhoImagem, string palavra, string PostComentario)
        {

            var post = (from p in _bd.tw_posts
                        where
                            p.tw_perfis.idempresa == idEmpresa &&
                            p.datahora >= dtInicial &&
                            p.datahora <= dtFinal
                        select p  
                            );

            if (palavra != "null" && PostComentario == "Post")
            {

                post = (from p in post
                         join t in _bd.tw_post_termos on p.idpost equals t.idpost
                         where
                                    t.termo == palavra
                         select p);
            }



            if (palavra != "null" && PostComentario == "Comentario")
            {

                post = (from p in post
                         join c in _bd.tw_mencoes on p.idtwitter equals c.idtwrespondido
                         join t in _bd.tw_mencao_termos on c.idmencao equals t.idmencao
                        where
                               t.termo == palavra
                         group p.idpost by new { p } into g
                         select g.Key.p);
            }

            this.TotalPost = post.Select(x => x.idpost).Count();






            switch (ordenacao.ToLower())
            {
                case "recentes":
                    post = post.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
                case "engajamento":
                    post = post.OrderByDescending(x => x.qtdretweets + x.qtdfavoritado).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;

                default:
                    post = post.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
            }

            var pp = post.ToList().Select(p=> new Post()
            {
                idPost = p.idpost,
                idTwitterPost = p.idtwitter,
                idPerfil = p.idperfil,
                usuarioPost = p.tw_perfis.empresas.nome,
                dataPost = p.datahora,
                imagem = p.nomeimagem,
                postagem = p.postagem,
                totalRetweets = p.qtdretweets,
                totalFavoritadas = p.qtdfavoritado,
                totalrespostas = _bd.tw_mencoes.Where(x => x.idtwrespondido == p.idtwitter).Select(x => x.idtwitter).Count(),
                imagemTw = new ImagemTW() { caminho = caminhoImagem + "/twitter/" + p.nomeimagem },
                comentarios = RetornaMaisRetweets(p.idpost,0,5)
            });

            List<Post> posts = new List<Post>();

            foreach(var p in pp.ToList())
            {   
                if(p.dataPost != null)
                {
                    p.dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.dataPost);
                    posts.Add(p);
                }
                
            }

            //return pp.ToList();
            return posts;
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosPosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            //var termos = (from t in _bd.tw_post_termos
            //              where
            //                t.tw_posts.tw_perfis.idempresa == idempresa &&
            //                t.tw_posts.datahora >= dtinicial &&
            //                t.tw_posts.datahora <= dtfinal &&
            //                t.tw_posts.is_reply == false &&
            //                t.tw_posts.is_retweet == false
            //              group t by t.termo into g
            //              orderby g.sum(x => x.frequencia) descending
            //              select new graficos.tagcloud.termo()
            //              {
            //                  termo = g.key,
            //                  frequencia = g.sum(x => x.frequencia)
            //              }).take(60);


            var termos = (from termo in _bd.tw_post_termos
                          join post in _bd.tw_posts on termo.idpost equals post.idpost
                          join perfil in _bd.tw_perfis on post.idperfil equals perfil.idperfil
                          where perfil.idempresa == idEmpresa &&
                                post.datahora >= dtInicial &&
                                post.datahora <= dtFinal &&
                                post.is_reply == false &&
                                post.is_retweet == false
                          group termo by termo.termo into res
                          orderby res.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = res.Key,
                              frequencia = res.Sum(x => x.frequencia)
                          }).Take(50).Where(x => x.frequencia > 1);



            return termos.ToList();
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosMencoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var termos = (from t in _bd.tw_mencao_termos
                          where
                            t.tw_mencoes.tw_perfis.idempresa == idEmpresa &&
                            t.tw_mencoes.datahora >= dtInicial &&
                            t.tw_mencoes.datahora <= dtFinal && 
                            t.tw_mencoes.is_reply == true
                          group t by t.termo into g
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x => x.frequencia)
                          }).Take(50).Where(x => x.frequencia > 1);

            return termos.ToList();
        }



    }
}
