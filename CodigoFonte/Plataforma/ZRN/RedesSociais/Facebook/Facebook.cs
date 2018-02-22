using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;
using ZRN.Graficos.TabelaCalor;

namespace ZRN.RedesSociais.Facebook
{
    public class Facebook
    {

        int TotalPost;

        ZBD.Model.zeengEntities _bd = new ZBD.Model.zeengEntities();

        /// <summary>
        /// Retorna a quantidade de seguidores da empresa
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <returns></returns>
        public Seguidores RetornaSeguidores(int idEmpresa)
        {

            var seguidoresAtual = (from f in _bd.fb_perfis_stats
                                   where f.fb_perfis.empresas.idempresa == idEmpresa
                                   orderby f.datahora descending
                                   select f).FirstOrDefault();

            if (seguidoresAtual == null)
                return null;


            var dataAnterior = new DateTime(DateTime.Now.Ticks);
            dataAnterior = dataAnterior.AddDays(-30);

            var seguidoresAntes = (from f in _bd.fb_perfis_stats
                                   where
                                        f.fb_perfis.empresas.idempresa == idEmpresa &&
                                        f.datahora < dataAnterior
                                   orderby f.datahora descending
                                   select f).FirstOrDefault();


            if (seguidoresAntes == null)
            {
                seguidoresAntes = (from f in _bd.fb_perfis_stats
                                   where
                                        f.fb_perfis.empresas.idempresa == idEmpresa
                                   orderby f.datahora ascending
                                   select f).FirstOrDefault();
            }


            var seg = new Seguidores()
            {
                totalAnterior = seguidoresAntes.qtdlikes,
                dataVarAnterior = seguidoresAntes.datahora,
                totalAtual = seguidoresAtual.qtdlikes,
                dataVarAtual = seguidoresAtual.datahora
            };

            return seg;
        }

        /// <summary>
        /// Retorna os dados para montar o gráfico de crescimento de fãs
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <param name="dtInicial">data inicial em que os dados devem ser consultados</param>
        /// <param name="dtFinal">data final em que os dados devem ser consultados</param>
        /// <param name="agruparpor">indica como deve ser agrupado os dados pesquisados</param>
        /// <returns>Retona uma lista de valores agrupados</returns>
        public List<Linha> RetornaGraficoCrescimentoFas(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {            
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetFacebookCrescimentoFas('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<CrescimentoFas> dadosFace = _bd.Database.SqlQuery<CrescimentoFas>(query).ToList();

            var linhas = (from d in dadosFace
                          select new Linha()
                          {
                              categoria = "Facebook",
                              data = d.datahora.ToString("dd/MM/yyyy"),
                              //valor = d.diferenca_likes == d.qtdlikes ? 0 : d.diferenca_likes
                              valor = d.Saldo == d.qtdlikes ? 0 : d.Saldo

                          }).ToList();
            return linhas;


        }

        /// <summary>
        /// Retorna os dados para montar o gráfico de crescimento de fãs
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <param name="dtInicial">data inicial em que os dados devem ser consultados</param>
        /// <param name="dtFinal">data final em que os dados devem ser consultados</param>
        /// <param name="agruparpor">indica como deve ser agrupado os dados pesquisados</param>
        /// <returns>Retona uma lista de valores agrupados</returns>
        public List<Linha> RetornaGraficoQuantidadePosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetFacebookCrescimentoFas('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<CrescimentoFas> dadosFace = _bd.Database.SqlQuery<CrescimentoFas>(query).ToList();

            var linhas = (from d in dadosFace
                          select new Linha()
                          {
                              categoria = "Facebook",
                              data = d.datahora.ToString("dd/MM/yyyy"),
                              valor = d.diferenca_seguidores == d.qtdlikes ? 0 : d.diferenca_seguidores
                          }).ToList();

            return linhas;

        }

        /// <summary>
        /// Retorna os dados para montar o gráfico de crescimento de fãs
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <param name="dtInicial">data inicial em que os dados devem ser consultados</param>
        /// <param name="dtFinal">data final em que os dados devem ser consultados</param>
        /// <param name="agruparpor">indica como deve ser agrupado os dados pesquisados</param>
        /// <returns>Retona uma lista de valores agrupados</returns>
        public List<Interacoes> RetornaGraficoQuantidadeInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetFacebookInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Interacoes> interacFace = _bd.Database.SqlQuery<Interacoes>(query).ToList();

            return interacFace;

        }

        public Interacoes RetornaTotalInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var totalInteracoes = (from i in _bd.fb_posts
                                   where
                                     i.fb_perfis.empresas.idempresa == idEmpresa &&
                                     i.datahora >= dtInicial &&
                                     i.datahora <= dtFinal
                                   group i by i.idpost into g
                                   select new Interacoes()
                                   {

                                       interacoes = g.Sum(x => x.reacoes + x.compartilhamentos + x.fb_comentarios.Count()),
                                       comentarios = g.Sum(x => x.fb_comentarios.Count()),
                                       compartilhamentos = g.Sum(x => x.compartilhamentos),
                                       posts = g.Count(),
                                       reacoes = g.Sum(x => x.reacoes)
                                   }).SingleOrDefault();

            return totalInteracoes;

        }

