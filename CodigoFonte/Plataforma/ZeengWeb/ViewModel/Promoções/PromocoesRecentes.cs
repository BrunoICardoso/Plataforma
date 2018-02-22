using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeengWeb.ViewModel.Promoções
{
    public class PromocoesRecentes
    {
        public int idPromocao { get; set; }
        public string nomePromocao { get; set; }
        public string modalidade { get; set; }
        public string abrangencia { get; set; }
        public string orgaoRegulador { get; set; }
        public string numeroRegistro { get; set; }
        public DateTime dataVigenciaInicial { get; set; }
        public DateTime dataVigenciaFinal { get; set; }        
        public DateTime dataCadastro { get; set; }
        public List<Empresas> empresas { get; set; }
        public List<Estados> abrangestados { get; set; }
        public List<Municipios> abrangmunicipios { get; set; }
    }

    public class Empresas
    {
        public int idempresa { get; set; }
        public string nome { get; set; }
    }

    public class Estados
    {
        public int idestado { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
    }

    public class Municipios
    {
        public int idmunicipio { get; set; }
        public int idestado { get; set; }
        public string uf { get; set; }
        public string nome { get; set; }
    }
}