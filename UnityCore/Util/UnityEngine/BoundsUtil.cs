namespace UnityEngine
{
    /// <summary>
    /// 包围盒
    /// </summary>
    public static class BoundsUtil
    {

        /// <summary>
        /// 获取包含该对象所有子渲染网格的包围盒
        /// </summary>
        public static Bounds GetBounds(GameObject obj)
        {
            bool Encapsulate = false;
            Bounds bounds = default(Bounds);
            var renders = obj.GetComponentsInChildren<Renderer>();
            foreach(var render in renders)
            {
                if (Encapsulate)
                {
                    bounds.Encapsulate(render.bounds);
                }
                else
                {
                    Encapsulate = true;
                    bounds = render.bounds;
                }
            }
            return bounds;
        }
        /// <summary>
        /// 用于解决计算时产生gc的临时变量
        /// </summary>
        internal static Vector3[] temp_vs = new Vector3[8];

        /// <summary>
        /// 当前包围盒包含另一包围盒
        /// </summary>
        public static bool Contains(this Bounds bounds,Bounds other)
        {
            return (bounds.Contains(other.min) && bounds.Contains(other.max));
        }
        /// <summary>
        /// 由包围盒得到8个顶点<para/>
        /// 下面0(小小小),1(小小大),2(大小大),3(大小小) <para/>
        /// 上面4(小大小),5(小大大),6(大大大),7(大大小) 
        /// </summary>
        public static Vector3[] ToVertexs(this Bounds bounds)
        {
            var vs = new Vector3[8];
            bounds.ToVertexs(ref vs);
            return vs;
        }
        /// <summary>
        /// 由包围盒得到8个顶点<para/>
        /// 可传长度4的数组返回下面<para/>
        /// 下面0(小小小),1(小小大),2(大小大),3(大小小) <para/>
        /// 上面4(小大小),5(小大大),6(大大大),7(大大小) 
        /// </summary>
        public static void ToVertexs(this Bounds bounds,ref Vector3[] vs)
        {
            var min = bounds.min;
            var max = bounds.max;
            //8个顶点
            //下面0(小小小),1(小小大),2(大小大),3(大小小) 
            if (vs.Length >= 4)
            {
                vs[0] = new Vector3(min.x, min.y, min.z);
                vs[1] = new Vector3(min.x, min.y, max.z);
                vs[2] = new Vector3(max.x, min.y, max.z);
                vs[3] = new Vector3(max.x, min.y, min.z);
            }

            if (vs.Length >= 8)
            {
                //上面4(小大小),5(小大大),6(大大大),7(大大小) 
                vs[4] = new Vector3(min.x, max.y, min.z);
                vs[5] = new Vector3(min.x, max.y, max.z);
                vs[6] = new Vector3(max.x, max.y, max.z);
                vs[7] = new Vector3(max.x, max.y, min.z);
            }
        }
        /// <summary>
        /// 由有旋转或者变换信息的包围盒得到在世界坐标系下的8个顶点<para/>
        /// 可传长度4的数组返回下面<para/>
        /// 下面0(小小小),1(小小大),2(大小大),3(大小小) <para/>
        /// 上面4(小大小),5(小大大),6(大大大),7(大大小) 
        /// </summary>
        public static Vector3[] ToWorldVertexs(this Bounds bounds, Matrix4x4? worldMat)
        {
            var vs = new Vector3[8];

            bounds.ToWorldVertexs(worldMat, ref vs);
            return vs;
        }
        /// <summary>
        /// 由有旋转或者变换信息的包围盒得到在世界坐标系下的8个顶点<para/>
        /// 可传长度4的数组返回下面<para/>
        /// 下面0(小小小),1(小小大),2(大小大),3(大小小) <para/>
        /// 上面4(小大小),5(小大大),6(大大大),7(大大小) 
        /// </summary>
        public static void ToWorldVertexs(this Bounds bounds, Matrix4x4? worldMat,ref Vector3[] vs)
        {
            bounds.ToVertexs(ref vs);

            if (worldMat != null)
            {
                var mat = (Matrix4x4)worldMat;
                for (int i = 0; i < vs.Length; i++)
                {
                    vs[i] = mat.MultiplyPoint(vs[i]);
                }
            }
        }

        /// <summary>
        /// 由有旋转或者变换信息的包围盒得到在世界坐标系下的包围盒
        /// </summary>
        public static Bounds ToWorldBounds(this Bounds bounds, Matrix4x4 worldMat)
        {
            bounds.ToWorldVertexs(worldMat,ref temp_vs);
            var renderBounds = new Bounds(temp_vs[0], Vector3.zero);
            for (int i = 0; i < 8; i++)
            {
                renderBounds.Encapsulate(temp_vs[i]);
            }
            return renderBounds;
        }

        


        /// <summary>
        /// 世界坐标系下的两个包围盒是否相交
        /// </summary>
        public static bool Intersects(Bounds a,Matrix4x4 aMat,Bounds b,Matrix4x4 bMat)
        {
            //得到在a坐标系下的b包围盒
            var _b= b.ToWorldBounds(aMat.inverse*bMat);
            
            return a.Intersects(_b);
        }

        /// <summary>
        /// 世界坐标系下的包围盒与射线检测是否发生碰撞,参数返回距离
        /// </summary>
        public static bool IntersectRay(this Bounds bounds,Matrix4x4 mat, Ray ray,out float dis)
        {
            var localRay = ray.Multiply(mat.inverse);
            var bl = bounds.IntersectRay(localRay, out dis);

            if (bl)
            {
                var localColliderPoint = localRay.GetPoint(dis);
                var p = mat.MultiplyPoint(localColliderPoint);
                
                dis = Vector3.Distance(p, ray.origin);
            }
            return bl;
        }

        /// <summary>
        /// 转y=0区域
        /// </summary>
        public static Rect ToRect(this Bounds bounds)
        {
            return new Rect(bounds.center.ToVector2(), bounds.size.ToVector2());
        }
    }
}
