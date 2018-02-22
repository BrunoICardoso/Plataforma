using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Promocoes
{

    public class FiltroPromocoes
    {

        public int idEmpresa { get; set; }
        public List<int> idModalidades { get; set; }

        public int pagina { get; set; }
        public int qtdregistros { get; set; }

        public string pesquisa { get; set; }
        public string numproc { get; set; }

        /*
        public DateTime dataCadastroInicial { get; set; }
        public DateTime dataCadastroFinal { get; set; }

        public DateTime dataVigenciaInicial { get; set; }
        public DateTime dataVigenciaFinal { get; set; }
		*/
        public DateTime? dataCadastroInicial { get; set; }
        /// <summary>
        /// Formata a data conforme o padrão NEST. Caso o campo seja NULL, retorna a data de um ano atrás
        /// </summary>
        public string dataCadastroInicialNEST
        {
            get
            {
                return dataCadastroInicial.HasValue ? string.Format("{0:yyyy-MM-dd}", dataCadastroInicial) : string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(-1));
            }
        }

        public DateTime? dataCadastroFinal { get; set; }

        /// <summary>
        /// Formata a data conforme o padrão NEST. Caso o campo seja NULL, retorna a data atual
        /// </summary>
        public string dataCadastroFinalNEST
        {
            get
            {
                return dataCadastroFinal.HasValue ? string.Format("{0:yyyy-MM-dd}", dataCadastroFinal) : string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
        }

        public DateTime? dataVigenciaInicial { get; set; }
        /// <summary>
        /// Formata a data conforme o padrão NEST. Caso o campo seja NULL, retorna a data de 100 anos atrás
        /// </summary>
        public string dataVigenciaInicialNEST
        {
            get
            {
                return dataVigenciaInicial.HasValue ? string.Format("{0:yyyy-MM-dd}", dataVigenciaInicial) : string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(-100));
            }
        }

        public DateTime? dataVigenciaFinal { get; set; }

        /// <summary>
        /// Formata a data conforme o padrão NEST. Caso o campo seja NULL, retorna a data de 100 anos pra frente
        /// </summary>
        public string dataVigenciaFinalNEST
        {
            get
            {
                return dataVigenciaFinal.HasValue ? string.Format("{0:yyyy-MM-dd}", dataVigenciaFinal) : string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(100));
            }
        }


        public abrangencia abrangencia { get; set; }
        public ordenacao ordenacao { get; set; }
        
    }

    public class abrangencia
    {
        public bool? Nacional { get; set; }
        public List<int> idEstados { get; set; }
        //public List<int> idMunicipios { get; set; }
    }

    public class ordenacao
    {
        public string campo { get; set; }
        public orientacao ordem { get; set; }
    }

    public enum orientacao
    {
        Crescente = 0,
        Decrescente = 2
    }

}



