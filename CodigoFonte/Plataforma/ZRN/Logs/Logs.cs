using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Logs
{
    public class Logs
    {

        private ZBD.Model.zeengEntities bd = new ZBD.Model.zeengEntities();

        public void GravarLog(Log log)
        {
            var logBD = new ZBD.Model.logs();
            logBD.datahora = log.datahora;
            logBD.controle = log.controle;
            logBD.acao = log.acao;
            logBD.descricao = log.descricao;
            logBD.url = log.url;
            logBD.idusuario = log.idusuario;
            logBD.nivel = log.nivel;

            bd.logs.Add(logBD);
            bd.SaveChanges();
        }

    }
}
