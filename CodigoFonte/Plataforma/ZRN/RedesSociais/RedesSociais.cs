using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.RedesSociais
{
    public class RedesSociais
    {
        ZBD.Model.zeengEntities _bd;

        public RedesSociais()
        {
            _bd = new ZBD.Model.zeengEntities();
        }

        public List<Graficos.Linha> RetornaGraficoInteracoesEmpresa(int idEmpresa, DateTime dtInicial, DateTime dtFinal)
        {
            string dataIni = dtInicial.ToString("yyyy-MM-dd");
            string dataFim = dtFinal.ToString("yyyy-MM-dd");

            int numeroMes = 0;
            DateTime dataGrafico;
            var dadosGraf = new List<Graficos.Linha>();

            var grafico = new List<Graficos.Linha>();

            // Facebook
            // =================================================================================================================
            string query = "CALL GetFacebookInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Facebook.Interacoes> interacFace = _bd.Database.SqlQuery<Facebook.Interacoes>(query).ToList();

            numeroMes = Convert.ToDateTime(dataIni).Month;

            grafico.AddRange((from i in interacFace
                              select new Graficos.Linha
                              {
                                  categoria = "facebook",
                                  data = i.data.ToString("yyyy-MM-dd"),
                                  valor = i.posts
                              }).ToList());

            dataGrafico = Convert.ToDateTime(dtInicial).AddMonths(0 - numeroMes);
            while (dataGrafico <= Convert.ToDateTime(dtFinal))
            {
                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "facebook";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var lstDadosRedeSocial = grafico.Where(x =>
                                                        Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
                                                        Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year &&
                                                        x.categoria == "facebook"
                                                    ).ToList();
                if (lstDadosRedeSocial.Count > 0)
                {
                    valorLinha.categoria = lstDadosRedeSocial.FirstOrDefault().categoria;
                    valorLinha.valor = lstDadosRedeSocial.Sum(x => x.valor);
                }


                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddMonths(1);
            }


            // Twitter
            // =================================================================================================================
            string queryTW = "CALL GetTwitterInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Twitter.Interacoes> interacFaceTW = _bd.Database.SqlQuery<Twitter.Interacoes>(queryTW).ToList();

            numeroMes = Convert.ToDateTime(dataIni).Month;

            grafico.AddRange((from i in interacFaceTW
                              select new Graficos.Linha
                              {
                                  categoria = "twitter",
                                  data = i.data.ToString("yyyy-MM-dd"),
                                  valor = i.posts
                              }).ToList());

            dataGrafico = Convert.ToDateTime(dtInicial).AddMonths(0 - numeroMes);
            while (dataGrafico <= Convert.ToDateTime(dtFinal))
            {
                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "twitter";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var lstDadosRedeSocial = grafico.Where(x =>
                                                        Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
                                                        Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year &&
                                                        x.categoria == "twitter"
                                                    ).ToList();
                if (lstDadosRedeSocial.Count > 0)
                {
                    valorLinha.categoria = lstDadosRedeSocial.FirstOrDefault().categoria;
                    valorLinha.valor = lstDadosRedeSocial.Sum(x => x.valor);
                }

                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddMonths(1);
            }

            // Instagran
            // =================================================================================================================
            string queryIN = "CALL GetInstagramInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Instagram.Interacoes> interacFaceIN = _bd.Database.SqlQuery<Instagram.Interacoes>(queryIN).ToList();

            numeroMes = Convert.ToDateTime(dataIni).Month;

            grafico.AddRange((from i in interacFaceIN
                              select new Graficos.Linha
                              {
                                  categoria = "instagram",
                                  data = i.data.ToString("yyyy-MM-dd"),
                                  valor = i.posts
                              }).ToList());

            dataGrafico = Convert.ToDateTime(dtInicial).AddMonths(0 - numeroMes);
            while (dataGrafico <= Convert.ToDateTime(dtFinal))
            {
                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "instagram";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var lstDadosRedeSocial = grafico.Where(x =>
                                                        Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
                                                        Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year &&
                                                        x.categoria == "instagram"
                                                    ).ToList();
                if (lstDadosRedeSocial.Count > 0)
                {
                    valorLinha.categoria = lstDadosRedeSocial.FirstOrDefault().categoria;
                    valorLinha.valor = lstDadosRedeSocial.Sum(x => x.valor);
                }

                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddMonths(1);
            }

            // Youtube
            // =================================================================================================================
            string queryYT = "CALL GetYoutubeInteracoesSemana('" + idEmpresa + "', '" + dataIni + "', '" + dataFim + "');";

            List<Youtube.Interacoes> interacFaceYT = _bd.Database.SqlQuery<Youtube.Interacoes>(queryYT).ToList();

            numeroMes = Convert.ToDateTime(dataIni).Month;

            grafico.AddRange((from i in interacFaceYT
                              select new Graficos.Linha
                              {
                                  categoria = "youtube",
                                  data = i.data.ToString("yyyy-MM-dd"),
                                  valor = i.videos
                              }).ToList());

            dataGrafico = Convert.ToDateTime(dtInicial).AddMonths(0 - numeroMes);
            while (dataGrafico <= Convert.ToDateTime(dtFinal))
            {
                var valorLinha = new Graficos.Linha();
                valorLinha.categoria = "youtube";
                valorLinha.data = dataGrafico.ToString("yyyy-MM-dd");
                valorLinha.valor = 0;

                var lstDadosRedeSocial = grafico.Where(x =>
                                                        Convert.ToDateTime(x.data).Date.Month == dataGrafico.Date.Month &&
                                                        Convert.ToDateTime(x.data).Date.Year == dataGrafico.Date.Year &&
                                                        x.categoria == "youtube"
                                                    ).ToList();
                if (lstDadosRedeSocial.Count > 0)
                {
                    valorLinha.categoria = lstDadosRedeSocial.FirstOrDefault().categoria;
                    valorLinha.valor = lstDadosRedeSocial.Sum(x => x.valor);
                }

                dadosGraf.Add(valorLinha);
                dataGrafico = dataGrafico.AddMonths(1);
            }


            return dadosGraf;
        }

        public EmpresaRedeSocial VerificaRedesSociaisEmpresas(int idEmpresa)
        {
            var Redes = (from r in _bd.empresaredessociais
                             where r.idempresa == idEmpresa
                             select new DadosRedeSocial()
                             {
                                 idRede = r.idredesocial,
                                 Url = r.urlredesocial,
                                 idEmpresa = idEmpresa
                             }
                             ).ToList();

            if(Redes != null)
            {
                EmpresaRedeSocial EmpRedeSocial = new EmpresaRedeSocial();
                EmpRedeSocial.dadosFacebook = Redes.Where(x => x.idEmpresa == idEmpresa && x.idRede==1).Select(c => new DadosFacebook { Id = c.idRede, Url = c.Url }).FirstOrDefault();
                EmpRedeSocial.dadosTwitter = Redes.Where(x => x.idEmpresa == idEmpresa && x.idRede == 2).Select(c => new DadosTwitter { Id = c.idRede, Url = c.Url }).FirstOrDefault();
                EmpRedeSocial.dadosInstagram = Redes.Where(x => x.idEmpresa == idEmpresa && x.idRede == 3).Select(c => new DadosInstagram { Id = c.idRede, Url = c.Url }).FirstOrDefault();
                EmpRedeSocial.dadosYoutube = Redes.Where(x => x.idEmpresa == idEmpresa && x.idRede == 4).Select(c => new DadosYoutube { Id = c.idRede, Url = c.Url }).FirstOrDefault();
                return EmpRedeSocial;
            }
            
            return null;                         
        }
    }

    public class DadosRedeSocial
    {
        public int idEmpresa { get; set; }
        public int idRede { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }                
    }
}
