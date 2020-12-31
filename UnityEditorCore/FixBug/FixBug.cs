using System;

namespace UnityEditor
{
    /// <summary>
    /// 解决unity bug
    /// </summary>
    public static class FixBug
    {
        static long time;
        /// <summary>
        /// 当MenuItem修饰的静态函数,通过右键在Hierarchy回调时,如果是多选GameObject,会产生BUG,被回调多次.
        /// </summary>
        public static void CallMenuItemOnce(Action action)
        {
            if (action == null) return;
            var now = DateTime.Now.Ticks / 10000;//毫秒
            //用Time.realtimeSinceStartup记录时间会有问题,会应为编辑器下场景切换而重新计时.
            if (now < time + 2000)
            {
                time = now;
                return;
            }
            time = now;
            action();
        }
    }
}
