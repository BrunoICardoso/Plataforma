using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ZRN.PromocoesElastic
{

    [ElasticsearchType(Name="promocao")]
    public class PromocaoElastic
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

        public string Mecanicapromo { get; set; }
        public string ProdutosParticipantes { get; set; }
        public string PremiosPromo { get; set; }
        public string TextoRegulamento { get; set; }

        public bool abrangencia_nacional { get; set; }

        public List<Abrangmunicipio> abrangmunicipios { get; set; }
        public List<Abrangestados> abrangestados { get; set; }

        public DateTime dtCadastro { get; set; }
        public DateTime dtVigenciaIni { get; set; }
        public DateTime dtVigenciaFim { get; set; }

        public Decimal ValorPremios { get; set; }

        public List<Noticia> noticias { get; set; }

        public List<FacePost> postsfacebook { get; set; }
        public List<InstaPost> postsinstagram { get; set; }
        public List<TwPost> poststwitter { get; set; }
        public List<VideoYt> videosyoutube { get; set; }
        public List<Empresa> empresas { get; set; }
        public List<Arquivo> arquivosrelacionados { get; set; }
        public List<Arquivo> arquivosregulamento { get; set; }
    }

}

public class Abrangmunicipio
{
    public int idestado { get; set; }
    public int idmunicipio { get; set; }
    public string uf { get; set; }
    public string nome { get; set; }
}

public class Abrangestados
{
    public int idestado { get; set; }
    public string uf { get; set; }
    public string nome { get; set; }
}

public class FacePost
{
    public int compartilhamentos { get; set; }
    public int curtidas { get; set; }
    public string datahora { get; set; }
    public int idpost { get; set; }
    public string nomeimagem { get; set; }
    public string postagem { get; set; }
    public int qtdcomentarios { get; set; }
    public List<Promo> promocoes { get; set; }
}

public class InstaPost {
   
    public int curtidas { get; set; }
    public string datahora { get; set; }
    public int idpost { get; set; }
    public string nomeimagem { get; set; }
    public string postagem { get; set; }
    public int qtdcomentarios { get; set; }
    public List<Promo> promocoes { get; set; }

}

public class TwPost
{

    public int curtidas { get; set; }
    public string datahora { get; set; }
    public int idpost { get; set; }
    public string nomeimagem { get; set; }
    public string postagem { get; set; }
    public int retweets { get; set; }
    public List<Promo> promocoes { get; set; }

}

public class VideoYt
{

    public int curtidas { get; set; }
    public string datahora { get; set; }
    public string descricao { get; set; }
    public int idvideo { get; set; }
    public string nomeimagem { get; set; }
    public List<Promo> promocoes { get; set; }
    public int qtdcomentarios { get; set; }
    public int visualizacoes { get; set; }

}


public class Promo
{
    public int idpromocao { get; set; }
    public string nomepromocao { get; set; }
}

public class Empresa
{
    public int idempresa { get; set; }
    public string nome { get; set; }
}

public class Arquivo
{
    public string NomeArquivo { get; set; }
    public string Url { get; set; }
    public string Tipo { get; set; }
}