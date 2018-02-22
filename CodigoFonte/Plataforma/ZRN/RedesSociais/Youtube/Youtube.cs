using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRN.Graficos;
using ZRN.Graficos.TabelaCalor;

namespace ZRN.RedesSociais.Youtube
{
    public class Youtube
    {

        ZBD.Model.zeengEntities _bd = new ZBD.Model.zeengEntities();

        /// <summary>
        /// Retorna a quantidade de seguidores da empresa
        /// </summary>
        /// <param name="idEmpresa">id da empresa a ser consultada</param>
        /// <returns></returns>
        public Seguidores RetornaSeguidoresYoutube(int idEmpresa)
        {

            var seguidoresAtual = (from i in _bd.yt_canais_stats
                                   where i.yt_canais.empresas.idempresa == idEmpresa
                                   orderby i.datahora descending
                                   select i).FirstOrDefault();

            if (seguidoresAtual == null)
                return null;


            var dataAnterior = new DateTime(DateTime.Now.Ticks);
            dataAnterior = dataAnterior.AddDays(-30);

            var seguidoresAntes = (from i in _bd.yt_canais_stats
                                   where
                                        i.yt_canais.empresas.idempresa == idEmpresa &&
                                        i.datahora < dataAnterior
                                   orderby i.datahora descending
                                   select i).FirstOrDefault();


            if (seguidoresAntes == null)
            {
                seguidoresAntes = (from i in _bd.yt_canais_stats
                                   where
                                        i.yt_canais.empresas.idempresa == idEmpresa
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
        public List<Linha> RetornaGraficoYoutubeCrescimentoSeguidores(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetYoutubeCrescimentoSeguidores('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<CrescimentoSeguidores> dadosInsta = _bd.Database.SqlQuery<CrescimentoSeguidores>(query).ToList();

            var linhas = (from d in dadosInsta
                          select new Linha()
                          {
                              categoria = "Youtube",
                              data = d.datahora.ToString("dd/MM/yyyy"),
                              //valor = d.diferenca_seguidores == d.qtdseguidores ? 0 : d.diferenca_seguidores
                              valor = d.Saldo == d.qtdseguidores ? 0 : d.Saldo
                          }).ToList();

            return linhas;

        }

        public List<Interacoes> RetornaGraficoYoutubeQuantidadeInteracoes(int idEmpresa, DateTime dtInicial, DateTime dtFinal, Graficos.Configuracoes.AgruparPor agruparpor)
        {

            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetYoutubeInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Interacoes> interacYoutube = _bd.Database.SqlQuery<Interacoes>(query).ToList();

            //foreach(var x in interacYoutube)
            //{
            //    x.dataFormatada = x.data.ToString("dd/MM/yyyy");
            //}

            return interacYoutube;

        }
        
        public TabelaCalor RetornaGraficoYoutubeEngajamentoPorHora(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            string query = "CALL GetYoutubeInteracoesPorHora('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<InteracoesHora> interacoesYoutube = _bd.Database.SqlQuery<InteracoesHora>(query).ToList();

           
                var tab = new TabelaCalor()
                {
                    Itens = (from i in interacoesYoutube
                             select new ItemTabelaCalor()
                             {
                                 DiaSemana = i.diaSemana,
                                 Hora = i.hora,
                                 Valor = i.interacoes,
                                 reacoes = i.curtidas,
                                 comentarios = i.comentarios
                             }).ToList(),
                    valorMaximo = interacoesYoutube.Count==0?0:interacoesYoutube.Max(x => x.interacoes)
                };
                return tab;

        }

        public List<Video> RetornaPostsMaiorEngajamento(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdPosts, int pagina)
        {
            if (pagina <= 0) {
                pagina = 1;
            }

            var videos = (from v in _bd.yt_videos
                          where v.yt_canais.empresas.idempresa == idEmpresa &&
                          v.datahora >= dtInicial &&
                          v.datahora <= dtFinal
                          orderby (v.qtdcomentarios +
                                    v.qtdcurtidas +
                                      v.qtddescurtidas +
                                        v.qtdfavoritados +
                                          v.qtdvisualizacoes) descending
                          select new Video() {
                              idVideo = v.idvideo,
                              dataVideo = v.datahora,
                              qtdComentarios = v.qtdcomentarios,
                              qtdCurtidas = v.qtdcurtidas,
                              qtdDescurtidas = v.qtddescurtidas,
                              qtdFavoritados = v.qtdfavoritados,
                              qtdVisualizacoes = v.qtdvisualizacoes,
                              nomeImagem = v.nomeimagem
                          }).Skip((pagina - 1) * qtdPosts).Take(qtdPosts).ToList();
            return videos;
           

        }

        public int RetornaTotalDePosts(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var totalDeVideos = (from v in _bd.yt_videos
                                 where v.yt_canais.empresas.idempresa == idEmpresa &&
                                 v.datahora >= dtInicial &&
                                 v.datahora <= dtFinal
                                 select v.idvideo).Count();

            return totalDeVideos;
        }

        public Video RetornaDadosVideoYoutube(int id) {
            var video = (from v in _bd.yt_videos
                         where v.idvideo == id
                         select new Video()
                         {
                             idVideo = v.idvideo,
                             dataVideo = v.datahora,
                             qtdCurtidas = v.qtdcurtidas,
                             qtdDescurtidas = v.qtddescurtidas,
                             qtdFavoritados = v.qtdfavoritados,
                             qtdVisualizacoes = v.qtdvisualizacoes,
                             postagem = v.titulo + " - " + v.descricao,
                             usuariopost = v.yt_canais.empresas.nome,
                             nomeImagem = v.nomeimagem,
                             qtdComentarios = v.qtdcomentarios,
                             qtdComentariosReal = _bd.yt_comentarios.Where(x => x.idvideo == v.idvideo && x.idcomentarioresposta == null).Count()
                         }).SingleOrDefault();

            //video.qtdComentarios = _bd.yt_comentarios.Where(x => x.idvideo == video.idVideo && x.idcomentarioresposta == video.idVideo).Count();

            video.comentarios = _bd.yt_comentarios.Where(x => x.idvideo == video.idVideo && x.idcomentarioresposta == null).Select(z => new Comentario()
            {
                idvideo = z.idvideo,
                idcomentario = z.idcomentario,
                dataComentario = z.datahora,
                nomeUsuario = z.nomeusuario,
                postagem = z.postagem,
                totalCurtidas = z.qtdcurtidas,
                totalRespostas = z.qtdrespostas,
                respostas = _bd.yt_comentarios.Where(cx => cx.idcomentarioresposta == z.idcomentario).Select(rs => new Resposta {
                    dataComentario = rs.datahora,
                    idcomentario = rs.idcomentario,
                    idvideo = rs.idvideo,
                    idcomentarioresposta = rs.idcomentarioresposta,
                    nomeUsuario = rs.nomeusuario,
                    postagem = rs.postagem,
                    totalCurtidas = rs.qtdcurtidas 
                }).Take(1)
            }).OrderByDescending(x => x.dataComentario).Take(3).ToList();

            return video;
        }

        public List<Comentario> RetornaComentariosDeUmVideo(int idVideo, int inicial, int quantidade)
        {
            var comentarios = _bd.yt_comentarios.Where(x => x.idvideo == idVideo && x.idcomentarioresposta == null).Select(z => new Comentario()
            {
                idvideo = z.idvideo,
                idcomentario = z.idcomentario,
                dataComentario = z.datahora,
                nomeUsuario = z.nomeusuario,
                postagem = z.postagem,
                totalCurtidas = z.qtdcurtidas,
                totalRespostas = z.qtdrespostas,
                respostas = _bd.yt_comentarios.Where(c => c.idcomentarioresposta == z.idcomentario).Select(s => new Resposta()
                {
                    dataComentario = s.datahora,
                    idcomentario = s.idcomentario,
                    idcomentarioresposta = s.idcomentarioresposta,
                    idvideo = s.idvideo,
                    nomeUsuario = s.nomeusuario,
                    postagem = s.postagem
                }).Take(1)
            }).OrderByDescending(x => x.dataComentario).Skip(inicial);

            if (quantidade > 0)
                comentarios = comentarios.Take(quantidade);

            return comentarios.ToList();
        }
                
        public List<Resposta> RetornaRespostasComentario(int idComentario, int inicial, int quantidade)
        {
            var respostas = _bd.yt_comentarios.Where(c => c.idcomentarioresposta == idComentario).Select(r => new Resposta()
            {
                dataComentario = r.datahora,
                idcomentario = r.idcomentario,
                idcomentarioresposta = r.idcomentarioresposta,
                idvideo = r.idvideo,
                nomeUsuario = r.nomeusuario,
                postagem = r.postagem
            }).OrderByDescending(x => x.dataComentario).Skip(inicial);

            if (quantidade > 0)
                respostas = respostas.Take(quantidade);
            
            return respostas.ToList();
        }

        public List<Video> RetornaVideosYoutubeEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal, int qtdpagina, int pagina, string ordenacao)
        {
            var videos = (from v in _bd.yt_videos
                          where v.yt_canais.empresas.idempresa == idEmpresa &&
                                v.datahora >= dtInicial &&
                                 v.datahora <= dtFinal
                          select v);
            

            switch (ordenacao.ToLower())
            {
                case "recentes":
                    videos = videos.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
                case "engajamento":
                    videos = videos.OrderByDescending(x => x.qtdcomentarios + x.qtdcurtidas + x.qtddescurtidas + x.qtdfavoritados).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
                default:
                    videos = videos.OrderByDescending(x => x.datahora).Skip((pagina - 1) * qtdpagina).Take(qtdpagina);
                    break;
            }

            var vv = videos.ToList().Select(v => new Video()
            {
                idVideo = v.idvideo,
                usuariopost = v.yt_canais.empresas.nome,
                dataVideo = v.datahora,
                postagem = v.titulo + " - " + v.descricao,
                qtdComentarios = v.qtdcomentarios,
                qtdCurtidas = v.qtdcurtidas,
                qtdDescurtidas = v.qtddescurtidas,
                qtdFavoritados = v.qtdfavoritados,
                qtdVisualizacoes = v.qtdvisualizacoes,
                urlImagem = v.urlimagem,
                nomeImagem = v.nomeimagem,
                qtdComentariosReal = _bd.yt_comentarios.Where(x => x.idvideo == v.idvideo && x.idcomentarioresposta == null).Count(),
                comentarios = RetornaComentariosDeUmVideo(v.idvideo,0,5)
            });

            List<Video> Videos = new List<Video>();
            foreach(var v in vv)
            {
                v.dataVideo = UtilsZRN.FuncoesDatas.ConvertDataHoraTimeZoneUTCtoBrasil(v.dataVideo);
                Videos.Add(v);
            }

            return Videos;
            //return vv.ToList();
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosVideosYoutube(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var termos = (from t in _bd.yt_video_termos
                          where t.yt_videos.yt_canais.idempresa == idEmpresa &&
                          t.yt_videos.datahora >= dtInicial &&
                          t.yt_videos.datahora <= dtFinal
                          group t by t.termo into g
                          orderby g.Sum(x => x.frequencia) descending
                          select new Graficos.TagCloud.Termo()
                          {
                              termo = g.Key,
                              frequencia = g.Sum(x => x.frequencia)
                          }).Take(50).Where(x => x.frequencia > 1);
            return termos.ToList();
        }

        public List<Graficos.TagCloud.Termo> RetornaTermosComentariosYoutube(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            var termos = (from t in _bd.yt_comentario_termos
                          where t.yt_comentarios.yt_videos.yt_canais.idempresa == idEmpresa &&
                          t.yt_comentarios.yt_videos.datahora >= dtInicial &&
                          t.yt_comentarios.yt_videos.datahora <= dtFinal
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
