using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Netil.Helper;
using System.IO;

namespace Netil.Pipeline
{
    class UrlPipe:Pipe
    {
        #region constructor
        public UrlPipe(pipeline Parente_Pipeline,string _PackName,Dictionary<string, Regex> _RuleList)
        {
            Parent = Parente_Pipeline;
            PackName = _PackName;
            RuleList = _RuleList;
        }
        #endregion

        #region method
        public void Producting()
        {
            var Product_Dict = new Dictionary<string, List<string>>();
            foreach(var rule in RuleList)
            {
                Product_Dict[rule.Key] = new List<string>();
                foreach (var order in Parent.)
                {
                    var matches = rule.Value.Matches(order.value);
                    foreach (Match match in matches)
                        if (UrlCheck(Path.GetDirectoryName(order.Key.ToString()), match.Groups[rule.Key].Value))
                            Product_Dict[rule.Key].Add(CheckedUrl);
                        else
                            InformHelper.SendMessage("在" + order.Key + "爬取包" + rule.Key + "失败，请对应" + rule.Key + "包的匹配规则检查前述链接地址源码");
                }
            }
        }
        #endregion

        #region private_method
        private bool UrlCheck(string directory, string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Relative))
                if (Uri.IsWellFormedUriString(CheckedUrl = directory + url, UriKind.Absolute))
                    return true;
                else
                    return false;
            else if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                CheckedUrl = url;
                return true;
            }
            else
                return false;
        }
        #endregion

        #region variables
        private int OrderCount;
        private int CacheCount;
        private string CheckedUrl;
        private string PackName;
        private pipeline Parent;
        private Dictionary<string, Regex> RuleList;
        #endregion
    }
}
