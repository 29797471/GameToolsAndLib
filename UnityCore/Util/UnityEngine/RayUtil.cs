namespace UnityEngine
{
    /// <summary>
    /// 射线扩展
    /// </summary>
    public static class RayUtil
    {
        /// <summary>
        /// 坐标系转换
        /// </summary>
        public static Ray Multiply(this Ray ray,Matrix4x4 mat)
        {
            return new Ray(mat.MultiplyPoint(ray.origin), mat.MultiplyVector(ray.direction));
        }
        /// <summary>
        /// 射线转矩阵
        /// </summary>
        public static Matrix4x4 ToMatrix(this Ray ray)
        {
            return Matrix4x4.TRS(ray.origin, Quaternion.LookRotation(ray.direction), Vector3.one);
        }
        /// <summary>
        /// 绘制射线
        /// </summary>
        public static void DebugDraw(this Ray ray, Color color)
        {
            Debug.DrawRay(ray.origin, ray.direction,color);
        }

        /// <summary>
        /// 绘制射线
        /// </summary>
        public static void DebugDraw(this Ray ray)
        {
            Debug.DrawRay(ray.origin, ray.direction);
        }

        /// <summary>
        /// 获取两直线交点,当角度在10度以内视为平行,返回null
        /// </summary>
        public static Vector2? TryIntersect(this Ray2D line1, Ray2D line2, float angleLimit=10f)
        {
            return Vector2Util.TryIntersect(
                line1.origin,
                line1.origin + line1.direction, 
                line2.origin,
                line2.origin + line2.direction,
                angleLimit);
        }

        ///// <summary>
        ///// 获取两直线交点,当角度在10度以内视为平行,返回null
        ///// </summary>
        //public static Vector2? TryIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float angleLimit = 10f)
        //{
        //    var intersection = new Vector2();

        //    float denominator = (p2.y - p1.y) * (p4.x - p3.x) - (p1.x - p2.x) * (p3.y - p4.y);
        //    // If denominator is 0, means parallel
        //    if (denominator == 0)
        //    {
        //        return null;
        //    }

        //    // Check angle between segments
        //    float angle = Vector2.Angle(p2-p1, p4-p3);
        //    // if the angle between two segments is too small, we treat them as parallel
        //    if (angle < angleLimit || (180f - angle) < angleLimit)
        //    {
        //        return null;
        //    }

        //    float x = ((p2.x - p1.x) * (p4.x - p3.x) * (p3.y - p1.y)
        //            + (p2.y - p1.y) * (p4.x - p3.x) * p1.x
        //            - (p4.y - p3.y) * (p2.x - p1.x) * p3.x) / denominator;
        //    float y = -((p2.y - p1.y) * (p4.y - p3.y) * (p3.x - p1.x)
        //            + (p2.x - p1.x) * (p4.y - p3.y) * p1.y
        //            - (p4.x - p3.x) * (p2.y - p1.y) * p3.y) / denominator;

        //    intersection.Set(x, y);
        //    return intersection;
        //}
        /// <summary>
        /// 获取点到直线的距离,正负可以用来判定在直线的哪一侧
        /// </summary>
        public static float Distance(this Ray2D line, Vector2 p)
        {
            var A = line.direction.y; //1 / line.direction.x;
            var B = -line.direction.x; //-1 / line.direction.y;
            var C = -line.origin.x * line.direction.y+ line.origin.y * line.direction.x;
            return (A*p.x+B*p.y+C)/Mathf.Sqrt(A*A+B*B);
        }

        /// <summary>
        /// 投影法
        /// 获取点到直线的距离
        /// 为正时表示p,射线起点,射线方向上一点,按顺时针排列
        /// </summary>
        public static float Distance(this Ray ray, Vector3 p)
        {
            var u = ray.origin;
            var v = ray.origin+ray.direction*1000;
            Vector3 p_vec = p - u;
            Vector3 a_vec = v - u;
            return (p_vec - Vector3.Project(p_vec, a_vec)).magnitude;
        }

        /// <summary>
        /// 投影法
        /// 获取点到直线的垂线(向量)
        /// </summary>
        public static Vector3 GetVerticalVec(this Ray ray, Vector3 p)
        {
            var u = ray.origin;
            var v = ray.origin + ray.direction * 1000;
            Vector3 p_vec = p - u;
            Vector3 a_vec = v - u;
            return p_vec - Vector3.Project(p_vec, a_vec);
        }

        /// <summary>
        /// 获取该点投影在射线上的点,到射线起点的距离,垂足在射线方向上为正,反之为负
        /// </summary>
        public static float ProjectDistance(this Ray ray, Vector3 p)
        {
            return Vector3.Dot(p - ray.origin, ray.direction);
            //var u = ray.origin;
            //var v = ray.origin + ray.direction;
            //Vector3 p_vec = p - u;
            //Vector3 a_vec = v - u;
            //var t = Vector3.Project(p_vec, a_vec);

            //var xx= t.normalized.EqualsByEpsilon(ray.direction) ? t.magnitude : -t.magnitude;

            //var yy = Vector3.Dot(p - ray.origin, ray.direction);

            //return xx;
        }
    }
}
