using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{

    //[ElasticsearchType(Name= "promocao")]
    public class Processos_Seae
    {
        private string Comoparticipar;
        private string Resumo;
        private string Premios;

        public int idprocesso { get; set; }
        public string nome { get; set; }
        public string numprocesso { get; set; }
        public int numdocs { get; set; }
        public string dtprocesso { get; set; }
        public string dtvigenciaini { get; set; }
        public string dtvigenciafim { get; set; }
        public string situacaoatual { get; set; }
        public bool abrangencia_nacional { get; set; }
        public double valortotalpremios { get; set; }
        public string modalidade { get; set; }
        public string formacontemplacao { get; set; }
        public string interessados { get; set; }

        public string resumo {

            get {

                return Resumo.Replace("\n", "<br>");
                }

            set
                {

                Resumo = value;
                    
                }

        }


        public string comoparticipar
        {
            get
            {
                return Comoparticipar.Replace("\n", "<br>");
            }
            set
            {
                Comoparticipar = value;
            }
        }


        public string premios
        {
            get
            {
                return Premios.Replace("\n", "<br>");
            }
            set
            {
                 Premios = value;
            }
        }
        public List<Abrangestados> abrangestados { get; set; }
        public List<Abrangmunicipios> abrangmunicipios { get; set; }
        public List<Arquivos> arquivos { get; set; }
        public List<Empresas> empresas { get; set; }
        public List<Setores> setores { get; set; }
    }



    public class Empresas
    {
            public int idempresa { get; set; }
            public string nome { get; set; }
    }

    public class Abrangestados
    {
        public int idestado { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }

    }

    public class Abrangmunicipios
    {
        public int idmunicipio { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }

    }


    public class Arquivos
    {
            public string coordenacao { get; set; }
            public int idarquivo { get; set; }
            public string link { get; set; }
            public string nomearquivo { get; set; }
            public string numdoc { get; set; }
            public string situacao { get; set; }
            public string textoarquivo { get; set; }
    }


    public class Setores
    {
        public string codsetor { get; set; }
        public string codsubsetor { get; set; }
        public string descsetor { get; set; }
        public string descsubsetor { get; set; }
        public int idsetor { get; set; }
        public int idsubsetor { get; set; }

    }


    
}
