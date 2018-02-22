using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{
    public class Promocao
    {
        public int idPromocao { get; set; }
        public int idOrgaoRegulador { get; set; }
        public int idModalidade { get; set; }

        public string NomeOrgaoRegulador { get; set; }
        public string NomeModalidade { get; set; }

        public string nomePromocao { get; set; }
        public string CertificadoAutorizacao { get; set; }
        public string OutrosInteressados { get; set; }

        public string LinkSitePromocao { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkInstagram { get; set; }
        public string LinkTwitter { get; set; }
        public string LinkYoutube { get; set; }
        public string LinkRegulamento { get; set; }

        public string Mecanica { get; set; }
        public string ProdutosParticipantes { get; set; }
        public string Premios { get; set; }
        public string TextoRegulamento { get; set; }
        
        public Abrangencia Abrangecia;

        public DateTime dtCadastro { get; set; }
        public DateTime dtvigenciaini { get; set; }
        public DateTime dtvigenciafim { get; set; }

        public Decimal ValorPremios { get; set; }

        public List<RedesSociais.Facebook.Post> FacebookPosts { get; set; }
        public List<RedesSociais.Instagram.Post> InstagramPosts { get; set; }
        public List<RedesSociais.Twitter.Post> TwitterPosts { get; set; }
        public List<RedesSociais.Youtube.Video> YoutubePosts { get; set; }
        public List<NoticiasElastic.NoticiaElastic> Noticias { get; set; }
        public List<ZRN.Promocoes.Empresas> empresas { get; set; }
        public List<Arquivo> arquivos { get; set; }
        public List<Arquivo> arquivosrelacionados { get; set; }
        public List<Arquivo> arquivosregulamento { get; set; }
    }

    public class Abrangencia
    {

        public Abrangencia(bool nacional) : this(nacional, null, null)
        {

        }

        public Abrangencia(bool nacional, List<Estados.Estado> estados, List<Municipios.Municipio> municipios)
        {
            this.Nacional = nacional;
            this.Estados = estados;
            this.Municipios = municipios;

        }

        private bool _nacional = false;
        public bool Nacional
        {
            get { return _nacional; }
            set { _nacional = value; }
        }


        private List<Estados.Estado> _estados;
        public List<Estados.Estado> Estados
        {
            get
            {
                return _estados;
            }
            set
            {
                //if (Nacional)
                //{

                //    throw new Exception("Não pode definir estados em uma abrangência nacional");
                //}

                _estados = value;

            }
        }
        public List<Municipios.Municipio> Municipios { get; set; }
    }
    
    public class Arquivo{
      
       
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
        public string Tipo { get; set; }        
    }

    //public class PromocaoElastic
    //{
    //    public int idpromocao { get; set; }
    //    public int nome { get; set; }
    //}    
}
