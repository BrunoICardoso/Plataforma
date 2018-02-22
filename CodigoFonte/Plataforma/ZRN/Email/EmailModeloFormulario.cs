using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Email
{
    public class EmailModeloFormulario
    {
        public int idUsuario { get; set; }
        public string urlAtual { get; set; }
        public string nomeUsuario { get; set; }
        public string emailUsuario { get; set; }
        public string nomeArquivo { get; set; }
        public string assunto { get; set; }
        public string mensagem { get; set; }
        public Stream anexo{ get; set; }
        public int tamanhoAnexo{ get; set; }

    }
}
