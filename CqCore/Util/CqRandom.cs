using CqCore;
using System;
namespace CqCore
{
    public static class CqRandom
    {
        static Random random;
        static int mSeed;

        /// <summary>
        /// 随机种子
        /// </summary>
        public static int Seed
        { 
            get
            {
                return mSeed;
            }
            set
            {
                mSeed = value;
                random = new Random(mSeed);
            }
        }
        static CqRandom()
        {
            long tick = DateTime.Now.Ticks;
            Seed = (int)(tick & 0xffffffffL) | (int)(tick >> 32);
        }
        /// <summary>
        /// 返回x(0&lt;=x&lt;int.MaxValue)
        /// </summary>
        public static int Next()
        {
            return random.Next();
        }
        /// <summary>
        /// 返回x(0&lt;=x&lt;maxValue)
        /// </summary>
        public static int Next(int maxValue)
        {
            return random.Next(maxValue);
        }
        /// <summary>
        /// 返回大于等于minValue,小于maxValue的整数
        /// </summary>
        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
        public static void NextBytes(byte[] buffer)
        {
            random.NextBytes(buffer);
        }

        /// <summary>
        /// 返回0~1之间的浮点数
        /// </summary>
        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
