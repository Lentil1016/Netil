using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Netil.Helper
{
    class StringHelper
    {
        /// <summary>
        /// TODO:字符串转化为正则表达式前的前处理
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <returns></returns>
        public string Pre_Proc(string str)
        {
            //I wonder if there is any way better to surpport both RegExp and string? sincerely.
            //My way is so dummmmmmmmmmmmmmmmmmmmmmmmm!
            if (ConfigReg.IsMatch(str))
            {
                str=ConfigReg.Match(str).Groups["RegExp"].Value;
            }
            else if(ConfigStr.IsMatch(str))//将普通字符串转义为正则表达式，效率损失堪忧，不幸中的万幸是，这是一次性损失。
            {
                str.Replace(@"\", @"\\");
                str.Replace(@"?", @"\?");
                str.Replace(@"*", @"\*");
                str.Replace(@"+", @"\+");
                str.Replace(@".", @"\.");
                str.Replace(@"^", @"\^");
                str.Replace(@"$", @"\$");
                str.Replace(@"|", @"\|");
                str.Replace(@"(", @"\)");
                str.Replace(@")", @"\)");
                str.Replace(@"{", @"\{");
                str.Replace(@"}", @"\}");
                str.Replace(@"[", @"\[");
                str.Replace(@"]", @"\]");
            }
            return str;
        }

        Regex ConfigStr = new Regex(@"[^\s\d\w]");
        Regex ConfigReg = new Regex(@"^\/(?<RegExp>.*)\/g$");
    }
}
