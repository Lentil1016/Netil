using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;

namespace Netil.Helper
{
    class HttpHelper
    {

        #region Methods

        public HttpHelper()
        {
        }

        /// <summary>
        /// 以String返回URL指向的文本文件
        /// </summary>
        /// <param name="URL">请求的地址</param>
        /// <returns></returns>
        public string GETResponse(Uri URL)
        {
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(URL);
            Request.CookieContainer = Cookies;
            Request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            Request.Accept = "text/html, application/xhtml+xml, */*";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "GET";

            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            using (Stream responsestream = Response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responsestream, System.Text.Encoding.UTF8))
                return sr.ReadToEnd();
        }
        /// <summary>
        /// 以Stream返回向URL POST消息所得到的响应
        /// </summary>
        /// <param name="URL">发送请求的目标地址</param>
        /// <param name="PostString">String类型的POST消息</param>
        /// <returns></returns>
        public Stream POSTRequest(Uri URL,string PostString)
        {
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(URL);
            Request.CookieContainer = Cookies;
            Request.Method = "POST";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            Request.Accept = "text/html, application/xhtml+xml, */*";
            Request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(PostString);
            Request.ContentLength = bytes.Length;
            Stream rqstStream = Request.GetRequestStream();
            rqstStream.Write(bytes, 0, bytes.Length);
            rqstStream.Close();
            Uri Host = new Uri(URL.Host);
            using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                if(IsCookiesSaved)
                    foreach (Cookie cookie in Response.Cookies)
                        Cookies.SetCookies(Host, ("" + cookie.Name + "=" + cookie.Value));
                return Response.GetResponseStream();
            }
        }
        #endregion

        #region Attributes
        private bool IsCookiesSaved { get; set; } = true;
        #endregion

        #region variables
        private CookieContainer Cookies;
        private HttpClient Client = new HttpClient();
        #endregion
    }
}
