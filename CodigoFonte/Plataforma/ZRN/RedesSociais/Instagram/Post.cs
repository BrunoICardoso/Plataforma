using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.IO;

namespace ZRN.RedesSociais.Instagram
{
    public class Post
    {
        private DateTime DataPost;

        public long idPost { get; set; }
        public DateTime dataPost {
            get
            {

                return DataPost;
            }

            set
            {

                DataPost = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(value.ToString()), "E. South America Standard Time");
            }
        }

        public long? totalComentarios { get; set; }
        public long? totalCurtidas { get; set; }
        public string postagem { get; set; }
        public string urlImagem { get; set; }
        public string nomeImagem { get; set; }
        public string usuarioPost { get; set; }
        public int? totalsemrespostaInsta { get; set; }
        public IEnumerable<Comentario> comentarios { get; set; }
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
