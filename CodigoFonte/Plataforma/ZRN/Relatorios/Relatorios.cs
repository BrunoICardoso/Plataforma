using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class Relatorios
    {

        private ZBD.Model.zeengEntities db = new ZBD.Model.zeengEntities();

        public List<metricasFacebook> RetornaMetricasFacebook(int idRelatorio)
        {

            var relatorio = Relatorio(idRelatorio);

            if (relatorio != null)
            {

                string dataInicial = relatorio.dataInicial.Value.ToString("yyyy-MM-dd");
                string dataFinal = relatorio.dataFinal.Value.ToString("yyyy-MM-dd");
                string empresas = retornaEmpresasRelatorio(idRelatorio);

                string query = "CALL GetMetricasFace('" + empresas + "', '" + dataInicial + "', '" + dataFinal + "');";

                List<metricasFacebook> metricasFacebook = db.Database.SqlQuery<metricasFacebook>(query).ToList();

                return metricasFacebook;

            }
            else
            {
                return new List<metricasFacebook>();
            }

        }

        private string retornaEmpresasRelatorio(int idRelatorio)
        {

            var empresas = (from empresa in db.cliente_relatorio_empresas
                            where empresa.idclienterelatorio == idRelatorio
                            select empresa.idempresa).ToList();

            return string.Join(",", empresas);
        }

        private Relatorio Relatorio(int idRelatorio)
        {

            var resultado = (from relatorio in db.cliente_relatorios
                             where relatorio.excluido == false && relatorio.idclienterelatorio == idRelatorio
                             select new Relatorio()
                             {
                                 idCliente = relatorio.idcliente,
                                 idClienteRelatorio = relatorio.idclienterelatorio,
                                 nome = relatorio.nome,
                                 idUsuario = relatorio.idusuario,
                                 dataHora = relatorio.datahora,
                                 dataInicial = relatorio.dataini,
                                 dataFinal = relatorio.datafim,
                                 excluido = relatorio.excluido
                             }).FirstOrDefault();

            return resultado;

        }

        public Relatorio RetornaRelatorio(int idRelatorio)
        {

            var relatorio = (from relatorioDB in db.cliente_relatorios
                             where relatorioDB.excluido == false && relatorioDB.idclienterelatorio == idRelatorio
                             select new Relatorio()
                             {
                                 nomeRelatorio = relatorioDB.nome,
                                 dataInicial = relatorioDB.dataini,
                                 dataFinal = relatorioDB.datafim
                             }).FirstOrDefault();

            relatorio.metricasFacebook = retornaMetricasRelatorioFacebook(idRelatorio);
            relatorio.metricasInstagram = retornaMetricasRelatorioInstagram(idRelatorio);
            relatorio.metricasTwitter = retornaMetricasRelatorioTwitter(idRelatorio);
            relatorio.metricasYoutube = retornaMetricasRelatorioYoutube(idRelatorio);
            relatorio.metricas = retornaMetricasRelatorio(idRelatorio);
            relatorio.empresas = db.cliente_relatorio_empresas.Where(x => x.idclienterelatorio == idRelatorio).Select(x => x.idempresa).ToList();


            return relatorio;
        }

        public ZRN.Mensagem.Mensagem Editar(Relatorio relatorio)
        {

            var resultado = validaRelatorio(relatorio);

            if (resultado.Codigo == 1)
            {

                try
                    {

                    var relatorioDB = (from r in db.cliente_relatorios
                                       where r.excluido == false && r.idclienterelatorio == relatorio.idClienteRelatorio
                                       select r).FirstOrDefault();

                    relatorioDB.nome = relatorio.nomeRelatorio;
                    relatorioDB.datahora = DateTime.Now;
                    relatorioDB.dataini = relatorio.dataInicial.Value;
                    relatorioDB.datafim = relatorio.dataFinal.Value;
                    db.SaveChanges();

                    //EMPRESAS RELATORIO

                    var empresasRelatorioDB = db.cliente_relatorio_empresas.Where(x => x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x.idempresa).ToList();

                    var adicionarEmpresas = relatorio.empresas.Except(empresasRelatorioDB);
                    var removerEmpresas = empresasRelatorioDB.Except(relatorio.empresas);

                    if (adicionarEmpresas.Any())
                    {
                        foreach (var empresa in adicionarEmpresas)
                        {
                            var empresaDB = new ZBD.Model.cliente_relatorio_empresas();
                            empresaDB.idclienterelatorio = relatorio.idClienteRelatorio;
                            empresaDB.idempresa = empresa;
                            db.cliente_relatorio_empresas.Add(empresaDB);
                            db.SaveChanges();
                        }
                    }
                    if (removerEmpresas.Any())
                    {
                        foreach (var empresa in removerEmpresas)
                        {
                            var empresaDB = db.cliente_relatorio_empresas.Where(x => x.idclienterelatorio == relatorio.idClienteRelatorio && x.idempresa == empresa).Select(x => x).FirstOrDefault();
                            db.cliente_relatorio_empresas.Remove(empresaDB);
                            db.SaveChanges();
                        }
                    }


                    //METRICAS REDES SOCIAS

                    //var MetricasFacebookBD = retornaMetricasRelatorioFacebook(relatorio.idClienteRelatorio);
                    //var MetricasTwitterBD = retornaMetricasRelatorioTwitter(relatorio.idClienteRelatorio);
                    //var MetricasInstagramBD = retornaMetricasRelatorioInstagram(relatorio.idClienteRelatorio);
                    //var MetricasYoutubeBD = retornaMetricasRelatorioYoutube(relatorio.idClienteRelatorio);


                    //var adicionarMetricasFacebook = relatorio.metricasFacebook.Except(MetricasFacebookBD);
                    //var removerMetricasFacebook = MetricasFacebookBD.Except(relatorio.metricasFacebook);

                    //if (adicionarMetricasFacebook.Any())
                    //{
                    //    foreach (var metricaFacebook in adicionarMetricasFacebook)
                    //    {
                    //        var metricaFacebookDB = new ZBD.Model.cliente_relatorio_metricas();
                    //        metricaFacebookDB.campo = metricaFacebook;
                    //        metricaFacebookDB.idclienterelatorio = relatorio.idClienteRelatorio;
                    //        db.cliente_relatorio_metricas.Add(metricaFacebookDB);
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (removerMetricasFacebook.Any())
                    //{
                    //    foreach (var metricaFacebook in removerMetricasFacebook)
                    //    {
                    //        var metricaFacebookDB = db.cliente_relatorio_metricas.Where(x => x.campo.ToLower().Contains(metricaFacebook.ToLower()) && x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x).FirstOrDefault();
                    //        db.cliente_relatorio_metricas.Remove(metricaFacebookDB);
                    //        db.SaveChanges();
                    //    }
                    //}

                    //var adicionarMetricasTwitter = relatorio.metricasTwitter.Except(MetricasTwitterBD);
                    //var removerMetricasTwitter = MetricasTwitterBD.Except(relatorio.metricasTwitter);

                    //if (adicionarMetricasTwitter.Any())
                    //{
                    //    foreach (var metricaTwitter in adicionarMetricasTwitter)
                    //    {
                    //        var metricaTwitterDB = new ZBD.Model.cliente_relatorio_metricas();
                    //        metricaTwitterDB.campo = metricaTwitter;
                    //        metricaTwitterDB.idclienterelatorio = relatorio.idClienteRelatorio;
                    //        db.cliente_relatorio_metricas.Add(metricaTwitterDB);
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (removerMetricasTwitter.Any())
                    //{
                    //    foreach (var metricaTwitter in removerMetricasTwitter)
                    //    {
                    //        var metricaTwitterDB = db.cliente_relatorio_metricas.Where(x => x.campo.ToLower().Contains(metricaTwitter.ToLower()) && x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x).FirstOrDefault();
                    //        db.cliente_relatorio_metricas.Remove(metricaTwitterDB);
                    //        db.SaveChanges();
                    //    }
                    //}

                    //var adicionarMetricasInstagram = relatorio.metricasInstagram.Except(MetricasInstagramBD);
                    //var removerMetricasInstagram = MetricasInstagramBD.Except(relatorio.metricasInstagram);

                    //if (adicionarMetricasInstagram.Any())
                    //{
                    //    foreach (var metricaInstagram in adicionarMetricasInstagram)
                    //    {
                    //        var metricaInstagramDB = new ZBD.Model.cliente_relatorio_metricas();
                    //        metricaInstagramDB.campo = metricaInstagram;
                    //        metricaInstagramDB.idclienterelatorio = relatorio.idClienteRelatorio;
                    //        db.cliente_relatorio_metricas.Add(metricaInstagramDB);
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (removerMetricasInstagram.Any())
                    //{
                    //    foreach (var metricaInstagram in removerMetricasInstagram)
                    //    {
                    //        var metricaInstagramDB = db.cliente_relatorio_metricas.Where(x => x.campo.ToLower().Contains(metricaInstagram.ToLower()) && x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x).FirstOrDefault();
                    //        db.cliente_relatorio_metricas.Remove(metricaInstagramDB);
                    //        db.SaveChanges();
                    //    }
                    //}

                    //var adicionarMetricasYoutube = relatorio.metricasYoutube.Except(MetricasYoutubeBD);
                    //var removerMetricasYoutube = MetricasYoutubeBD.Except(relatorio.metricasYoutube);

                    //if (adicionarMetricasYoutube.Any())
                    //{
                    //    foreach (var metricaYoutube in adicionarMetricasYoutube)
                    //    {
                    //        var metricaYoutubeDB = new ZBD.Model.cliente_relatorio_metricas();
                    //        metricaYoutubeDB.campo = metricaYoutube;
                    //        metricaYoutubeDB.idclienterelatorio = relatorio.idClienteRelatorio;
                    //        db.cliente_relatorio_metricas.Add(metricaYoutubeDB);
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (removerMetricasYoutube.Any())
                    //{
                    //    foreach (var metricaYoutube in removerMetricasYoutube)
                    //    {
                    //        var metricaYoutubeDB = db.cliente_relatorio_metricas.Where(x => x.campo.ToLower().Contains(metricaYoutube.ToLower()) && x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x).FirstOrDefault();
                    //        db.cliente_relatorio_metricas.Remove(metricaYoutubeDB);
                    //        db.SaveChanges();
                    //    }
                    //}



                    var MetricasBD = retornaMetricasRelatorio(relatorio.idClienteRelatorio);

                    var adicionarMetricas = relatorio.metricas.Except(MetricasBD);
                    var removerMetricas = MetricasBD.Except(relatorio.metricasFacebook);

                    if (adicionarMetricas.Any())
                    {
                        foreach (var metrica in adicionarMetricas)
                        {
                            var metricaDB = new ZBD.Model.cliente_relatorio_metricas();
                            metricaDB.campo = metrica;
                            metricaDB.idclienterelatorio = relatorio.idClienteRelatorio;
                            db.cliente_relatorio_metricas.Add(metricaDB);
                            db.SaveChanges();
                        }
                    }
                    if (removerMetricas.Any())
                    {
                        foreach (var metrica in removerMetricas)
                        {
                            var metricaDB = db.cliente_relatorio_metricas.Where(x => x.campo.ToLower().Contains(metrica.ToLower()) && x.idclienterelatorio == relatorio.idClienteRelatorio).Select(x => x).FirstOrDefault();
                            db.cliente_relatorio_metricas.Remove(metricaDB);
                            db.SaveChanges();
                        }
                    }

                    return resultado;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new Mensagem.Mensagem() { Codigo = 0, Texto = "erro", Tipo = Mensagem.Mensagem.tipoMensagem.Erro }; ;
                }

            }
            else
            {
                return resultado;
            }
        }

        private ZRN.Mensagem.Mensagem validaRelatorio(Relatorio relatorio)
        {

            var nomeRelatorioDB = db.cliente_relatorios.Where(x => x.nome.ToLower().Equals(relatorio.nomeRelatorio.ToLower()) && x.idcliente == relatorio.idCliente && x.idclienterelatorio != relatorio.idClienteRelatorio && x.excluido == false).Select(x => x);

            if (nomeRelatorioDB.Any())
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Nome do relatório já cadastrado!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            if (relatorio.dataInicial == null || relatorio.dataFinal == null)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Selecionar datas para o período de comparação!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            var dias = (relatorio.dataFinal - relatorio.dataInicial).Value.TotalDays;
            if (dias > 30)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "O intervalo entre as datas não deve ser maior que 30 dias.", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            if (relatorio.empresas.Count == 0 || relatorio.empresas == null)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Pelo menos uma empresa deve ser selecionada.", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }


            var clienteAcessoTotal = db.clientes.Where(x => x.idclientes == relatorio.idCliente).Select(x => x.vertodasempresas.Value).FirstOrDefault();

            if (!clienteAcessoTotal)
            {

                var empresasClienteDB = db.cliente_empresas
                                 .Where(x => x.idcliente == relatorio.idCliente && x.excluido == false)
                                   .Select(x => x.idempresa.Value).ToList();

                var resultado = relatorio.empresas.Except(empresasClienteDB);

                if (resultado.Any())
                {
                    return new Mensagem.Mensagem() { Codigo = 0, Texto = "Erro: acesso de empresas!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
                }

            }

            var resultadoCount = relatorio.metricas.Count;
            if (resultadoCount == 0)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Pelo menos uma métrica deve ser selecionada!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            return new Mensagem.Mensagem() { Codigo = 1, Texto = "ok", Tipo = Mensagem.Mensagem.tipoMensagem.Sucesso }; ;
        }
        
        public List<string> retornaMetricasRelatorioFacebook(int idRelatorio)
        {
            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio &&
                                    metrica.campo.ToLower().Contains("facebook")
                             select metrica.campo).Distinct().ToList();
            return resultado;
        }

        public ListaRelatorio RetornaRelatorios(FiltroRelatorio filtro)
        {
            DateTime? dataInicial = null;
            DateTime? dataFinal = null;

            if(filtro.dataInicial != null)
                dataInicial = new DateTime(filtro.dataInicial.Value.Year, filtro.dataInicial.Value.Month, filtro.dataInicial.Value.Day, 0, 0, 0);

            if(filtro.dataFinal != null)
                dataFinal = new DateTime(filtro.dataFinal.Value.Year, filtro.dataFinal.Value.Month, filtro.dataFinal.Value.Day, 23, 59, 59);


            var lista = (
                            from r in db.cliente_relatorios
                            join u in db.usuarios on r.idusuario equals u.idusuario
                            where r.idcliente == filtro.idCliente &&
                                r.excluido == false &&
                                (
                                    filtro.pesquisa != "" ?
                                        r.nome.ToLower().Contains(filtro.pesquisa.ToLower()) ||
                                        u.login.ToLower().Contains(filtro.pesquisa.ToLower()) ||
                                        u.nome.ToLower().Contains(filtro.pesquisa.ToLower())
                                        :
                                        true
                                )
                            select new Relatorio()
                            {
                                dataFinal = r.datafim,
                                dataInicial = r.dataini,
                                idClienteRelatorio = r.idclienterelatorio,
                                nomeRelatorio = r.nome,
                                nomeUsuario = u.nome
                            }
                );

            if (dataInicial == null && dataFinal != null)
            {
                lista = lista.Where(r => r.dataFinal <= dataFinal);
            }
            else if (dataFinal == null && dataInicial != null)
            {
                lista = lista.Where(r => r.dataInicial >= dataInicial);
            }
            else if(dataFinal != null && dataInicial != null)
            {
                lista = lista.Where(r => (
                                            dataInicial.Value >= r.dataInicial && dataInicial.Value <= r.dataFinal
                                         )
                                         ||
                                         (
                                            dataFinal.Value >= r.dataInicial && dataFinal.Value <= r.dataFinal
                                         )
                                         ||
                                         (
                                            dataInicial.Value <= r.dataInicial && dataFinal.Value >= r.dataFinal
                                         )
                                    );
            }



            var listaDados = new ListaRelatorio();
            listaDados.totalRegistros = lista.Count();
            listaDados.relatorios = lista.OrderByDescending(x => x.dataInicial).Skip((filtro.pagina - 1) * filtro.qtdeRegistros).Take(filtro.qtdeRegistros).ToList();

            return listaDados;
        }

        public List<string> VerificaMetricasRelatorio(int idRelatorio)
        {
            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio
                             select metrica.campo.ToString()).ToList();

            return resultado.Any() ? resultado : new List<string>();
        }

        public List<metricasTwitter> RetornaMetricasTwitter(int idRelatorio)
        {

            var relatorio = Relatorio(idRelatorio);

            if (relatorio != null)
            {

                string dataInicial = relatorio.dataInicial.Value.ToString("yyyy-MM-dd");
                string dataFinal = relatorio.dataFinal.Value.ToString("yyyy-MM-dd");
                string empresas = retornaEmpresasRelatorio(idRelatorio);

                string query = "CALL GetMetricasTwitter('" + empresas + "', '" + dataInicial + "', '" + dataFinal + "');";

                List<metricasTwitter> metricasTwitter = db.Database.SqlQuery<metricasTwitter>(query).ToList();

                return metricasTwitter;

            }
            else
            {
                return new List<metricasTwitter>();
            }


        }

        public List<string> retornaMetricasRelatorioTwitter(int idRelatorio)
        {

            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio &&
                                    metrica.campo.ToLower().Contains("twitter")
                             select metrica.campo).Distinct().ToList();

            return resultado;

        }

        public List<metricasYoutube> RetornaMetricasYoutTube(int idRelatorio)
        {

            var relatorio = Relatorio(idRelatorio);

            if (relatorio != null)
            {

                string dataInicial = relatorio.dataInicial.Value.ToString("yyyy-MM-dd");
                string dataFinal = relatorio.dataFinal.Value.ToString("yyyy-MM-dd");
                string empresas = retornaEmpresasRelatorio(idRelatorio);

                string query = "CALL GetMetricasYouTube('" + empresas + "', '" + dataInicial + "', '" + dataFinal + "');";

                List<metricasYoutube> metricasYoutube = db.Database.SqlQuery<metricasYoutube>(query).ToList();

                return metricasYoutube;

            }
            else
            {
                return new List<metricasYoutube>();
            }


        }

        public List<string> retornaMetricasRelatorioYoutube(int idRelatorio)
        {

            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio &&
                                    metrica.campo.ToLower().Contains("youtube")
                             select metrica.campo).Distinct().ToList();

            return resultado;

        }

        public List<string> retornaMetricasRelatorio(int idRelatorio)
        {

            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio
                             select metrica.campo).Distinct().ToList();

            return resultado;

        }

        public bool excluir(int idRelarorio)
        {

            try
            {
                var relatorio = db.cliente_relatorios.Where(x => x.idclienterelatorio == idRelarorio).FirstOrDefault();
                if (relatorio != null)
                {
                    relatorio.excluido = true;
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public string gerarCSV(int idRelatorio, string nomeRedeSocial)
        {

            string linhas = "";

            try
            {
                var csv = new StringBuilder();

                switch (nomeRedeSocial)
                {
                    case "facebook":

                        linhas = carregaLinhasCSVFacebook(RetornaMetricasFacebook(idRelatorio));

                        break;
                }

                //string linhas = string.Format("{0};{1}\n\r", "Rodrigo", "Prado");

                return linhas;


            }
            catch (Exception ex)
            {

                return "";
            }
        }

        public string carregaLinhasCSVFacebook(List<metricasFacebook> dados)
        {
            var info = dados;

            return null;
        }

        public Mensagem.Mensagem CriarRelatorio(CadastroRelatorio relatorio)
        {
            var resultado = validaRelatorioCadastro(relatorio);

            if (resultado.Codigo == 1)
            {
                var relatorioDB = new ZBD.Model.cliente_relatorios();

                relatorioDB.idcliente = relatorio.idCliente;
                relatorioDB.idusuario = relatorio.idUsuario;
                relatorioDB.nome = relatorio.nome;
                relatorioDB.datahora = DateTime.Now;
                relatorioDB.dataini = relatorio.dtInicio;
                relatorioDB.datafim = relatorio.dtFim;
                relatorioDB.excluido = false;

                db.cliente_relatorios.Add(relatorioDB);

                //Salvar relatorio_empresas
                foreach (var idempresa in relatorio.ListaEmpresas)
                {
                    var relatorioempDB = new ZBD.Model.cliente_relatorio_empresas();
                    relatorioempDB.idempresa = idempresa;
                    relatorioempDB.idclienterelatorio = relatorioDB.idclienterelatorio;

                    db.cliente_relatorio_empresas.Add(relatorioempDB);
                }

                //Salvar relatorio_metricas
                foreach (var metrica in relatorio.ListaMetricas)
                {
                    var relatoriometDB = new ZBD.Model.cliente_relatorio_metricas();
                    relatoriometDB.campo = metrica;
                    relatoriometDB.idclienterelatorio = relatorioDB.idclienterelatorio;

                    db.cliente_relatorio_metricas.Add(relatoriometDB);
                }

                db.SaveChanges();

                return new Mensagem.Mensagem() { Codigo = relatorioDB.idclienterelatorio, Texto = "Ok", Tipo = Mensagem.Mensagem.tipoMensagem.Sucesso };
            }
            else
            {
                return resultado;
            }
        }

        public List<metricasInstagram> RetornaMetricasInstagram(int idRelatorio)
        {

            var relatorio = Relatorio(idRelatorio);

            if (relatorio != null)
            {

                string dataInicial = relatorio.dataInicial.Value.ToString("yyyy-MM-dd");
                string dataFinal = relatorio.dataFinal.Value.ToString("yyyy-MM-dd");
                string empresas = retornaEmpresasRelatorio(idRelatorio);

                string query = "CALL GetMetricasInstagram('" + empresas + "', '" + dataInicial + "', '" + dataFinal + "');";

                List<metricasInstagram> metricasInstagram = db.Database.SqlQuery<metricasInstagram>(query).ToList();

                return metricasInstagram;

            }
            else
            {
                return new List<metricasInstagram>();
            }

        }

        public List<string> retornaMetricasRelatorioInstagram(int idRelatorio)
        {

            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio &&
                                    metrica.campo.ToLower().Contains("instagram")
                             select metrica.campo).Distinct().ToList();

            return resultado;

        }

        private ZRN.Mensagem.Mensagem validaRelatorioCadastro(CadastroRelatorio relatorio)
        {
            var nomeRelatorioDB = db.cliente_relatorios.Where(x => x.nome.ToLower().Equals(relatorio.nome.ToLower()) && x.idcliente == relatorio.idCliente).Select(x => x);

            if (nomeRelatorioDB.Any())
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Nome do relatório já cadastrado!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            if (relatorio.dtInicio == null || relatorio.dtFim == null)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Selecionar datas para o período de comparação!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            var dias = (relatorio.dtFim - relatorio.dtInicio).TotalDays;
            if (dias > 30)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "O intervalo de datas ultrapassou os 30 dias!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            if (relatorio.ListaEmpresas.Count == 0 || relatorio.ListaEmpresas == null)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Selecionar pelo menos uma empresa!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }


            var clienteAcessoTotal = db.clientes.Where(x => x.idclientes == relatorio.idCliente).Select(x => x.vertodasempresas.Value).FirstOrDefault();

            if (!clienteAcessoTotal)
            {

                var empresasClienteDB = db.cliente_empresas
                                 .Where(x => x.idcliente == relatorio.idCliente && x.excluido == false)
                                   .Select(x => x.idempresa.Value).ToList();

                var resultado = relatorio.ListaEmpresas.Except(empresasClienteDB);

                if (resultado.Any())
                {
                    return new Mensagem.Mensagem() { Codigo = 0, Texto = "Selecione pelo menos uma empresa!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
                }

            }

            var resultadoCount = relatorio.ListaMetricas.Count;

            if (resultadoCount == 0)
            {
                return new Mensagem.Mensagem() { Codigo = 0, Texto = "Selecionar pelo menos uma métrica!", Tipo = Mensagem.Mensagem.tipoMensagem.Erro };
            }

            return new Mensagem.Mensagem() { Codigo = 1, Texto = "Ok", Tipo = Mensagem.Mensagem.tipoMensagem.Sucesso }; ;
        }

        public List<string> retornaMetricasRelatorioPresencaOnlineNoticia(int idRelatorio)
        {
            var resultado = (from metrica in db.cliente_relatorio_metricas
                             where metrica.idclienterelatorio == idRelatorio &&
                                    ( metrica.campo.ToLower().Contains("presenca_") || metrica.campo.ToLower().Contains("noticias_")) 
                             select metrica.campo).Distinct().ToList();
            return resultado;
        }

        public List<metricasPresencaOnlineNoticias> RetornaMetricasPresencaOnlineNoticia(int idRelatorio)
        {
            var relatorio = Relatorio(idRelatorio);

            if (relatorio != null)
            {
                string empresas = retornaEmpresasRelatorio(idRelatorio);
                string dataInicial = relatorio.dataInicial.Value.ToString("yyyy-MM-dd");
                string dataFinal = relatorio.dataFinal.Value.ToString("yyyy-MM-dd");

                string query = "CALL GetMetricasPresencaOnlineNoticia('" + empresas + "', '" + dataInicial + "', '" + dataFinal + "');";

                List<metricasPresencaOnlineNoticias> metricasPresencaNoticia = db.Database.SqlQuery<metricasPresencaOnlineNoticias>(query).ToList();

                return metricasPresencaNoticia;

            }
            else
            {
                return null;
            }
                
        }


    }
}
