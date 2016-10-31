using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Netil.Helper
{
    class NetHelper
    {
        protected static HttpClient Client = new HttpClient();
    }

    class HttpHelper:NetHelper
    {
        HttpHelper(HttpClient Client)
        {
            this.Client = Client;
        }


        /// <summary>
        /// 返回URL指向文本为String
        /// </summary>
        /// <param name="URL">请求的地址</param>
        /// <returns></returns>
        public String GetRequest(String URL)
        {
            return Client.GetAsync(URL).Result.Content.ToString();
        }

        public 

        private HttpClient Client;
    }
}
