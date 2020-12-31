using UnityEngine;

namespace UnityCore
{
    public enum Style
    {
        Assertion,
        Warning,
        Normal,
        Error,
    }
    public static class Debuger
    {
        public static bool EnableLog = false;
        public static void Log(object message, Style style= Style.Normal)
        {
            if(EnableLog)
            {
                DebugLog(message, style);
            }
        }
        static void DebugLog(object message, Style style)
        {
            switch (style)
            {
                case Style.Assertion:
                    Debug.LogAssertion(message);
                    break;
                case Style.Warning:
                    Debug.LogWarning(message);
                    break;
                case Style.Normal:
                    Debug.Log(message);
                    break;
                case Style.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }
        public static void LogStackTrace()
        {
            string info = "";
            //设置为true，这样才能捕获到文件路径名和当前行数，当前行数为GetFrames代码的函数，也可以设置其他参数  
            var st = new System.Diagnostics.StackTrace(true);
            //得到当前的所以堆栈  
            System.Diagnostics.StackFrame[] sf = st.GetFrames();
            for (int i = 0; i < sf.Length; ++i)
            {
                var it = sf[i];
                if (info != "") info += "\r\n";
                info += string.Format("{2}:{3}\t[{0}:{1}]", it.GetFileName(), it.GetFileLineNumber(), sf[i].GetMethod().DeclaringType.FullName, it.GetMethod().Name);
                
            }
            Log(info);
        }
    }
}
