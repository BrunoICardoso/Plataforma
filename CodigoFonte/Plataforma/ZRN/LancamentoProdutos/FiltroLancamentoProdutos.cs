using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.LancamentoProdutos
{
    public class ItensFiltroLancamentoProdutos
    {
        public List<ItemFiltro> Estados { get; set; }
        public List<ItemFiltro> Areas { get; set; }
        public List<ItemFiltro> Especies { get; set; }
        public List<ItemFiltro> Subespecies { get; set; }
        public List<ItemFiltro> Bases { get; set; }
        public List<ItemFiltro> Caracteristicas { get; set; }
        public List<ItemFiltro> Atributos { get; set; }
        public List<ItemFiltro> Complementos { get; set; }
        public List<ItemFiltro> Origens { get; set; }
        public List<ItemFiltro> Marcas { get; set; }
        public List<ItemFiltro> Anos { get; set; }
        public List<ItemFiltro> Status { get; set; }
        public List<ItemFiltro> Produtos { get; set; }
       
       


    }

    public class FiltroLancamentoProdutos
    {
        private int _regPorPagina = 10;
        public int regPorPagina
        {
            get { return _regPorPagina; }
            set { _regPorPagina = value; }
        }

        public int idEmpresa { get; set; }
        public int pagina { get; set; }
        public string NomeMarca { get; set; }
        public string NomeProduto { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
    }
}
