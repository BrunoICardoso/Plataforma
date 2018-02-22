using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Relatorios
{
    public class Relatorio
    {
        public int idClienteRelatorio { get; set; }
        public int idCliente { get; set; }
        public int idUsuario { get; set; }
        public string nome { get; set; }
        public DateTime? dataHora { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public bool excluido { get; set; }
		public string nomeRelatorio { get; set; }
        public string nomeUsuario { get; set; }
        public List<int> empresas { get; set; }
        public List<string> metricasFacebook { get; set; }
        public List<string> metricasTwitter { get; set; }
        public List<string> metricasInstagram { get; set; }
        public List<string> metricasYoutube { get; set; }
        public List<string> metricas { get; set; }
    }
    
    public class ListaRelatorio
    {
        public List<Relatorio> relatorios { get; set; }
        public int totalRegistros { get; set; }
    }
    
    public class CadastroRelatorio
    {
        public int idClienteRelatorio { get; set; }
        public int idCliente { get; set; }
        public int idUsuario { get; set; }
        public string nome { get; set; }
        public DateTime dtInicio { get; set; }
        public DateTime dtFim { get; set; }
        public List<int> ListaEmpresas { get; set; }
        public List<string> ListaMetricas { get; set; }
    }

}
