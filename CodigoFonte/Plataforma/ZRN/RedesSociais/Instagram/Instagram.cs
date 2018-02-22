using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;
using ZRN.Graficos.TabelaCalor;

namespace ZRN.RedesSociais.Instagram
{
    public class Instagram
    {
        int TotalPost;

        ZBD.Model.zeengEntities _bd = new ZBD.Model.zeengEntities();

        /// <summary>
        /// Retorna a quantidade de seguidores da empresa
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <returns></returns>
        public Seguidores RetornaSeguidoresInsta(int idEmpresa)
        {

            var seguidoresAtual = (from i in _bd.insta_perfis_stats
                                   where i.insta_perfis.empresas.idempresa == idEmpresa
                                   orderby i.datahora descending
                                   select i).FirstOrDefault();

            if (seguidoresAtual == null)
                return null;


            var dataAnterior = new DateTime(DateTime.Now.Ticks);
            dataAnterior = dataAnterior.AddDays(-30);

            var seguidoresAntes = (from i in _bd.insta_perfis_stats
                                   where
                                        i.insta_perfis.empresas.idempresa == idEmpresa &&
                                        i.datahora < dataAnterior
                                   orderby i.datahora descending
                                   select i).FirstOrDefault();


            if (seguidoresAntes == null)
            {
                seguidoresAntes = (from i in _bd.insta_perfis_stats
                                   where
                                        i.insta_perfis.empresas.idempresa == idEmpresa
                                   orderby i.datahora ascending
                                   select i).FirstOrDefault();
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

        /// <summary>
        /// Retorna os dados para montar o gráfico de crescimento de fãs
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <param name="dtInicial">data inicial em que os dados devem ser consultados</param>
        /// <param name="dtFinal">data final em que os dados devem ser consultados</param>
        /// <param name="agruparpor">indica como deve ser agrupado os dados pesquisados</param>
        /// <returns>Retona uma lista de valores agrupados</returns>
        public List<Linha> RetornaGraficoInstaCrescimentoSeguidores(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetInstagramCrescimentoSeguidores('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<CrescimentoSeguidores> dadosInsta = _bd.Database.SqlQuery<CrescimentoSeguidores>(query).ToList();

            var linhas = (from d in dadosInsta
                          select new Linha()
                          {
                              categoria = "Instagram",
                              data = d.datahora.ToString("dd/MM/yyyy"),
                              //valor = d.diferenca_seguidores == d.qtdseguidores ? 0 : d.diferenca_seguidores
                              valor = d.Saldo == d.qtdseguidores ? 0 : d.Saldo
                          }).ToList();

            return linhas;

        }

        public List<Interacoes> RetornaGraficoQuantidadeInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetInstagramInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Interacoes> interacInsta = _bd.Database.SqlQuery<Interacoes>(query).ToList();

            //foreach(var x in interacInsta)
            //{
            //    x.dataFormatada = x.data.ToString("dd/MM/yyyy");
            //}

            return interacInsta;

        }

        public List<Post> RetornaPostsMaiorEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdPosts, int pagina, string caminhoImagem)
        {
            if (pagina <= 0)
            {
                pagina = 1;
            }
            var posts = (from p in _bd.insta_posts
                         where p.insta_perfis.empresas.idempresa == idEmpresa &&
                         p.datahora >= dtInicial &&
                         p.datahora <= dtFinal
                         orderby (p.qtdcomentarios + p.qtdcurtidas) descending
                         select new Post() {
                             idPost = p.idpost,
                             dataPost = p.datahora,
                             totalComentarios = p.qtdcomentarios,
                             totalCurtidas = p.qtdcurtidas,
                             urlImagem = p.urlimagem,
                             nomeImagem = p.nomeimagem,
                             imagem = new Imagem() { caminho = caminhoImagem + "/instagram/" + p.nomeimagem }
                         }).Skip((pagina - 1) * qtdPosts).Take(qtdPosts).ToList();
            
            return posts;
        }

        public TabelaCalor RetornaGraficoEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetInstagramInteracoesPorHora('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<InteracoesHora> interacoesInsta = _bd.Database.SqlQuery<InteracoesHora>(query).ToList();

           
                var tab = new TabelaCalor()
                {
                    Itens = (from i in interacoesInsta
                             select new ItemTabelaCalor()
                             {
                                 DiaSemana = i.diaSemana,
                                 Hora = i.hora,
                                 Valor = i.interacoes,
                                 comentarios = i.comentarios,
                                 reacoes = i.curtidas
                             }).ToList(),
                    valorMaximo = interacoesInsta.Count()==0?0:interacoesInsta.Max(x => x.interacoes)
                };

                return tab;
        }

        public int RetornaTotalDePosts() {


            return this.TotalPost;

        }


        public int RetornaTotalDePosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var totalDePosts = (from p in _bd.insta_posts
                                where p.insta_perfis.empresas.idempresa == idEmpresa &&
                                p.datahora >= dtInicial &&
                                p.datahora <= dtFinal
                                select p.idpost).Count();
            return totalDePosts;
        }

