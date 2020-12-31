using System;
using System.Collections.Generic;
using System.Linq;

namespace CqCore
{
    /// <summary>
    ///常用算法类
    /// </summary>
    public static partial class MathUtil
    {

        /// <summary>
        /// 保留digits位有效数字(向下取整)
        /// </summary>
        public static double KeepNumber(double v, int digits = 4)
        {
            if (v == 0)
            {
                return 0;
            }
            int scale = 1;
            var min = Math.Pow(10, digits - 1);
            var max = Math.Pow(10, digits);
            if (v >= max)
            {
                while (v >= max)
                {
                    v /= 10; scale *= 10;
                }
                return v.FloorByEpsilon() * scale;
            }
            else
            {
                while (v < min)
                {
                    v *= 10; scale *= 10;
                }
                return v.FloorByEpsilon() / scale;
            }
        }

        /// <summary>
        /// 标准差
        /// </summary>
        public static float CalculateStdDev(IEnumerable<float> values)
        {
            float ret = 0;
            if (values.Count() > 0)
            {
                //  计算平均数   
                float avg = values.Average();
                //  计算各数值与平均数的差值的平方，然后求和 
                float sum = values.Sum(d => (float)Math.Pow(d - avg, 2));
                //  除以数量，然后开方
                ret = (float)Math.Sqrt(sum / values.Count());
            }
            return ret;
        }
        /// <summary>
        /// 整数除法 返回余数 原始数据改为商
        /// </summary>
        public static int Mod(ref int data, int k)
        {
            int mod = data % k;
            data = (data - mod) / k;
            return mod;
        }

        /// <summary>
        /// 数轴的逆序数
        /// </summary>
        public static int InverseNumber(params float[] data)
        {
            int count = 0;
            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = i + 1; j < data.Length; j++)
                {
                    if (data[i] > data[j]) count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 数在数轴上的区间位置（从0开始） args从小到大排列<para/>
        /// t为该数在所在区间的插值系数<para/>
        /// 每区间前闭后开<para/>
        /// 比如:value=2.5,args={0,2,3,9} ,args把 数轴分成5个区间(-无穷大,0),[0,2),[2,3),[3,9),[9,+无穷大),而2.5在第3个区间上所以返回索引2,2.5在2~3之间的插值为t=0.5
        /// </summary>
        public static int GetIndexOfExtent(float value, out float t, IList<float> axisList)
        {
            var list = axisList.ToList();
            list.Sort();
            int i = 0;
            for (; i < list.Count; ++i)
            {
                if (value < list[i])
                {
                    t = 0;
                    
                    if (i != 0) t = (value - list[i - 1]) / (list[i] - list[i - 1]);
                    return i;
                }
            }
            t = 1;
            return i;
        }

        /// <summary>
        /// 确保value在start和end之间,
        /// 如果value比start小,value=start,
        /// 如果value比end大,value=end
        /// </summary>
        public static void BetweenRange(ref float value, float start, float end)
        {
            if (start > end)
            {
                var temp = start;
                start = end;
                end = start;
            }
            value = Math.Min(value, end);
            value = Math.Max(value, start);
        }

        /// <summary>
        /// 确保value在start和end之间,
        /// 如果value比start小,value=start,
        /// 如果value比end大,value=end
        /// </summary>
        public static void BetweenRange(ref int value, int start, int end)
        {
            if (start > end)
            {
                var temp = start;
                start = end;
                end = start;
            }
            value = Math.Min(value, end);
            value = Math.Max(value, start);
        }


        /// <summary>
        /// 将数据按区间的范围为单位,移动到区间内,一般用于角度计算(start,end)
        /// 例:MoveToRange( -60,0,360) 结果为300 
        /// </summary>
        public static float MoveToRange(float value, float min, float max)
        {
            var range = max - min;
            var d = (value - min) % range;
            return min + (d + range) % range;
        }
        /// <summary>
        /// 将数据按区间的范围为单位,移动到区间内,一般用于角度计算(start,end)
        /// 例:MoveToRange( -60,0,360) 结果为300 
        /// </summary>
        public static int MoveToRange(int value, int min, int max)
        {
            var range = max - min;
            var d = (value - min) % range;
            return min + (d + range) % range;
        }

        /// <summary>
        ///是否在弹道距离内 
        /// </summary>
        public static bool InMassileRange(float speed, float distance)
        {
            return (speed * speed / 9.8) > distance ? true : false;
        }

        /// <summary>
        /// (高效copy)非清空的方式（添加没有的，删除多余的），将一个表赋值给另一个表
        /// </summary>
        public static void CompareCopy<T>(List<T> source, List<T> dst)
        {
            //删掉多余的
            for (int i = 0; i < dst.Count; i++)
            {
                var it = dst[i];
                if (!source.Contains(it))
                {
                    dst.Remove(it);
                }
            }
            //添加没有的
            for (int i = 0; i < source.Count; i++)
            {
                var it = source[i];
                if (!dst.Contains(it))
                {
                    dst.Add(it);
                }
            }
        }

       
        

        /// <summary>
        /// 角度转弧度
        /// </summary>
        public static double DegreeToRadianConvert(double degree)
        {
            return (degree * Math.PI) / 180;
        }

    }
}


