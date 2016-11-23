using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading;

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
        public static bool GETResponse(string URL,out string content)
        {
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(URL);
            //Request.CookieContainer = Cookies;//TODO: MT_CookieContainer
            Request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            Request.Accept = "text/html, application/xhtml+xml, */*";
            Request.Timeout = 2000;
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "GET";
            
            bool flag = false;//标志content是否获取成功
            int retry = 0;
            content = "";
            while (retry<3)
            {
                try
                {
                    Console.WriteLine("重试" + retry);
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)Request.GetResponse()).GetResponseStream(), System.Text.Encoding.UTF8))
                        content = sr.ReadToEnd();
                    flag = true;//代码执行到这里意味着content获取成功
                }
                catch (WebException)//响应超时，重试两次后放弃。
                {
                    retry++;
                    Request.Timeout *= 2;//响应时间翻倍
                }
            }
            return flag;
        }

        /// <summary>
        /// TODO：以Stream返回向URL POST消息所得到的响应
        /// </summary>
        /// <param name="URL">发送请求的目标地址</param>
        /// <param name="PostString">String类型的POST消息</param>
        /// <returns></returns>
        public static Stream POSTRequest(Uri URL,string PostString)
        {
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(URL);
            //Request.CookieContainer = Cookies;
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
                /*if(IsCookiesSaved)
                    foreach (Cookie cookie in Response.Cookies)
                        Cookies.SetCookies(Host, ("" + cookie.Name + "=" + cookie.Value));*/
                return Response.GetResponseStream();
            }
        }

        /// <summary>
        /// 以二进制形式下载文件
        /// </summary>
        /// <param name="filename">保存文件的绝对路径</param>
        /// <param name="url">下载的地址</param>
        /// <returns>下载结果</returns>
        /// TODO：大文件可续传下载
        public static async Task DowloadAsBinary(string filename,string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    if (!File.Exists(filename))
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Timeout = 20000;
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)"; // 
                        request.AllowAutoRedirect = true;//是否允许302
                        WebResponse response =await request.GetResponseAsync();
                        Stream reader = response.GetResponseStream();

                        FileStream writer = new FileStream(filename+1, FileMode.OpenOrCreate, FileAccess.Write);
                        byte[] buff = new byte[512];

                        int c = 0; //实际读取的字节数
                        while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                        {
                            writer.Write(buff, 0, c);
                        }
                        writer.Close();
                        writer.Dispose();
                        reader.Close();
                        reader.Dispose();
                        response.Close();
                        File.Move(filename+1, filename);
                        InformHelper.SendMessage (filename+"下载完毕");
                    }

                    InformHelper.SendMessage( filename+"已下载过");
                }
                catch (Exception)
                {
                    InformHelper.SendMessage("下载错误：地址为" + url,InformHelper.colors.red);
                }
            }
            InformHelper.SendMessage("下载错误：地址为空", InformHelper.colors.red);
        }

        /// <summary>
        /// 将相对地址改为绝对地址存储
        /// </summary>
        /// <param name="target">要检查的地址</param>
        /// <param name="Url">当前目录</param>
        /// <returns></returns>
        public static string CheckUrl(string target,string Url)
        {
            if (Uri.IsWellFormedUriString(target, UriKind.Relative))
                target = Url +target;
            return target;
        }
        #endregion

        #region Attributes
        private bool IsCookiesSaved { get; set; } = false;//TODO
        #endregion
    }
}
