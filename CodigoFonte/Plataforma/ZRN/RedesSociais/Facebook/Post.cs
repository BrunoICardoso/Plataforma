using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZRN.RedesSociais.Facebook
{
    public class Post
    {
        private DateTime Datapost; 

        public long idPost { get; set; }
        public DateTime dataPost{

            get {

                return Datapost;
            }

            set {

                Datapost = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()),"E. South America Standard Time");
            }
        }

        public string usuarioPost { get; set; }
        public string postagem { get; set; }
        public long? totalCurtidas { get; set; }
        public long? totalCompartilhamentos { get; set; }
        public long? totalComentarios { get; set; }
        public long? qtdlikes { get; set; }
        public long? engajamento { get; set; }
        public string nomeimagem { get; set; }
        public int? totalsemreposta { get; set; }
        public IEnumerable<Comentario> comentarios;

        public Imagem imagem { get; set; }
    }

    public class Imagem
    {
        public int altura
        {
            get
            {
                try
                {
                    var path = HttpContext.Current.Server.MapPath(caminho);

                    if (string.IsNullOrEmpty(path))
                        return 0;

                    if (File.Exists(path))
                    {
                        Image img = Image.FromFile(path);
                        return img.Height;
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {

                    return 0;
                }

            }

        }
        public int largura
        {
            get
            {
                try
                {
                    var path = HttpContext.Current.Server.MapPath(caminho);

                    if (string.IsNullOrEmpty(path))
                        return 0;

                    if (File.Exists(path))
                    {
                        Image img = Image.FromFile(path);
                        return img.Width;
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {

                    return 0;
                }

            }
        }
        public string caminho { get; set; }
    }
}
