namespace UnityEngine
{
    public static partial class Vector3Util
    {
        /// <summary>
        /// 通过0~1系数和位置的关系,推导距离和位置的关系<para/>
        /// 主要提供给一些运动单位作固定轨迹上的可控速度的运动.
        /// </summary>
        public static System.Func<float, Vector3> GetSportFun_Len_Pos(System.Func<float, Vector3> fun_Pos, out float totalLen, int partCount = 100)
        {
            var ary = new float[partCount];
            float delta = 1f / partCount;
            float t = 0;
            Vector3 lastPos = fun_Pos(t);
            totalLen = 0f;
            for (int i = 0; i < ary.Length; i++)
            {
                t += delta;
                var nowPos = fun_Pos(t);
                var len = Vector3.Distance(lastPos, nowPos);
                totalLen += len;
                ary[i] = len;
                lastPos = nowPos;
            }

            return len =>
            {
                float k = 0f;
                for (int i = 0; i < ary.Length; i++)
                {
                    if (len > ary[i])
                    {
                        len -= ary[i];
                    }
                    else
                    {
                        k = i * 1f / ary.Length + (len / ary[i]) * 1f / ary.Length;
                        break;
                    }
                }
                return fun_Pos(k);
            };
        }
    }
}
