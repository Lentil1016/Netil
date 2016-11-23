using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Netil.Helper
{
    static class StringHelper
    {
        #region method
        /// <summary>
        /// 将文件路径中的":PackName"替换为Pack中的Msg
        /// </summary>
        /// <param name="path"></param>
        /// <param name="counter"></param>
        /// <param name="stash"></param>
        /// <returns></returns>
        public static string FilenameParser(string path,int counter,Dictionary<string,List<string>> stash)
        {
            foreach (Match match in PackRegex.Matches(path))
                path.Replace(match.Value, stash[match.Groups[1].Value][counter]);
            return path;
        }

        /// <summary>
        /// 字符串转化为正则表达式前的前处理
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <returns>处理完成的字符串</returns>
        public static string Pre_Proc(string str)
        {
            if (ConfigReg.IsMatch(str))
            {
                return ConfigReg.Match(str).Groups["RegExp"].Value;
            }
            else
            {
                var matches = ConfigStr.Matches(str) ;
                for (int counter=matches.Count-1;counter>=0;counter--)
                    str.Insert(matches[counter].Index,"\\");
                return str;
            }
        }
        #endregion

        #region variable
        private static Regex PackRegex = new Regex("\":(.*?)\"");
        private static Regex ConfigStr = new Regex(@"[^\s\d\w]");
        private static Regex ConfigReg = new Regex(@"^\/(?<RegExp>.*)\/g$");
        #endregion
    }
}
