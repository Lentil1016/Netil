using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using Netil.Helper;
using Netil.Pipeline;

namespace Netil
{
    class core
    {
        #region Constructor

        #endregion
        #region Method

        /// <summary>
        /// 将List中的Urls排入处理队列
        /// </summary>
        /// <param name="Urls">装载Url的List</param>
        public void QueueUrls(List<Uri> Urls)
        {
            lock (_QueueInUsed)
                foreach (Uri URL in Urls)
                {
                    if (pipeline.QueryUrl(URL))
                        UrlsQueue.Enqueue(URL);
                }
        }

        public Regex CreatRule(string Pre_Feature, string GroupName, string Post_Feature)
        {
            StrHelper.Pre_Proc(Pre_Feature);
            StrHelper.Pre_Proc(GroupName);
            StrHelper.Pre_Proc(Post_Feature);
            return new Regex(Pre_Feature + "(?<" + GroupName + ">.*?)" + Post_Feature);
        }
        public List<string> QueryGroups()
        {
            pipeline.
        }
        #endregion

        #region variables
        private pipeline Mainpipline = new pipeline();
        private StringHelper StrHelper = new StringHelper();
        private readonly object _QueueInUsed = new object();
        private Queue<Uri> UrlsQueue = new Queue<Uri>();
        private HttpHelper http = new HttpHelper();
        #endregion
    }
}
