using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace CqCore
{
    /// <summary>
    ///常用算法类
    /// </summary>
    public static partial class MathUtil
    {

        /// <summary>
        /// 得到一个通用排序的函数
        /// </summary>
        public static Func<object, object, int> Comparison(Type t)
        {
            Func<object, object, int> call = null;
            if (t == typeof(int))
            {
                call = (x, y) => Convert.ToInt32(x) - Convert.ToInt32(y);
            }
            else if (t == typeof(float))
            {
                call = (x, y) =>Math.Sign( Convert.ToDouble(x) - Convert.ToDouble(y));
            }
            else if (t == typeof(string))
            {
                call = (x, y) => string.Compare(x.ToString(),y.ToString());
            }
            return call;
        }
        public static void QuickSort(int[] numbers)
        {
            QuickSort(numbers, 0, numbers.Length - 1);
        }

        private static void QuickSort(int[] numbers, int left, int right)
        {
            if (left < right)
            {
                int middle = numbers[(left + right) / 2];
                int i = left - 1;
                int j = right + 1;
                while (true)
                {
                    while (numbers[++i] < middle) ;

                    while (numbers[--j] > middle) ;

                    if (i >= j)
                        break;

                    Swap(numbers, i, j);
                }

                QuickSort(numbers, left, i - 1);
                QuickSort(numbers, j + 1, right);
            }
        }

        public static void Swap(IList numbers, int i, int j)
        {
            object number = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = number;
        }

        

    }
}


