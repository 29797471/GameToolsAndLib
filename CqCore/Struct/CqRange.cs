using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 对应数轴上的一个区间(前后都是闭合的区间,可以看成一个线段)
    /// </summary>
    public struct CqRange
    {
        public float min;
        public float max;

        public CqRange(float a,float b)
        {
            if(a>b)
            {
                min = b;
                max = a;
            }
            else
            {
                min = a;
                max = b;
            }
        }

        /// <summary>
        /// 区间并集<para/>
        /// 两区间不重叠也不相邻时返回null
        /// </summary>
        public static CqRange? operator +(CqRange a,CqRange b)
        {
            var min = Math.Max(a.min, b.min);
            var max = Math.Min(a.max, b.max);
            if (min>max) return null;
            return new CqRange(Math.Min(a.min, b.min), Math.Max(a.max, b.max));
        }

        /// <summary>
        /// 区间差集<para/>
        /// ab不重叠时返回null<para/>
        /// b完全覆盖a时返回长度为0的列表
        /// </summary>
        public static List<CqRange> operator -(CqRange a, CqRange b)
        {
            if (b.min <= a.min && b.max >= a.max)
            {
                return new List<CqRange>();
            }
            else if(b.min>a.min && b.max<a.max)
            {
                return new List<CqRange>()
                {
                    new CqRange(a.min,b.min),
                    new CqRange(b.max,a.max),
                };
            }
            else if(b.min>a.min && b.min<a.max)
            {
                return new List<CqRange>()
                {
                    new CqRange(a.min,b.min),
                };
            }
            else if (b.max < a.max && b.max>a.min)
            {
                return new List<CqRange>()
                {
                    new CqRange(b.max,a.max),
                };
            }
            return null;
        }
        public override bool Equals(object obj)
        {
            if(obj is CqRange)
            {
                var b = (CqRange)obj;
                return min == b.min && max == b.max;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -897720056;
            hashCode = hashCode * -1521134295 + min.GetHashCode();
            hashCode = hashCode * -1521134295 + max.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// 区间交集
        /// </summary>
        public static CqRange? operator *(CqRange a, CqRange b)
        {
            var min = Math.Max(a.min, b.min);
            var max = Math.Min(a.max, b.max);
            if (min >= max) return null;
            return new CqRange(min, max);
        }
    }
}
