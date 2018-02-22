using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Graficos.TagCloud
{
    public class tf
    {
        public long? iddocumento { get; set; }
        public string termo { get; set; }
        public double freqdoc { get; set; }
        public double totaltermos { get; set; }

        public double gettf
        {
            get
            {
                var tf = freqdoc / totaltermos;
                return tf;
            }
        }
    }
}
