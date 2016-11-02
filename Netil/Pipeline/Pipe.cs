using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netil.Pipeline
{
    class Pipe
    {
        public virtual void p()
        {
        }
        protected List<Uri> UrlList = new List<Uri>();
    }
}
