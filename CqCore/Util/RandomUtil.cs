using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    public static class RandomUtil
    {
        /// <summary>
        /// 返回min,max之间的浮点数
        /// </summary>
        public static float Random(float min, float max)
        {
            return (float)CqRandom.NextDouble()*(max-min)+min;
        }
        /// <summary>
        /// 返回大于等于min,小于max的整数
        /// </summary>
        public static int Random(int min, int max)
        {
            return CqRandom.Next(min, max);
        }
        /// <summary>
        /// 随机获取一个列表元素
        /// </summary>
        public static object RandomIt(IList list)
        {
            return list[CqRandom.Next(list.Count)];
        }


        /// <summary>
        /// 随机获取一个列表元素
        /// </summary>
        public static T RandomIt_T<T>(IList<T> list)
        {
            return list[CqRandom.Next(list.Count)];
        }
        /// <summary>
        /// 乱序数组
        /// </summary>
        public static void RandomShuffle(IList list)
        {
            int count = list.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var index = Random(0, i);
                MathUtil.Swap(list, index, i);
            }
        }

        /// <summary>
        /// 从若干个元素中,随机取出n个元素组成列表<para/>
        /// </summary>
        /// <param name="weightList">每个元素对应的权重组成的列表</param>
        /// <param name="num">随机抽取数量</param>
        /// <param name="repeat">是否可以重复抽取</param>
        /// <returns>返回随机到的元素在权重列表对应的索引</returns>
        public static List<int> GetIndexs(IList<float> weightList, int num, bool repeat = true)
        {
            var totalWeight = 0f;

            var axisList = weightList.ToList();//权重数轴
            for (int i = 0; i < axisList.Count; i++)
            {
                totalWeight += axisList[i];
            };
            var result = new List<int>();
            if (!repeat) num = Math.Min(num, weightList.Count);
            while (result.Count < num)
            {
                var v = (float)CqRandom.NextDouble() * totalWeight;
                for (var i = 0; i < axisList.Count; i++)
                {
                    if (!repeat && result.Contains(i))
                    {
                        continue;
                    }
                    if (v < axisList[i])
                    {
                        result.Add(i);
                        if (!repeat) totalWeight -= axisList[i];
                        break;
                    }
                    else
                    {
                        v -= axisList[i];
                    }
                }
            }
            return result;
        }
    }
}
