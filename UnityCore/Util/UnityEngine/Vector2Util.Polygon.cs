using System.Collections.Generic;

namespace UnityEngine
{

    /// <summary>
    /// 多边形相关判定和集合运算
    /// </summary>
    public static partial class Vector2Util
    {
        /// <summary>
        /// 快速凸包算法<para/>
        /// 分别从8个方向(右,右上,上..逆时针方向)找最远的顶点组成凸包<para/>
        /// </summary>
        public static void QuickHull(IList<Vector2> list, HashSet<int> indexs/*,ref float minX,ref float maxX,ref float minY,ref float maxY*/)
        {
            //unity 视口坐标系(左下0,0;右上1,1)
            int left_index = 0;
            int right_index = 0;
            int top_index = 0;
            int bottom_index = 0;

            int leftTop_index = 0;
            int rightTop_index = 0;
            int leftBottom_index = 0;
            int rightBottom_index = 0;

            for (int i = 1; i < list.Count; i++)
            {
                var p = list[i];
                if (p.x < list[left_index].x)
                {
                    left_index = i;
                }
                if (p.x > list[right_index].x)
                {
                    right_index = i;
                }
                if (p.y > list[top_index].y)
                {
                    top_index = i;
                }
                if (p.y < list[bottom_index].y)
                {
                    bottom_index = i;
                }

                if(p.x-p.y<list[leftTop_index].x-list[leftTop_index].y)
                {
                    leftTop_index = i;
                }

                if (p.x - p.y > list[rightBottom_index].x - list[rightBottom_index].y)
                {
                    rightBottom_index = i;
                }

                if (p.x + p.y < list[leftBottom_index].x + list[leftBottom_index].y)
                {
                    leftBottom_index = i;
                }

                if (p.x + p.y > list[rightTop_index].x + list[rightTop_index].y)
                {
                    rightTop_index = i;
                }
            }

            //minX = list[left_index].x;
            //maxX = list[right_index].x;
            //minY = list[bottom_index].y;
            //maxY = list[top_index].y;

            indexs.Clear();
            indexs.Add(right_index);
            indexs.Add(rightTop_index);
            indexs.Add(top_index);
            indexs.Add(leftTop_index);
            indexs.Add(left_index);
            indexs.Add(leftBottom_index);
            indexs.Add(bottom_index);
            indexs.Add(rightBottom_index);
        }