        //retorna post com os primeiros 3 comentarios.
        public Post RetornaPostIntagram(int id, string caminhoImagem)
        {
            var post = (from p in _bd.insta_posts
                        where p.idpost == id
                        select new Post()
                        {
                            idPost = p.idpost,
                            dataPost = p.datahora,
                            postagem = p.postagem,
                            urlImagem = p.urlimagem,
                            nomeImagem = p.nomeimagem,
                            totalCurtidas = p.qtdcurtidas,
                            usuarioPost = p.insta_perfis.empresas.nome,
                            imagem = new Imagem() { caminho = caminhoImagem + "/instagram/" + p.nomeimagem }


                        }).SingleOrDefault();

            post.totalComentarios = _bd.insta_comentarios.Where(x => x.idpost == post.idPost).Count();

            post.comentarios = _bd.insta_comentarios.Where(x => x.idpost == post.idPost).Select(c => new Comentario()
            {
                dataComentario = c.datahora,
                idcomentario = c.idcomentario,
                nomeUsuario = c.nomeusuario,
                postagem = c.postagem
            }).OrderByDescending(x => x.dataComentario).Take(3).ToList();
            
            return post;
        }

        public List<Comentario> RetornaMaisComentariosDeUmPost(int idPost, int inicial, int quantidade){


            var comentarios = _bd.insta_comentarios.Where(x => x.idpost == idPost).Select(c => new Comentario()
            {
                idPost = c.idpost,
                dataComentario = c.datahora,
                idcomentario = c.idcomentario,
                nomeUsuario = c.nomeusuario,
                postagem = c.postagem,
                        
            }).OrderByDescending(x => x.dataComentario).Skip(inicial);
             

            if (quantidade > 0)
                comentarios = comentarios.Take(quantidade);

            return comentarios.ToList();
        }

        public List<Post> RetornaPostsTimelineInstagram(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdpagina, int pagina, string ordenacao, string caminhoImagem, string palavra, string PostComentario)
        {
            var posts = (from p in _bd.insta_posts
                         where p.insta_perfis.empresas.idempresa == idEmpresa &&
                         p.datahora >= dtInicial &&
                         p.datahora <= dtFinal
                         select p);


            if (palavra != "null" && PostComentario == "Post")
            {

                posts = (from p in posts
                        join t in _bd.insta_post_termos on p.idpost equals t.idpost
                        where
                                   t.termo == palavra
                        select p);
            }



            if (palavra != "null" && PostComentario == "Comentario")
            {

                posts= (from p in posts
                        join c in _bd.insta_comentarios on p.idpost equals c.idpost
                        join t in _bd.insta_comentario_termos on c.idcomentario equals t.idcomentario
                        where
                              t.termo == palavra
                        group p.idpost by new { p } into g
                        select g.Key.p);
            }

            this.TotalPost = posts.Select(x => x.idpost).Count();





            switch (ordenacao.ToLower())
            {
                case "recentes":
                    posts = posts.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
                case "engajamento":
                    posts = posts.OrderByDescending(x => x.insta_comentarios.Select(x1 => x1.idcomentario).Count() + x.qtdcurtidas).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
                default:
                    posts = posts.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
            }


            var pp = posts.ToList().Select(p => new Post()
            {
                idPost = p.idpost,
                dataPost = p.datahora,
                postagem = p.postagem,
                urlImagem = p.urlimagem,
                nomeImagem = p.nomeimagem,
                totalCurtidas = p.qtdcurtidas,
                usuarioPost = p.insta_perfis.empresas.nome,
                totalComentarios = p.insta_comentarios.Select(x => x.idcomentario).Count(),
                imagem = new Imagem() { caminho = caminhoImagem + "/instagram/" + p.nomeimagem },
                comentarios = RetornaMaisComentariosDeUmPost(p.idpost,0,5)

            });

            List<Post> Posts = new List<Post>();
            foreach (var p in pp)
            {
                p.dataPost = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(p.dataPost);
                Posts.Add(p);
            }

            return Posts;
            //return pp.ToList();
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosPostInstagram(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var termos = (from t in _bd.insta_post_termos
                          where t.insta_posts.insta_perfis.idempresa == idEmpresa &&
                                 t.insta_posts.datahora >= dtInicial &&
                                 t.insta_posts.datahora <= dtFinal
                          group t by t.termo into g
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x=> x.frequencia) 
                          }).Take(50).Where(x => x.frequencia > 1).ToList();

            return termos;
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosComentarios(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var termos = (from t in _bd.insta_comentario_termos
                          where 
                            t.insta_comentarios.insta_posts.insta_perfis.idempresa == idEmpresa &&
                            t.insta_comentarios.insta_posts.datahora >= dtInicial &&
                            t.insta_comentarios.insta_posts.datahora <= dtFinal
                          group t by t.termo into g
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x => x.frequencia)
                          }).Take(50).Where(x => x.frequencia > 1).ToList();

            return termos;
        }

    }
}
