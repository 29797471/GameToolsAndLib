using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    public static class EnvironmentUtil
    {
        /// <summary>
        /// 获取应用程序启动参数
        /// </summary>
        /// <returns></returns>
        public static string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
