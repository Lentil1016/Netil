using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Netil.Helper;
using System.IO;

namespace Netil.Pipeline
{
    class Pipe
    {
        #region constructor
        /// <summary>
        /// 创建一个流水线处理单元
        /// </summary>
        /// <param name="_RequestedPack">输入订单名</param>
        /// <param name="_OutputPack">输出订单名单</param>
        /// <param name="_CheckRules">检查网页内容是否符合预期</param>
        /// <param name="_MsgRules">从网页中抽取信息的规则</param>
        /// <param name="_FileRules">将信息组合保存成文件的规则</param>
        /// <param name="_DownloadRules">将信息识别为URL并下载到指定文件的规则</param>
        /// <param name="_WorkerCount">下载并行线程数</param>
        public Pipe(string _RequestedPack,
            List<string> _OutputPack,
            Dictionary<bool,Regex>  _CheckRules,
            Dictionary<string,Regex> _MsgRules,
            Dictionary<string,List<string>> _FileRules,
            Dictionary<string,string> _DownloadRules,
            int _WorkerCount)
        {
            RequestedPack=_RequestedPack;
            OutputPack = _OutputPack;
            CheckRules = _CheckRules;
            MsgRules = _MsgRules;
            FileRules = _FileRules;
            DownloadRules = _DownloadRules;
            WorkerCount = _WorkerCount;
        }
        #endregion

        #region method
        /// <summary>
        /// 使得Pipe的产出物能够被输出到Pileline，以进行后续的调度(多播委托)
        /// </summary>
        /// <param name="_EnqueueIndexHandle"></param>
        public void Online(enqueue_index_handle _EnqueueIndexHandle)
        {
            EnqueueIndexHandle = _EnqueueIndexHandle;//传入添加订单函数委托
        }

        public void proccessing()
        {
            Parallel.For(0, WorkerCount, (i) => Worker());
        }
        
        #endregion

        #region MT_method_多线程方法
        private void Worker()
        {
            string content,Url;
            Dictionary<string, List<string>> thread_stash = new Dictionary<string, List<string>>();//线程内缓存，每次while循环清空一次
            int counter;

            foreach (var rule in MsgRules)
                thread_stash[rule.Key] = new List<string>();//初始化

            while (!pipeline.CTS.IsCancellationRequested)//子线程结束标志判断
            {
                if(HttpHelper.GETResponse(Url=Dequeue(),out content))//提取一项Url并下载，锁OrderManager->解锁OrderManager
                {
                    Url=Path.GetDirectoryName(Url);
                    if (ContentCheck(content))//检查是否出现期待或不期待出现的内容
                    {
                        //爬取信息阶段
                        foreach(var Rule in MsgRules)//使用每条正则表达式对content进行匹配
                        {
                            foreach (Match match in Rule.Value.Matches(content))
                                thread_stash[Rule.Key].Add(HttpHelper.CheckUrl(match.Groups[Rule.Key].Value,Url));//将每条匹配中的目标数据存入stash
                        }

                        //下载文件阶段
                        Task FireAndIgnore;
                        foreach (var Rule in DownloadRules)
                            for (counter = 0; counter < thread_stash[Rule.Key].Count; counter++)
                                FireAndIgnore  = HttpHelper.DowloadAsBinary(thread_stash[Rule.Key][counter],//To avoid the CS4014 Warning http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
                                    StringHelper.FilenameParser(Rule.Value, counter, thread_stash)) ;

                        //保存数据表阶段

                    }
                }
            }
        }

        private bool ContentCheck(string content)
        {
            foreach (var CheckRule in CheckRules)
                if (!CheckRule.Key == CheckRule.Value.IsMatch(content))
                    return false;
            return true;
        }

        /// <summary>
        /// 由外部向订单缓冲区中加入订单
        /// </summary>
        /// <param name="Orders">欲加入缓冲区的订单</param>
        public void Enqueue(List<string> Orders)
        {
            lock (QueueLocker)
                OrderQueue.Concat(Orders);
        }

        /// <summary>
        /// 从订单缓冲区中取出订单
        /// </summary>
        /// <returns></returns>
        private string Dequeue()
        {
            lock (QueueLocker)
                return OrderQueue.Dequeue();
        }

        #endregion

        #region variables
        private object QueueLocker = new object();//订单队列锁
        private Queue<string> OrderQueue = new Queue<string>();//订单队列

        private int WorkerCount;//并行子线程数

        private string RequestedPack;//需求订单
        private List<string> OutputPack;//产出订单表
        private enqueue_index_handle EnqueueIndexHandle;//派送订单函数委托

        private Dictionary<bool, Regex> CheckRules;//key为逻辑标识符，ture为存在匹配时通过，false为不存在匹配时通过，value为匹配表达式
        private Dictionary<string, Regex> MsgRules;//key为数据包所在Group名，value为提取Group所用正则表达式
        private Dictionary<string, List<string>> FileRules;//key为文件保存路径文件名表达式，value为文件内包含数据包清单 
        private Dictionary<string, string> DownloadRules;//Key为Url数据包，value为保存路径文件名表达式

        private object StationLock=new object();
        private Dictionary<string, List<string>> DataStation=new Dictionary<string, List<string>>();//用于保存该pipe段本身产出的数据资源
        #endregion
    }
}
