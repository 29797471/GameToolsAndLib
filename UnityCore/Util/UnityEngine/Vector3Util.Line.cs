namespace UnityEngine
{
    public static partial class Vector3Util
    {
        /// <summary>
        /// 投影法
        /// 获取点到直线的距离
        /// 为正时表示p,u,v按顺时针排列
        /// </summary>
        public static float DistanceByLine(this Vector3 p, Vector3 u,Vector3 v)
        {
            Vector3 p_vec = p - u;
            Vector3 a_vec = v - u;
            return (p_vec-Vector3.Project(p_vec, a_vec)).magnitude;
            
        }

    }
}