        /// <summary>
        /// 凸包算法
        /// </summary>
        public static List<Vector2> CalcConvexHull(List<Vector2> list)
        {
            List<Vector2> resPoint = new List<Vector2>();
            //查找最小坐标点
            int minIndex = 0;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].y < list[minIndex].y)
                {
                    minIndex = i;
                }
            }
            Vector2 minPoint = list[minIndex];
            resPoint.Add(list[minIndex]);
            list.RemoveAt(minIndex);
            //坐标点排序
            list.Sort(delegate (Vector2 p1, Vector2 p2) {
                Vector2 baseVec;
                baseVec.x = 1;
                baseVec.y = 0;

                Vector2 p1Vec;
                p1Vec.x = p1.x - minPoint.x;
                p1Vec.y = p1.y - minPoint.y;

                Vector2 p2Vec;
                p2Vec.x = p2.x - minPoint.x;
                p2Vec.y = p2.y - minPoint.y;

                double up1 = p1Vec.x * baseVec.x;
                double down1 = Mathf.Sqrt(p1Vec.x * p1Vec.x + p1Vec.y * p1Vec.y);

                double up2 = p2Vec.x * baseVec.x;
                double down2 = Mathf.Sqrt(p2Vec.x * p2Vec.x + p2Vec.y * p2Vec.y);


                double cosP1 = up1 / down1;
                double cosP2 = up2 / down2;

                if (cosP1 > cosP2)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
                );
            resPoint.Add(list[0]);
            resPoint.Add(list[1]);
            for (int i = 2; i < list.Count; i++)
            {
                Vector2 basePt = resPoint[resPoint.Count - 2];
                Vector2 v1;
                v1.x = list[i - 1].x - basePt.x;
                v1.y = list[i - 1].y - basePt.y;

                Vector2 v2;
                v2.x = list[i].x - basePt.x;
                v2.y = list[i].y - basePt.y;

                if (v1.x * v2.y - v1.y * v2.x < 0)
                {
                    resPoint.RemoveAt(resPoint.Count - 1);
                    while (true)
                    {
                        Vector2 basePt2 = resPoint[resPoint.Count - 2];
                        Vector2 v12;
                        v12.x = resPoint[resPoint.Count - 1].x - basePt2.x;
                        v12.y = resPoint[resPoint.Count - 1].y - basePt2.y;
                        Vector2 v22;
                        v22.x = list[i].x - basePt2.x;
                        v22.y = list[i].y - basePt2.y;
                        if (v12.x * v22.y - v12.y * v22.x < 0)
                        {
                            resPoint.RemoveAt(resPoint.Count - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    resPoint.Add(list[i]);
                }
                else
                {
                    resPoint.Add(list[i]);
                }
            }
            return resPoint;
        }
        /// <summary>
        /// 计算任意多边形的中心<para/>
        /// 各顶点平均值
        /// </summary>
        public static Vector2? Average(IList<Vector2> mPoints)
        {
            if (mPoints.Count == 0) return null;
            Vector2 p = Vector2.zero;
            for (int i = 0; i < mPoints.Count; i++)
            {
                p += mPoints[i];
            };
            return p / mPoints.Count;
        }

        /// <summary>
        /// 计算任意多边形的重心<para/>
        /// 性质:经过重心的任意直线都可以把多边形分割成面积相等的两部分.
        /// </summary>
        public static Vector2 GetCenterOfGravityPoint(IList<Vector2> mPoints)
        {
            float area = 0.0f;//多边形面积  
            float Gx = 0.0f, Gy = 0.0f;// 重心的x、y  
            for (int i = 1; i <= mPoints.Count; i++)
            {
                float iLat = mPoints[(i % mPoints.Count)].x;
                float iLng = mPoints[(i % mPoints.Count)].y;
                float nextLat = mPoints[(i - 1)].x;
                float nextLng = mPoints[(i - 1)].y;
                float temp = (iLat * nextLng - iLng * nextLat) / 2.0f;
                area += temp;
                Gx += temp * (iLat + nextLat) / 3.0f;
                Gy += temp * (iLng + nextLng) / 3.0f;
            }
            Gx = Gx / area;
            Gy = Gy / area;
            return new Vector2(Gx, Gy);
        }
        /// <summary>
        /// a,b,c三个顶点构成的三角形面积
        /// 顺时针排列为负数;逆时针排列为正数
        /// </summary>
        public static float CalcArea(Vector2 a, Vector2 b, Vector2 c)
        {
            //这个算法结果一样
            //return Vector2Util.Cross(b - a, c - b)/2

            //沿着多边形的边求曲线积分,若积分为正,则是沿着边界曲线正方向(逆时针),反之为顺时针
            float d = 0;
            //a0 b1 c2
            d += -0.5f * (a.y + c.y) * (a.x - c.x);
            d += -0.5f * (b.y + a.y) * (b.x - a.x);
            d += -0.5f * (c.y + b.y) * (c.x - b.x);

            //小于零为顺时针，大于零为逆时针
            return d;
        }

        /// <summary>
        /// a,b,c三个顶点 true:顺时针排列;false:逆时针排列
        /// </summary>
        public static bool IsPolyClockwise(Vector2 a, Vector2 b, Vector2 c)
        {
            //沿着多边形的边求曲线积分,若积分为正,则是沿着边界曲线正方向(逆时针),反之为顺时针
            double d = 0;
            //a0 b1 c2
            d += -0.5 * (a.y + c.y) * (a.x - c.x);
            d += -0.5 * (b.y + a.y) * (b.x - a.x);
            d += -0.5 * (c.y + b.y) * (c.x - b.x);

            //小于零为顺时针，大于零为逆时针
            return d < 0.0;
        }
        /// <summary>
        /// 是否顺时针排列顶点
        /// </summary>
        public static bool IsPolyClockwise(IList<Vector2> list)
        {
            return CalcArea(list) < 0;
        }

        /// <summary>
        /// 顺时针排列凸多边形顶点
        /// </summary>
        public static void SortByClockwise(List<Vector2> list)
        {
            //点集排序
            if (list.Count > 2)
            {
                Vector2 center = Vector2.zero;
                for (int i = 0; i < list.Count; i++)
                {
                    center += list[i];
                }
                center /= list.Count;
                list.Sort(p =>
                {
                    var dirX = (p - center).normalized;
                    if (dirX.y > 0) return dirX.x + 5;
                    else return -dirX.x;
                });
            }
        }
        /// <summary>
        /// 多边形面积
        /// </summary>
        public static float RealArea(IList<Vector2> list)
        {
            return Mathf.Abs(CalcArea(list));
        }

        /// <summary>
        /// 多边形面积(凹凸通用)
        /// 为正表示顶点按逆时针排列;反之,按顺时针排列
        /// </summary>
        public static float CalcArea(IList<Vector2> list)
        {
            if (list.Count < 3) return 0f;
            //沿着多边形的边求曲线积分,若积分为正,则是沿着边界曲线正方向(逆时针),反之为顺时针
            float d = 0;
            var nSize = list.Count;
            for (int i = 0, prev = nSize - 1; i < nSize; prev = i++)
            {
                d += (list[i].y + list[prev].y) * ( list[prev].x- list[i].x);
            }
            return d/2;
        }

        /// <summary>
        /// 获取点在不规则多边形内和最近的边的距离
        /// </summary>
        public static float MinDistance(this Vector2 p, IList<Vector2> list, out int start, out int end, out Vector2 intersection)
        {
            Vector2 _temp = Vector2.zero;
            //System.Func<int, int,float> Distance = (a, b) =>
            //{
            //    return Mathf.Abs(p.DistanceSegment(list[a], list[b],out _temp));
            //};
            start = list.Count - 1;
            end = 0;
            float minDis = Mathf.Abs(p.DistanceBySegment(list[start], list[end], out _temp));//Distance(start, end);
            intersection = _temp;
            for (int i = 0; i < list.Count - 1; i++)
            {
                var dis = Mathf.Abs(p.DistanceBySegment(list[i], list[i + 1], out _temp));//Distance(i, i + 1);
                if (dis < minDis)
                {
                    intersection = _temp;
                    minDis = dis;
                    start = i;
                    end = i + 1;
                }
            }
            return minDis;
        }

        /// <summary>
        /// 获取一个多边形的外包含矩形,width=0时外切
        /// </summary>
        public static List<Vector2> GetOutSizeRectangle(IList<Vector2> list,float edgeWidth)
        {
            float xMin = float.MaxValue, xMax = float.MinValue, yMin = float.MaxValue, yMax = float.MinValue;
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                xMax = Mathf.Max(xMax, p.x + edgeWidth);
                xMin = Mathf.Min(xMin, p.x - edgeWidth);
                yMax = Mathf.Max(yMax, p.y + edgeWidth);
                yMin = Mathf.Min(yMin, p.y - edgeWidth);
            }
            var edgelist = new List<Vector2>();
            edgelist.Add(new Vector2(xMin, yMin));
            edgelist.Add(new Vector2(xMin, yMax));
            edgelist.Add(new Vector2(xMax, yMax));
            edgelist.Add(new Vector2(xMax, yMin));

            return edgelist;
        }

        /// <summary>
        /// 获取胶囊的外接矩形
        /// </summary>
        public static Vector2[] GetExternalRectangleByCapsule(Vector2 from, Vector2 to, float rad)
        {
            var mPolygon = new Vector2[4];
            var Dir = (to - from).normalized;
            var delta = Dir.Rot90() * rad;
            mPolygon[0] = from;
            mPolygon[1] = from - delta;
            mPolygon[2] = to - delta;
            mPolygon[3] = to;
            return mPolygon;
        }
    }
}
