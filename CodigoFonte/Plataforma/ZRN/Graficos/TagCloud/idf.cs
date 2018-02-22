using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Graficos.TagCloud
{
    public class idf
    {
        public double totaldocs { get; set; }

        public double freqdocs { get; set; }

        public double getidf
        {
            get
            {
                var idf = Math.Log10(totaldocs / freqdocs);
                return idf;
            }
        }
    }
}
