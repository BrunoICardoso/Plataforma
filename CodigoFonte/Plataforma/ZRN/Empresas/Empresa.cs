using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Empresas
{
    public class Empresa
    {
        public int idempresa { get; set; }
        public int idsetor { get; set; }
        public string nomeSetor { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string urlsite { get; set; }
        public string imagem { get; set; }
        public int qtdLancamentos { get; set; }
        public long qtdInteracoes { get; set; }
        public long qtdPublicacoes { get; set; }
        public long qtdPromocoes { get; set; }
        public int qtdNoticias { get; set; }
        public int? rankBrasil { get; set; }
        public IEnumerable<Marca.Marca> marcas { get; set; }
        public IEnumerable<RedesSociais.RedeSocial> redesesociais { get; set; }
        public string urlFacebook { get; set; }
        public string urlTwitter { get; set; }
        public string urlIntagram { get; set; }
        public string urlYoutube { get; set; }
        public Clientes.ClienteVertentes VertenteClientes { get; set; }
        public bool clienteAssociado { get; set; }
    }
}
