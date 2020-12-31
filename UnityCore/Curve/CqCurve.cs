using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 曲线数据结构,定义在Mono中,可以在insepector中实时编辑
    /// </summary>
    [System.Serializable]
    public class CqCurve
    {
        /// <summary>
        /// 闭合
        /// </summary>
        [CheckBox("闭合")]
        public bool close;


        [SerializeField]
        //[ListBox("端点列表")]
        public List<CqCurvePoint> points;

        /// <summary>
        /// 将一个闭合图形采样,曲边化直,生成多边形
        /// </summary>
        /// <param name="sampling">每曲边采样点数</param>
        /// <returns></returns>
        public List<Vector3> ExportPolygon(int sampling=16)
        {
            var list = new List<Vector3>();
            for(var i=0;i<points.Count;i++)
            {
                var it = points[i];
                var next = points.GetItemByRound(i + 1);
                list.Add(it.point);
                if(!it.IsLine(next))
                {
                    for(int j=0;j<sampling;j++)
                    {
                        list.Add(it.LerpUnclamped(next, (j + 1f) / (sampling + 1f)));
                    }
                }
            }
            return list;
        }

        public float Length
        {
            get
            {
                var mLength = 0f;
                var count = points.Count + (close ? 0 : -1);
                for (int i = 0; i < count; i++)
                {
                    var p1 = points[i];
                    var p2 = points.GetItemByRound(i + 1);
                    var len = p1.Length(p2);
                    mLength += len;
                }
                return mLength;
            }
        }

        /// <summary>
        /// 控制系数:0~1
        /// 闭合时:0时在起点,1时返回起点
        /// </summary>
        public Vector3 this[float k]
        {
            get
            {
                if (k == 0) return points[0].point;
                if (k == 1) return close ? points[0].point : points[points.Count - 1].point;

                var realK = k * (points.Count + (close ? 0 : -1));
                var i = Mathf.FloorToInt(realK);
                var partK = realK - i;
                var current = points[i];
                var next = points.GetItemByRound(i + 1);
                return current.LerpUnclamped(next, partK);

            }
        }

        /// <summary>
        /// 二分法求贝塞尔曲线和直线的交点返回t:0~1(用于通过鼠标拾取曲线上一点)
        /// </summary>
        public float GetCrossoverPoint(Ray ray,out int index, Matrix4x4 mat, float limit = 0.01f)
        {
            var count = points.Count;
            if (!close) count--;
            for(int i=0;i< count;i++)
            {
                var current = points[i];
                var next = points.GetItemByRound(i + 1);
                var t = current.GetCrossoverPoint(next, ray, mat, limit);
                if (t != -1)
                {
                    index = i;
                    return t;
                }
            }
            index = -1;
            return -1;
        }

        /// <summary>
        /// 传入2次贝塞尔控制点,调整一条边的出入切线.
        /// </summary>
        public void SetOutInTangent(int index, Vector3 controlPos)
        {
            var current = points.GetItemByRound(index );
            var next = points.GetItemByRound(index + 1);
            var temp= controlPos / 3 * 2;
            current.outTangent = current.point / 3 + temp;
            next.inTangent = next.point / 3 + temp;
        }

        /// <summary>
        /// 平滑<para/>
        /// 由前后两点计算该点的切线,使曲线平滑
        /// </summary>
        public void Smooth(int index,float smoothK)
        {
            var prev = points.GetItemByRound(index - 1);
            var next = points.GetItemByRound(index + 1);
            points[index].Smooth(prev, next, smoothK);
        }
        /// <summary>
        /// 平移所有点,保证中心点在原点
        /// </summary>
        public void MoveCenterToZero()
        {
            var center = Vector3.zero;
            points.ForEach(x => center += x.point);
            center /= points.Count;
            points.ForEach(x =>
            {
                x.point -= center;
            });
        }

        /// <summary>
        /// 移除曲线顶点<para/>
        /// 如果左右两侧有一个是曲线,则移除后按曲线合并
        /// 如果左右都是直线,则移除后按直线合并
        /// </summary>
        public void Del(CqCurvePoint p,float smoothK)
        {
            var index = points.IndexOf(p);
            var prev = points.GetItemByRound(index - 1);
            var next = points.GetItemByRound(index + 1);
            points.RemoveAt(index);
            if (prev.IsLine(p) && p.IsLine(next))
            {

            }
            else
            {
                //如果左右两侧有一个是曲线
                //Smooth(index == points.Count ? 0 : index, smoothK);

                //target.curve.Smooth(index == 0 ? (target.curve.points.Count - 1) : (index - 1), config.smoothK);
            }
            
        }
    }
}