        public int RetornaTotalPosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {


            var posts = (from p in _bd.fb_posts
                         where
                            p.fb_perfis.empresas.idempresa == idEmpresa &&
                            p.datahora >= dtInicial &&
                            p.datahora <= dtFinal
                         select p.idpost).Count();

            return posts;
            
        }


        public int RetornaTotalPosts() { 

             return this.TotalPost;
        }


        public List<Graficos.TagCloud.Termo> RetornaTermosPosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var termos = (from t in _bd.fb_post_termos
                          where
                            t.fb_posts.fb_perfis.idempresa == idEmpresa &&
                            t.fb_posts.datahora >= dtInicial &&
                            t.fb_posts.datahora <= dtFinal
                          group t by t.termo  into g 
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x => x.frequencia)
                              
                          }).Take(50).Where(x => x.frequencia > 1);

            return termos.ToList();
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosComentarios(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            var termos = (from t in _bd.fb_comentarios_termos
                          where
                            t.fb_comentarios.fb_posts.fb_perfis.idempresa == idEmpresa &&
                            t.fb_comentarios.fb_posts.datahora >= dtInicial &&
                            t.fb_comentarios.fb_posts.datahora <= dtFinal
                          group t by t.termo into g
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x => x.frequencia)
                          }).Take(50).Where(x => x.frequencia > 1);

            return termos.ToList();
        }

        public List<Post> RetornaPostsMaiorEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdPosts, int pagina, string caminhoImagem)
        {
            var posts = (from p in _bd.fb_posts
                         where
                            p.fb_perfis.empresas.idempresa == idEmpresa &&
                            p.datahora >= dtInicial &&
                            p.datahora <= dtFinal
                         orderby (p.compartilhamentos + p.reacoes + p.fb_comentarios.Count()) descending
                         select new Post()
                         {
                             idPost = p.idpost,
                             dataPost = p.datahora,
                             //dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.datahora),
                             nomeimagem = p.nomeimagem,
                             totalComentarios = p.fb_comentarios.Count(),
                             totalCompartilhamentos = p.compartilhamentos,
                             totalCurtidas = p.reacoes,
                             qtdlikes = p.fb_perfis.fb_perfis_stats.Where(w=> w.datahora == p.datahora).Select(s=> s.qtdlikes).FirstOrDefault(),
                             imagem = new Imagem() { caminho = caminhoImagem + "/facebook/" + p.nomeimagem }
                         }).Skip((pagina - 1) * qtdPosts).Take(qtdPosts).ToList();

            posts = (from p in posts
                     select new Post() {
                     engajamento = ((p.totalComentarios + p.totalCompartilhamentos + p.totalCurtidas)/ p.qtdlikes)*100     
                     }
                     
                     ).ToList();



            foreach (var p in posts)
            {
                p.dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.dataPost);
            }

            return posts;
            
        }

        public TabelaCalor RetornaGraficoEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetFacebookInteracoesPorHora('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<InteracoesHora> interacFace = _bd.Database.SqlQuery<InteracoesHora>(query).ToList();

            var tab = new TabelaCalor()
            {
                Itens = (from i in interacFace
                         select new ItemTabelaCalor()
                         {
                             DiaSemana = i.diaSemana,
                             Hora = i.hora,
                             Valor = i.interacoes,
                             comentarios = i.comentarios,
                             reacoes = i.reacoes,
                             compartilhamentos = i.compartilhamentos
                         }).ToList(),
                valorMaximo = interacFace.Count() == 0 ? 0 : interacFace.Max(x => x.interacoes)
            };

            return tab;
        }

        public Post RetornaDadosPost(int idPost, string caminhoImagem)
        {

            var post = (from p in _bd.fb_posts
                        where p.idpost == idPost
                        select new Post()
                        {
                            idPost = p.idpost,
                            usuarioPost = p.fb_perfis.empresas.nome,
                            dataPost = p.datahora,
                            //dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.datahora),
                            nomeimagem = p.nomeimagem,
                            postagem = p.postagem,
                            totalComentarios = p.fb_comentarios.Count(),
                            totalCompartilhamentos = p.compartilhamentos,
                            totalCurtidas = p.reacoes,
                            totalsemreposta = p.fb_comentarios.Where(x => x.idcomentarioresposta == null).Count(),
                            imagem = new Imagem() { caminho = caminhoImagem + "/facebook/" + p.nomeimagem }

                        }).SingleOrDefault();

            post.dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(post.dataPost);

            post.comentarios = _bd.fb_comentarios.Where(x => x.idpost == idPost && x.idcomentarioresposta == null).Select(c => new Comentario()
            {
                idcomentario = c.idcomentario,
                idfacebook = c.idfacebook,
                idpost = c.idpost,
                nomeusuario = c.nomeusuario,
                datahora = c.datahora,
                //datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(c.datahora),
                postagem = c.postagem,
                nomeimagem = c.nomeimagem,
                totalCurtidas = c.curtidas,
                totalRespostas = c.respostas == null ? 0 : c.respostas,

                respostas = _bd.fb_comentarios.Where(x => x.idcomentarioresposta == c.idfacebook).Select(z => new Resposta()
                {
                    idcomentario = z.idcomentario,
                    idfacebook = z.idfacebook,
                    idrespostafacebook = z.idcomentarioresposta,
                    idpost = z.idpost,
                    datahora = z.datahora,
                    //datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(z.datahora),
                    nomeusuario = z.nomeusuario,
                    postagem = z.postagem,
                    nomeimagem = z.nomeimagem,
                    totalCurtidas = z.curtidas
                }).Take(1)

            }).OrderByDescending(x => x.datahora).Take(3).ToList();

            foreach(var c in post.comentarios)
            {
                c.datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(c.datahora);
                foreach(var r in c.respostas)
                {
                    r.datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(r.datahora);
                }
            }

            return post;
        }


        

        public List<Post> RetornaPostsEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdpagina, int pagina, string ordenacao, string caminhoImagem, string palavra,string PostComentario)
        {
            var post = (from p in _bd.fb_posts
                        where
                            p.fb_perfis.idempresa == idEmpresa &&
                            p.datahora >= dtInicial &&
                            p.datahora <= dtFinal
                        select p );

            if (palavra != "null" && PostComentario=="Post")
            {
                post = (from p in post
                        join t in _bd.fb_post_termos on p.idpost equals t.idpost
                        where
                                   t.termo == palavra
                        select p);
            }
            
            if (palavra != "null" && PostComentario == "Comentario")
            {
                 post = (from p in post
                         join c in _bd.fb_comentarios on p.idpost equals c.idpost
                         join t in _bd.fb_comentarios_termos on c.idcomentario equals t.idcomentario
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
                    post = post.OrderByDescending(x => x.comentarios + x.compartilhamentos + x.reacoes).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;

                default:
                    post = post.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
            }

            
            var pp = post.ToList().Select(p => new Post()
            {
                idPost = p.idpost,
                usuarioPost = p.fb_perfis.empresas.nome,
                dataPost = p.datahora,
                //dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.datahora),
                nomeimagem = p.nomeimagem,
                postagem = p.postagem,
                totalComentarios = p.fb_comentarios.Count(),
                totalCompartilhamentos = p.compartilhamentos,
                totalCurtidas = p.reacoes,
                totalsemreposta = p.fb_comentarios.Where(x => x.idcomentarioresposta == null).Count(),
                imagem = new Imagem() { caminho = caminhoImagem + "/facebook/" + p.nomeimagem },

                comentarios = RetornaComentariosPost(Convert.ToInt32(p.idpost), 0 ,5)
                });

            List<Post> posts = new List<Post>();
            foreach (var p in pp)
            {
                p.dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.dataPost);
                posts.Add(p);
            }
            
            return posts;
        }

        public List<Comentario> RetornaComentariosPost(int idPost, int inicial, int quantidade)
        {

     
            var comentarios = _bd.fb_comentarios.Where(x => x.idpost == idPost && x.idcomentarioresposta == null).Select(c => new Comentario()
            {
                idcomentario = c.idcomentario,
                idfacebook = c.idfacebook,
                idpost = c.idpost,
                datahora = c.datahora,
                //datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(c.datahora),
                nomeusuario = c.nomeusuario,
                postagem = c.postagem,
                totalCurtidas = c.curtidas,
                totalRespostas = c.respostas,
                urlimagem = c.urlimagem,
                respostas = _bd.fb_comentarios.Where(x => x.idcomentarioresposta == c.idfacebook).Select(z => new Resposta()
                {
                    idfacebook = z.idfacebook,
                    idrespostafacebook = z.idcomentarioresposta,
                    idcomentario = z.idcomentario,
                    idpost = z.idpost,
                    datahora = z.datahora,
                    //datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(z.datahora),
                    nomeusuario = z.nomeusuario,
                    postagem = z.postagem,
                    nomeimagem = z.nomeimagem,
                    totalCurtidas = z.curtidas
                }).Take(1)
            }).OrderByDescending(x => x.datahora).Skip(inicial);

           // comentarios.Where(x => x.idpost == idPost).Select(x => new Comentario { totalRespostasPost = x.totalRespostas }).Count();


            if (quantidade > 0)
                comentarios = comentarios.Take(quantidade);

            List<Comentario> resultComentarios = new List<Comentario>();

            foreach (var c in comentarios)
            {
                c.datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(c.datahora);
                resultComentarios.Add(c);

                foreach (var r in c.respostas)
                {
                    r.datahora = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(r.datahora);                    
                }
            }


            //return comentarios.ToList();
            return resultComentarios;
        }

        public List<Resposta> RetornaRespostasComentario(string idfacebookcomentario, int inicial, int quantidade)
        {
            var comentarios = _bd.fb_comentarios.Where(x => x.idcomentarioresposta == idfacebookcomentario).Select(z => new Resposta()
            {
                idfacebook = z.idfacebook,
                idrespostafacebook = z.idcomentarioresposta,
                idcomentario = z.idcomentario,
                idpost = z.idpost,
                datahora = z.datahora,
                nomeusuario = z.nomeusuario,
                postagem = z.postagem,
                nomeimagem = z.nomeimagem,
                totalCurtidas = z.curtidas
            }).OrderByDescending(x => x.datahora).Skip(inicial);

            if (quantidade > 0)
                comentarios = comentarios.Take(quantidade);


            return comentarios.ToList();
        }

    }
}
