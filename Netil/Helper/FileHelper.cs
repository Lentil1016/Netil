using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Netil.Helper
{
    class FileHelper
    {

        public void WriteFile(string FileName, string Content)
        {
            lock(AddIndexLock)
                if (!FileLocksDict.ContainsKey(FileName))
                {
                    FileLocksDict[FileName] = new object();
                    FilesDict[FileName] = new StreamWriter(new FileStream(FileName, FileMode.Create));
                }

            lock (FileLocksDict[FileName])
                FilesDict[FileName].Write(Content);
        }

        private object AddIndexLock = new object();//添加索引时加锁，避免多线程重复创建文件流索引

        private Dictionary<string, StreamWriter> FilesDict = new Dictionary<string, StreamWriter>();//文件写入流索引
        private Dictionary<string, object> FileLocksDict = new Dictionary<string, object>();//文件写入流锁索引
    }
}
