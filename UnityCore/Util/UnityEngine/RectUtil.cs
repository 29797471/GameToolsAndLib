namespace UnityEngine
{
    /// <summary>
    /// 区域扩展
    /// </summary>
    public static class RectUtil
    {
        /// <summary>
        /// 区域缩放
        /// </summary>
        public static Rect Scale(this Rect a, float b)
        {
            return new Rect(a.position * b, a.size * b);
        }
        /// <summary>
        /// 区域包含另一区域
        /// </summary>
        public static bool Contains(this Rect a, Rect b)
        {
            return a.Contains(b.position) &&
                a.Contains(b.position+b.size);
        }
        /// <summary>
        /// 广义上的相交(同另一区域不相离)<para/>
        /// 两个轴向上的中心点距离小于半宽度和.
        /// </summary>
        public static bool Intersects(this Rect a, Rect b)
        {
            var a_center = a.center;
            var b_center = b.center;
            return Mathf.Abs(a_center.x - b_center.x) <= (a.width + b.width) / 2 &&
                Mathf.Abs(a_center.y - b_center.y)  <= (a.height + b.height) / 2;
        }
        
        /// <summary>
        /// 由中心点和宽高构造矩形
        /// </summary>
        public static Rect GetRect(Vector2 center,Vector2 size)
        {
            return new Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
        }
        /// <summary>
        /// 由中心点和宽高构造矩形
        /// </summary>
        public static Rect GetRect(float center_x,float center_y, float width,float height)
        {
            return new Rect(center_x-width/2, center_y - height / 2, width, height);
        }

    }
}

