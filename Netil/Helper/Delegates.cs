using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netil.Helper
{
    #region delegates
    delegate void request_return_handle();//Being used by pipeline class
    delegate void enqueue_handle(List<string> EnqueueOrders);//Being used by pipeline & pipe class
    delegate void enqueue_index_handle(string EnqueueKey, List<string> EnqueueOrders);//Being used by pipeline class
    #endregion
}
