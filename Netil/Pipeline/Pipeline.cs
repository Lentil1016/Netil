using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Netil.Helper;

namespace Netil.Pipeline
{
    class pipeline
    {
        public pipeline()
        {
            Storage["Old_RootUrl"] = new List<string>();
        }

        public bool AddPipe(string PackName,Dictionary<string,Regex> RuleList,string PipeType)
        {
            if (NameList.Contains(PackName))
            {
                foreach (string NewPackName in RuleList.Keys)
                    if (!NameList.Contains(NewPackName))
                    {
                        NameList.Add(NewPackName);
                        Storage[NewPackName] = new List<string>();
                        Storage_Loker[NewPackName] = new object();
                    }
                _pipeline.Add(new UrlPipe(this, PackName, RuleList));
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 对指定RootUrl进行一次迭代，返回迭代过程中更新的RootUrl_List
        /// </summary>
        /// <param name="OperatedURL">要进行迭代的Url</param>
        /// <returns></returns>
        public List<string> Operating(string OperatedURL)
        {
            Storage["Old_RootUrl"].Add(OperatedURL);
            //operating

            return Storage["RootUrl"];
        }

        public static bool QueryUrl(string URL)
        {
            if (!B_Filter.Contains(URL))
                B_Filter.Add(URL);
            else
                return false;//exist
            return true;//not exist
        }

        private List<string> NameList = new List<string>();
        private Dictionary<string, List<string>> PagesCache = new Dictionary<string, List<string>>();//用于存放页面缓存
        private Dictionary<string, object> PagesCache_Loker = new Dictionary<string, object>();//页面缓存具名锁
        private Dictionary<string, List<string>> Storage = new Dictionary<string, List<string>>();//用于为PiplLine管理资源包
        private Dictionary<string, object> Storage_Loker = new Dictionary<string, object>();//资源包具名锁
        private List<Pipe> _pipeline;//管线List
        private static object BloomLoker = new object();
        private static BloomFilter<System.String> B_Filter = new BloomFilter<string>(100, 1000);//布隆过滤器
    }
}
