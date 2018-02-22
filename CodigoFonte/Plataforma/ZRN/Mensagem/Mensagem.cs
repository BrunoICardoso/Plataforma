using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Mensagem
{
    public class Mensagem
    {
        public enum tipoMensagem
        {
            Erro, 
            Alerta,
            Informativa, 
            Sucesso
        }

        private string _texto;

        public string Texto
        {
            get { return _texto; }
            set { _texto = value; }
        }

        public string MensagemException { get; set; }

        private int _codigo;

        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        private tipoMensagem _tipo;

        public tipoMensagem Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public Mensagem()
        {

        }

        public Mensagem(string texto)
        {
            _texto = texto;
        }

        public Mensagem(string texto, tipoMensagem  tipo)
        {
            _texto = texto;
            _tipo = tipo;
        }

        public Mensagem(string texto, tipoMensagem tipo, int codigo)
        {
            _texto = texto;
            _tipo = tipo;
            _codigo = codigo;
        }

    }
}
