using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WinCore
{
    internal class Comparer<T> : IComparer<T>
    {
        private readonly Comparison<T> comparison;

        public Comparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }
        #region IComparer<T> Members  
        public int Compare(T x, T y)
        {
            return comparison.Invoke(x, y);
        }
        #endregion
    }
}
