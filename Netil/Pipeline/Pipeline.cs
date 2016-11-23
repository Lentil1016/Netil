using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;        
using Netil.Helper;
using System.Threading.Tasks;
using System.Threading;

/*pipeline主要负责调度工作(多播委托、布隆过滤)，而各段pipe只负责数据的下载和提取操作
 * 也即pipe就像工厂中的工位，只负责根据给定的规则进行数据生产
 * 而pipeline就像工厂中的物流系统，负责为pipe下达生产任务，并将pipe产出的数据资源进行再分配。
 * pipe通过queue接收订单，通过调用委托送回产品
*/

namespace Netil.Pipeline
{

    #region delegates
    delegate void enqueue_handle(List<string> EnqueueOrders);
    delegate void enqueue_index_handle(string EnqueueKey,List<string> EnqueueOrders );
    #endregion

    class pipeline
    {
        #region constructor
        public pipeline()
        {
            EnqueueDictHandle = new enqueue_index_handle(EnqueueOrders);
        }
        #endregion

        #region method
        /// <summary>
        /// 通过UI向Pipeline添加一个处理单元
        /// </summary>
        /// <param name="NewPipe">欲添加进pipeline的pipe</param>
        public void AddPipe(Pipe NewPipe)
        {
            NewPipe.Online(EnqueueDictHandle);//为传入的新Pipe添加订单分发函数的委托
            _pipeline.Add(NewPipe);
        }

        /// <summary>
        /// 构造多播委托
        /// </summary>
        /// <param name="RequestList">各个pipe所请求的订单名PackName</param>
        public void Pre_Operating(List<string> RequestList)
        {
            var DistList = RequestList.Distinct();//创建一个无重复版本的RequestList
            foreach (string Name in DistList)
                EnqueueHandlesDict[Name] = new enqueue_handle(PreDelegate);//添加多重委托前需要初始化委托，这里使用一个无用的空函数
            for (int i=0;i<RequestList.Count;i++)
                EnqueueHandlesDict[RequestList[i]] += _pipeline[i].Enqueue;//将RequestList中元素对应的Pipe的Dequeue方法添加进多重委托
            foreach (string Name in DistList)
                EnqueueHandlesDict[Name] -= PreDelegate;//将初始化构造用的空函数去掉
        }

        /// <summary>
        /// 传入Init表单，开始迭代生产
        /// </summary>
        /// <param name="OperatedURL">初始的Url列表</param>
        /// <returns></returns>
        public bool Operating(List<string> InitOrders)
        {
            EnqueueOrders("InitOrders", InitOrders);
            if (Parallel.ForEach(_pipeline, new ParallelOptions { CancellationToken = CTS.Token }, (pipe) => { pipe.proccessing(); }).IsCompleted)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 订单分发函数
        /// </summary>
        /// <param name="QueryKey">所要提取的订单名</param>
        /// <returns></returns>
        public void EnqueueOrders(string EnqueueKey,List<string> Orders)
        {
            EnqueueHandlesDict[EnqueueKey](Orders);
        }


        #endregion

        #region Private_method

        /// <summary>
        /// 仅用于初始化多重委托的空函数
        /// </summary>
        /// <param name="Trivial">如其名，无用</param>
        private void PreDelegate(List<string> Trivial)
        {
            //Do Nothing
        }

        /// <summary>
        /// 查询某Url是否已经在订单中排队或被处理
        /// </summary>
        /// <param name="URL">要查询的Url</param>
        /// <returns></returns>
        private bool QueryUrl(string URL)
        {
            if (!B_Filter.Contains(URL))
            {
                B_Filter.Add(URL);
                return true;//not exist
            }
            else
                return false;//exist

        }

        #endregion

        #region Attribute
        public static CancellationTokenSource CTS { get; } = new CancellationTokenSource();//全局子线程取消标识
        #endregion

        #region variables
        private enqueue_index_handle EnqueueDictHandle;//订单分发函数委托
        private Dictionary<string, enqueue_handle> EnqueueHandlesDict=new Dictionary<string, enqueue_handle>();//分发委托字典

        private List<Pipe> _pipeline=new List<Pipe>();//生产管线

        private object BloomLoker = new object();//布隆过滤器锁
        private BloomFilter<string> B_Filter = new BloomFilter<string>(100, 1000);//布隆过滤器
        #endregion
    }
}
