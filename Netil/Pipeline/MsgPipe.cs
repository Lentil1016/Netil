using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Netil.Pipeline
{
    class MsgPipe:Pipe
    {
        public MsgPipe(List<Uri> _UrlList, Dictionary<string, List<string>> _MsgStore, Dictionary<string, Regex> _RuleList)
        {
            UrlList = _UrlList;
            MsgStore = _MsgStore;
            RuleList = _RuleList;
        }

        private Dictionary<string, List<string>> MsgStore;
        private Dictionary<string, Regex> RuleList;
    }
}
