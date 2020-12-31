using System;

namespace CqCore
{
    
    /// <summary>
    /// 打印函数
    /// </summary>
    public static class CqDebug
    {
        public static Action<string> BeginSample;
        public static Action EndSample;
        /// <summary>
        /// 打印
        /// </summary>
        public static Action<object, LogType> LogHandle= (obj, type) =>
        {
            Console.WriteLine(obj);
        };

        /// <summary>
        /// 调用注入的打印函数来打印
        /// </summary>
        public static void Log(object message, LogType type=LogType.Log)
        {
            if (LogHandle != null) LogHandle(message, type);
        }

        /// <summary>
        /// 在协程中调用注入的打印函数来打印
        /// </summary>
        public static void LogInCoroutine(object message, LogType type = LogType.Log)
        {
            if (LogHandle != null)
            {
                GlobalCoroutine.Call(() => LogHandle(message, type));
            }
        }

        /// <summary>
        /// 执行函数,打印执行时间
        /// </summary>
        public static void TestExec(Action action,uint times=1)
        {
            Log(string.Format("执行时间：{0}秒", ExecFun(action, times).ToString("n5")));
        }

        /// <summary>
        ///  执行函数,返回执行时间(s)
        /// </summary>
        public static double ExecFun(Action action,uint times=1)
        {
            DateTime testStartTime = DateTime.Now;
            for(int i=0;i<times;i++)action.Invoke();
            return (DateTime.Now - testStartTime).TotalMilliseconds / 1000.0;
            //string totalTime = ((DateTime.Now - testStartTime).TotalMilliseconds / 1000.0).ToString("n5");
            //string reval = string.Format("总共执行时间：{0}秒", totalTime);
            //Console.Write(reval);
        }
    }
}
