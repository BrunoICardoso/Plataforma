using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.LancamentoProdutos
{
    public class ItemFiltro
    {

        private string _valor;

        public string valor
        {
            get
            {
                if (string.IsNullOrEmpty(_valor))
                    return "Não especificado";
                else
                    return _valor;
            }
            set { _valor = value; }
        }


        private string _texto;

        public string texto
        {
            get
            {
                if (string.IsNullOrEmpty(_texto))
                    return "Não especificado";
                else
                    return _texto;
            }
            set { _texto = value; }
        }

    }
}
