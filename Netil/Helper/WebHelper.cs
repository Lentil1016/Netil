using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace Netil.Helper
{
        static class WebHelper
    {
        static private HttpClient Client=new HttpClient();
        static public string GetString(string Url)
        {
            return Client.GetStringAsync(Url).Result;
        }
        static public async Task<Stream> GetStream(string Url)
        {
            return await Client.GetStreamAsync(Url);
        }
        static public List<string> GetUrlsList(string Urls)
        {
            List<string> UrlList = new List<string>();
            Regex UrlRegex = new Regex("http(s)?://([^.\n]*\u002E)+[^\n]*");
            foreach (Match Url in UrlRegex.Matches(Urls))
                UrlList.Add(Url.Value);
            return UrlList;
        }
    }
}
