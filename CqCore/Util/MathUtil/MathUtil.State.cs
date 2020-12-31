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
        //应用：计算字符串的长度（一个双字节字符长度计2，ASCII字符计1）
        //String.prototype.len=function(){return this.replace([^\x00-\xff]/g,"aa").length;}

        //得到一个低4位全为1,其余全为0的数。可用~(~0<<4)

        //打开n中的bit位   常用于状态添加
        //n|=bit     

        //切换n中的bit位   常用于状态切换
        //n^=bit;

        //关闭n中的bit位   常用于状态删除
        //n&=~bit;

        //测试位值         常用于状态判断
        //n&bit==bit 或 n&bit


        /// <summary>
        /// 状态判断
        /// </summary>
        public static bool StateCheck(int n, int bit)
        {
            return (n& bit) == bit;
        }

        /// <summary>
        /// 返回状态相加的结果
        /// </summary>
        public static int StateAdd(int n, int bit)
        {
            return n | bit;
        }

        /// <summary>
        /// 返回状态切换后的结果
        /// </summary>
        public static int StateChange(int n, int bit)
        {
            return n ^ bit;
        }

        /// <summary>
        /// 返回从n中删除bit的结果
        /// </summary>
        public static int StateDel(int n, int bit)
        {
            return n & (~bit);
        }

        /// <summary>
        /// 状态判断
        /// </summary>
        public static bool StateCheck<T>(T n, T bit)
        {
            return StateCheck(Convert.ToInt32(n), Convert.ToInt32(bit));
        }

        /// <summary>
        /// 返回状态相加的结果
        /// </summary>
        public static T StateAdd<T>(T n, T bit)
        {
            return ConvertUtil.ChangeType<T>(StateAdd(Convert.ToInt32(n) , Convert.ToInt32(bit)));
        }


        /// <summary>
        /// 返回状态切换后的结果
        /// </summary>
        public static T StateChange<T>(T n, T bit)
        {
            return ConvertUtil.ChangeType<T>(StateChange(Convert.ToInt32(n), Convert.ToInt32(bit)));
        }

        /// <summary>
        /// 返回从n中删除bit的结果
        /// </summary>
        public static T StateDel<T>(T n, T bit)
        {
            return ConvertUtil.ChangeType<T>(StateDel(Convert.ToInt32(n) ,Convert.ToInt32(bit)));
        }
    }
}


