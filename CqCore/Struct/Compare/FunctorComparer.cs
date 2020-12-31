using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 内置比较器
    /// </summary>
    internal sealed class FunctorComparer<T> : IComparer<T>
    {
        private Comparison<T> comparison;

        private Comparer<T> c = Comparer<T>.Default;

        public FunctorComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        public FunctorComparer(ComparisonPriority<T> cp)
        {
            this.comparison = (xx, yy) => cp(xx) - cp(yy);
        }
        public FunctorComparer(ComparisonFloatPriority<T> cp)
        {
            this.comparison = (xx, yy) => System.Math.Sign(cp(xx) - cp(yy));
        }

        public int Compare(T x, T y)
        {
            return comparison(x, y);
        }
    }
}
