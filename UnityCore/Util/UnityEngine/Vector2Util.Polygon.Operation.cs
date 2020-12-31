using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    /// <summary>
    /// 多边形集合运算
    /// </summary>
    public static partial class Vector2Util
    {
        /// <summary>
        /// 获取两个多边形位置关系
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="isAToB">对两个关系的补充(比如:A包含B时返回true)</param>
        /// <param name="Epsilon"></param>
        /// <returns></returns>
        public static PolyRelations GetPolyRelations(IList<Vector2> a, IList<Vector2> b, out bool isAToB, float Epsilon = 0.001f)
        {
            isAToB = false;
            if (a.Count < 3 || b.Count < 3)
            {
                return PolyRelations.None;
            }

            var poly0 = new List<Vector2>(a);
            var poly1 = new List<Vector2>(b);

            var list0 = new List<float>();//第一个多边形的相关点索引,包含交点
            var list1 = new List<float>();//第二个多边形的相关点索引,包含交点
            var dic = GetPolygonRelationship(poly0, poly1, list0, list1);

            if (dic.Count==0)//内含或者外离
            {
                if (poly0[0].InRange(poly1))
                {
                    isAToB = false;
                    return PolyRelations.Contains;
                }
                if (poly1[0].InRange(poly0))
                {
                    isAToB = true;
                    return PolyRelations.Contains;
                }
                return PolyRelations.Separation;
            }
            foreach (var it in dic)
            {
                if (!it.Key.IsIntData() && !it.Value.IsIntData())
                {
                    return PolyRelations.Intersection;
                }
            }
            bool allInside0=false;
            bool allOutSide0=false;
            for(var i=0;i<poly0.Count;i++)
            {
                if(!dic.ContainsKey((float)i))
                {
                    if(poly0[i].InRange(poly1))
                    {
                        if(!allInside0)
                        {
                            allInside0 = true;
                        }
                        if (allOutSide0) return PolyRelations.Intersection;
                    }
                    else
                    {
                        if (!allOutSide0)
                        {
                            allOutSide0 = true;
                        }
                        if (allInside0) return PolyRelations.Intersection;
                    }
                }
            }
            bool allInside1 = false;
            bool allOutSide1 = false;
            for (var i = 0; i < poly1.Count; i++)
            {
                if (!dic.ContainsValue((float)i))
                {
                    if (poly1[i].InRange(poly0))
                    {
                        if (!allInside1)
                        {
                            allInside1 = true;
                        }
                        if (allOutSide1) return PolyRelations.Intersection;
                    }
                    else
                    {
                        if (!allOutSide1)
                        {
                            allOutSide1 = true;
                        }
                        if (allInside1) return PolyRelations.Intersection;
                    }
                }
            }
            if (allInside0 && allOutSide1)
            {
                isAToB = false;
                return PolyRelations.Inscribe;
            }
            else if(allOutSide0 && allInside1)
            {
                isAToB = true;
                return PolyRelations.Inscribe;
            }
            return PolyRelations.Intersection;
        }
        /// <summary>
        /// 两多边形求交集
        /// </summary>
        public static List<Vector2> PolygonIntersection(IList<Vector2> a, IList<Vector2> b, float Epsilon = 0.001f)
        {
            return PolygonCollectionCalc(a, b, true, Epsilon);
        }
        /// <summary>
        /// 两多边形求并集(适用于任意凹凸多边形)<para/>
        /// 没有并集时返回null<para/>
        /// 算法核心思想:<para/>
        /// 1.将两个多边形转化成两个双向链表,<para/>
        /// 2.将交点分别插入链表中.<para/>
        /// 3.从一个不在内部的顶点开始,得到外包的回路.
        /// </summary>
        public static List<Vector2> PolygonUnion(IList<Vector2> a, IList<Vector2> b, float Epsilon = 0.001f)
        {
            return PolygonCollectionCalc(a, b, false, Epsilon);
        }

        /// <summary>
        /// 两多边形求差集(适用于任意凹凸多边形),未完成<para/>
        /// 算法核心思想:<para/>
        /// 1.将两个多边形转化成两个双向链表,<para/>
        /// 2.将交点分别插入链表中.<para/>
        /// 3.遍历所有不在内部的顶点得到所有得回路
        /// 4.以下一个点不是交点的交点为起点,遍历回路
        /// </summary>
        public static List<List<Vector2>> PolygonSub(IList<Vector2> a, IList<Vector2> b, float Epsilon = 0.001f)
        {
            if (a.Count < 3 || b.Count < 3)
            {
                return null;
            }

            var poly0 = new List<Vector2>(a);
            var poly1 = new List<Vector2>(b);

            var list0 = new List<float>();//第一个多边形的相关点索引,包含交点
            var list1 = new List<float>();//第二个多边形的相关点索引,包含交点
            var dic = GetPolygonRelationship(poly0, poly1, list0, list1);

            if (dic.Count == 0 || dic.Count == 1)//内含或者外离
            {
                if (poly0[0].InRange(poly1)) return new List<List<Vector2>>();
                if (poly1[0].InRange(poly0)) return null;
                return new List<List<Vector2>>() { new List<Vector2>(a)};
            }

            var list = new List<List<Vector2>>();

            Func<float, IList<Vector2>, Vector2> GetPos = (f, poly) =>
            {
                int n = Mathf.FloorToInt(f);
                var t = f - n;
                return Vector2.LerpUnclamped(poly[n], poly.GetItemByRound(n + 1), t);
            };
            Func<float, Vector2> GetPos0 = f => GetPos(f, poly0);
            Func<float, Vector2> GetPos1 = f => GetPos(f, poly1);
            list1.Reverse();
            foreach(var it in dic)
            {
                var i = list0.IndexOf(it.Key);
                var p = GetPos0(list0[i]) / 2 + GetPos0(list0.GetItemByRound(i + 1)) / 2;
                //从交点开始,并且与第二个点的中心点不在第二个多边形内
                if (!p.InRange(poly1))
                {
                    var result = new List<Vector2>();
                    int j = i;

                    do
                    {
                        result.Add(GetPos0(list0[j]));

                        j = (j + 1) % list0.Count;

                    }
                    while (!dic.ContainsKey(list0[j]));

                    var k = list1.IndexOf(dic[list0[j]]);
                    do
                    {
                        result.Add(GetPos1(list1[k]));
                        k = (k + 1) % list1.Count;
                    }
                    while (list1[k] != it.Value);

                    MergeVert(result, Epsilon);
                    list.Add(result);
                }
            }
            return list;
        }
        
        /// <summary>
        /// 两多边形集合计算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="isIntersection">true:交集;false:并集</param>
        /// <param name="Epsilon"></param>
        /// <returns></returns>
        static List<Vector2> PolygonCollectionCalc(IList<Vector2> a, IList<Vector2> b,bool isIntersection,  float Epsilon = 0.001f)
        {
            if (a.Count < 3 || b.Count < 3)
            {
                return null;
            }

            var poly0 = new List<Vector2>(a);
            var poly1 = new List<Vector2>(b);

            var list0 = new List<float>();//第一个多边形的相关点索引,包含交点
            var list1 = new List<float>();//第二个多边形的相关点索引,包含交点
            var dic = GetPolygonRelationship(poly0, poly1, list0, list1);

            if (dic.Count == 0|| dic.Count==1)//内含或者外离
            {
                if (poly0[0].InRange(poly1)) return isIntersection ? poly0 : poly1;
                if (poly1[0].InRange(poly0)) return isIntersection ? poly1 : poly0;
                var list=new List<Vector2>();
                if(dic.Count == 1 && isIntersection)
                {
                    Func<float, IList<Vector2>, Vector2> GetPos = (f, poly) =>
                    {
                        int n = Mathf.FloorToInt(f);
                        var t = f - n;
                        return Vector2.LerpUnclamped(poly[n], poly.GetItemByRound(n + 1), t);
                    };
                    Func<float, Vector2> GetPos0 = f => GetPos(f, poly0);
                    Func<float, Vector2> GetPos1 = f => GetPos(f, poly1);

                    list.Add(GetPos0(dic.ToList()[0].Key));
                }
                return list;
            }

            //从一个交点开始作为起点
            bool indexInList0 = true;
            var iStart = list0.IndexOf(dic.Keys.First());
            var jStart = list1.IndexOf(dic.Values.First());

            //多边形顶点poly0,poly1已经按顺时针排列,
            //所以得到交集就是从交点开始以顺时针遍历顶点;反之,逆时针遍历获得并集
            var result = MakeOneList(isIntersection, indexInList0,iStart,jStart, list0, list1, dic, poly0,  poly1, Epsilon);
            MergeVert(result, Epsilon);
            return result;
        }
        static List<Vector2> MakeOneList(bool clockwise,bool indexInList0,int iStart,int jStart,
            List<float> list0,List<float> list1,Dictionary<float,float> dic,
            IList< Vector2> poly0, IList<Vector2> poly1, float Epsilon=0.001f)
        {
            Func<float, IList<Vector2>, Vector2> GetPos = (f, poly) =>
            {
                int n = Mathf.FloorToInt(f);
                var t = f - n;
                return Vector2.LerpUnclamped(poly[n], poly.GetItemByRound(n + 1), t);
            };
            Func<float, Vector2> GetPos0 = f => GetPos(f,poly0);
            Func<float, Vector2> GetPos1 = f => GetPos(f,poly1);



            var result = new List<Vector2>();
            var atList0 = indexInList0;
            var i = indexInList0?iStart:-2;
            int j = indexInList0?-2:jStart;

            do
            {
                if (atList0)
                {
                    var iValue = list0[i];
                    float jValue;
                    if (dic.TryGetValue(iValue, out jValue))//计算是否要切换链路
                    {
                        var jTemp = list1.IndexOf(jValue);
                        Vector2 p;
                        if (iValue.IsIntData())//由于两者对应同一个点,选取整数计算解决误差问题
                        {
                            p = GetPos0(iValue);
                        }
                        else
                        {
                            p = GetPos1(jValue);
                        }

                        var index = result.IndexOf(p);
                        if(index!=-1)
                        {
                            result.RemoveRange(0, index);
                            break;
                        }
                        else result.Add(p);

                        //判定顺逆关系
                        var next0 = GetPos0(list0.GetItemByRound(i + 1));

                        var next1 = GetPos1(list1.GetItemByRound(jTemp + 1));
                        var prev = GetPos0(list0.GetItemByRound(i - 1));

                        var vec = prev - p;
                        var vec0 = next0 - p;
                        var vec1 = next1 - p;

                        //var area = CalcArea(p, next0, next1);//判定顺逆关系

                        var angle0 = GetRoundAngle(vec, vec0);
                        var angle1 = GetRoundAngle(vec, vec1);
                        //if (area.EqualZero(Epsilon))//在同一直线上,用前一点来判定
                        {
                            //area = CalcArea(GetPos0(list0.GetItemByRound(i - 1)), next0, next1);
                        }

                        if ( (angle1< angle0 && clockwise) || (angle1 > angle0 && !clockwise))//切换链路
                        //if ((area > 0) != clockwise)//切换链路
                        {
                            atList0 = false;
                            j = (jTemp + 1) % list1.Count;
                            i = -2;
                        }
                        else
                        {
                            i = (i + 1) % list0.Count;
                        }
                    }
                    else
                    {
                        var p = GetPos0(iValue);
                        var index = result.IndexOf(p);
                        if (index != -1)
                        {
                            result.RemoveRange(0, index);
                            break;
                        }
                        else result.Add(p);

                        i = (i + 1) % list0.Count;
                    }

                    
                }
                else
                {
                    float iValue;
                    var jValue = list1[j];
                    if (dic.TryGetKey(jValue, out iValue))//计算是否要切换链路
                    {
                        var iTemp = list0.IndexOf(iValue);
                        Vector2 p;
                        if (iValue.IsIntData())//由于两者对应同一个点,选取整数计算解决误差问题
                        {
                            p = GetPos0(iValue);
                        }
                        else
                        {
                            p = GetPos1(jValue);
                        }


                        var index = result.IndexOf(p);
                        if (index != -1)
                        {
                            result.RemoveRange(0, index);
                            break;
                        }
                        else result.Add(p);


                        var prev = GetPos1(list1.GetItemByRound(j - 1));
                        var next1 = GetPos1(list1.GetItemByRound(j + 1));
                        
                        var next0 = GetPos0(list0.GetItemByRound(iTemp + 1));

                        var vec = prev - p;
                        var vec0 = next0 - p;
                        var vec1 = next1 - p;
                        var angle0 = GetRoundAngle(vec, vec0);
                        var angle1 = GetRoundAngle(vec, vec1);
                        //var area = Vector2Util.CalcArea(p, next1, next0);
                        //if (area.EqualZero(Epsilon))//在同一直线上,用前一点来判定
                        {
                            //area = Vector2Util.CalcArea(prev, next1, next0);
                        }

                        if ((angle0 < angle1 && clockwise) || (angle0 > angle1 && !clockwise))//切换链路
                        //if ((area > 0) != clockwise)//切换链路
                        {
                            atList0 = true;
                            i = (iTemp + 1) % list0.Count;
                            j = -2;
                        }
                        else
                        {
                            j = (j + 1) % list1.Count;
                        }
                    }
                    else
                    {
                        var p = GetPos1(list1[j]);
                        var index = result.IndexOf(p);
                        if (index != -1)
                        {
                            result.RemoveRange(0, index);
                            break;
                        }
                        else result.Add(p);

                        j = (j + 1) % list1.Count;
                    }
                    
                }
            }
            while (i!= iStart && j != jStart);
            return result;
        }
        
        /// <summary>
        /// 分析两个多边形所有线段的关系得到两条链表和交点映射表.
        /// </summary>
        static Dictionary<float, float> GetPolygonRelationship( List<Vector2> poly0, List<Vector2> poly1, List<float> list0, List<float> list1,float Epsilon = 0.001f)
        {
            //保证两多边形顶点都是顺时针排列
            if (CalcArea(poly0) > 0) poly0.Reverse();
            if (CalcArea(poly1) > 0) poly1.Reverse();

            for (int i = 0; i < poly0.Count; i++)
            {
                list0.Add(i);
            }
            
            for (int i = 0; i < poly1.Count; i++)
            {
                list1.Add(i);
            }
            //两多边形相交的端点映射
            var dic = new Dictionary<float, float>();//两个链表中同样的点的索引映射表

            System.Action<float, float> Link0To1 = (t0, t1) =>
            {
                t0 %= poly0.Count;
                t1 %= poly1.Count;
                dic[t0] = t1;
                if (!list0.Contains(t0)) list0.Add(t0);
                if (!list1.Contains(t1)) list1.Add(t1);
            };
            //计算多边形交点
            for (int i0 = 0; i0 < poly0.Count; i0++)
            {
                for (int i1 = 0; i1 < poly1.Count; i1++)
                {
                    var line0_a = poly0[i0];
                    var line0_b = poly0.GetItemByRound(i0 + 1);
                    var line1_a = poly1[i1];
                    var line1_b = poly1.GetItemByRound(i1 + 1);

                    //两条线段的位置关系可以分为三类：有重合部分、无重合部分但有交点、无交点
                    var p = Vector2Util.TryIntersect(line0_a, line0_b, line1_a, line1_b, 0);
                    if (p == null)//两条线段平行,或者同一直线上
                    {
                        float tA0;
                        var sameLine = line0_a.InSegment(line1_a, line1_b, out tA0, Epsilon);

                        if (sameLine)//在同一直线上
                        {
                            if (0 <= tA0 && tA0 <= 1)
                            {
                                Link0To1(i0, i1 + tA0);
                            }
                            var tB0 = LerpT(line1_a, line1_b, line0_b, Epsilon);
                            if (0 <= tB0 && tB0 <= 1)
                            {
                                Link0To1(i0 + 1, i1 + tB0);
                            }
                            var tA1 = LerpT(line0_a, line0_b, line1_a, Epsilon);
                            if (0 <= tA1 && tA1 <= 1)
                            {
                                Link0To1(i0 + tA1, i1);
                            }
                            var tB1 = LerpT(line0_a, line0_b, line1_b, Epsilon);
                            if (0 <= tB1 && tB1 <= 1)
                            {
                                Link0To1(i0 + tB1, i1 + 1);
                            }
                        }
                    }
                    else
                    {
                        var t0 = LerpT(line0_a, line0_b, (Vector2)p, Epsilon);
                        var t1 = LerpT(line1_a, line1_b, (Vector2)p, Epsilon);
                        if (0 <= t0 && t0 <= 1 && 0 <= t1 && t1 <= 1)//交点是两条线段的交点
                        {
                            if (t0 != 0 && t0 != 1)
                            {
                                if (!list0.Contains(t0 + i0)) list0.Add(t0 + i0);
                            }
                            if (t1 != 0 && t1 != 1)
                            {
                                if (!list1.Contains(t1 + i1)) list1.Add(t1 + i1);
                            }
                            dic[(t0 + i0) % poly0.Count] = (t1 + i1) % poly1.Count;
                        }
                    }
                }
            }
            list0.Sort();
            list1.Sort();
            return dic;
        }
        
        

        /// <summary>
        /// 如果一个顶点的内角是180,合并这个顶点.
        /// </summary>
        static void MergeVert(IList<Vector2> list, float Epsilon = 0.001f)
        {

            int index;
            do
            {
                index = -1;
                if (list.Count < 3) break;
                for (int i = 0; i < list.Count; i++)
                {
                    var prev = list.GetItemByRound(i - 1);
                    var current = list[i];
                    var next = list.GetItemByRound(i + 1);
                    if (CalcArea(prev, current, next).EqualZero(Epsilon))
                    {
                        index = i;
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
            while (index != -1);

        }
    }
}
