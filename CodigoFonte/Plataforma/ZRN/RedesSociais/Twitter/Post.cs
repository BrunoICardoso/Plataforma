using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.IO;

namespace ZRN.RedesSociais.Twitter
{
    public class Post
    {
        private DateTime DataPost;

        public string imagem { get; set; }
        public string usuarioPost { get; set; }
        public long? idPost { get; set; }
        public int? idPerfil{ get; set; }
        public string idTwitterPost { get; set; }
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


        public long? totalFavoritadas { get; set; }
        public long? totalRetweets { get; set; }
        public long? totalMencoes { get; set; }
        public long? totalReplies { get; set; }
        public string postagem { get; set; }
        public int? totalrespostas { get; set;}
        public IEnumerable<Post> postsReply;
        public ImagemTW imagemTw { get; set; }
        public IEnumerable<Comentario> comentarios;
    }


    public class ImagemTW
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

