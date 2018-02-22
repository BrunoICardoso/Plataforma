using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Usuarios
{

    public class Usuario
    {
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string email { get; set; }
        public bool? ativo { get; set; }
        public bool? excluido { get; set; }
    }

}
