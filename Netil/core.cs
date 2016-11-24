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
    public class core
    {
        #region Constructor

        #endregion
        #region Method
        

        public Regex CreatRule(string Pre_Feature, string GroupName, string Post_Feature)
        {
            StringHelper.Pre_Proc(Pre_Feature);
            StringHelper.Pre_Proc(GroupName);
            StringHelper.Pre_Proc(Post_Feature);
            return new Regex(Pre_Feature + "(?<" + GroupName + ">.*?)" + Post_Feature);
        }

        #endregion

        #region variables
        private pipeline Mainpipline = new pipeline();
        private readonly object _QueueInUsed = new object();
        private Queue<Uri> UrlsQueue = new Queue<Uri>();
        private HttpHelper http = new HttpHelper();
        #endregion
    }
}
