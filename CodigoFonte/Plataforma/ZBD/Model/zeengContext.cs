using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBD.Model
{
    public partial class zeengContext:DbContext
    {
        public zeengContext() : base("name=zeengEntities")
        {
            ((IObjectContextAdapter)(this)).ObjectContext.CommandTimeout = 1200;

        }
    }
}
