using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZRN.Usuarios;

namespace ZeengWeb.Utils
{
    public static class Configuracoes
    {

        public static string retornaUrlAtual
        {
            get
            {
                var urlAtual = HttpContext.Current.Request.Url.AbsolutePath;
                    return urlAtual;
            }
        }
        
        public static string NomeUsuarioLogado{
            get {

                if (HttpContext.Current.Session["usuario"] == null)
                    return "";
                else
                    return ((Usuario)HttpContext.Current.Session["usuario"]).nome;

            }
        }

        public static int idUsuarioLogado
        {
            get
            {

                if (HttpContext.Current.Session["usuario"] == null)
                    return 0;
                else
                    return ((Usuario)HttpContext.Current.Session["usuario"]).idUsuario;

            }
        }

        public static int idClienteLogado
        {
            get
            {

                if (HttpContext.Current.Session["usuario"] == null)
                    return 0;
                else
                    return ((Usuario)HttpContext.Current.Session["usuario"]).idCliente;

            }
        }

        public static string emailUsuarioLogado
        {
            get
            {

                if (HttpContext.Current.Session["usuario"] == null)
                    return null;
                else
                    return ((Usuario)HttpContext.Current.Session["usuario"]).email;

            }
        }

        public static string loginUsuarioLogado
        {
            get
            {

                if (HttpContext.Current.Session["usuario"] == null)
                    return "";
                else
                    return ((Usuario)HttpContext.Current.Session["usuario"]).login;

            }
        }

        public static string DiretorioImagens
        {
            get
            {
                var diretorioImagens = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioimagens"];
                    if (customSetting != null)
                        diretorioImagens = customSetting.Value;

                }

                return diretorioImagens;
            }
        }

        public static string DiretorioArquivos
        {
            get
            {
                var diretorioarquivos = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["diretorioarquivos"];
                    if (customSetting != null)
                        diretorioarquivos = customSetting.Value;

                }

                return diretorioarquivos;
            }
        }

        public static string ServidorElastic
        {
            get
            {
                var servidorElastic = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["serverElastic"];
                    if (customSetting != null)
                        servidorElastic = customSetting.Value;

                }

                return servidorElastic;
            }
        }

        public static string IndexElastic
        {
            get
            {
                var indexElastic = "";

                System.Configuration.Configuration appWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                if (appWebConfig.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = appWebConfig.AppSettings.Settings["indexElastic"];
                    if (customSetting != null)
                        indexElastic = customSetting.Value;

                }

                return indexElastic;
            }
        }

        public static bool acessoVertenteRedeSocial
        {
            get
            {
                return (bool)(HttpContext.Current.Session["AcessoRedeSocial"]);
            }
        }
    }
}