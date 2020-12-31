using System.Collections;

namespace CqCore
{
    /// <summary>
    /// 全局协程操作类
    /// </summary>
    public static partial class GlobalCoroutine
    {
        /// <summary>
        /// 协程等待一段时间(seconds秒)
        /// </summary>
        public static IEnumerator Sleep(float seconds = 1)
        {
            if (seconds >= 0)
            {
                var endTick = GetTickTime(seconds);
                while (Tick < endTick)
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 协程等待一段时间(seconds秒),可动态调整速度来减少等待时间.
        /// </summary>
        public static IEnumerator Sleep(float seconds, AsyncReturn<float> timeScale)
        {
            while (seconds > 0)
            {
                yield return null;
                seconds -= deltaTime * timeScale.data;
            }
        }

        

        /// <summary>
        /// 协程等待一段时间(frames帧)
        /// </summary>
        public static IEnumerator Sleep(int frames = 1)
        {
            for (int i = frames; i > 0; i--)
            {
                yield return null;
            }
        }
    }
}
