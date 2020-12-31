using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    public class CFile
    {
        string path;
        public CFile(string path)
        {
            this.path = path;
        }
        public void Delete()
        {

        }
        public void Move(string dstPath)
        {

        }
        //文件大小(字节单位 转 MB..
        public static string HumanReadableFilesize(double size)
        {
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
    }
}
