using System;
using System.Collections.Generic;
using System.Text;

namespace NetMQ
{
    /// <summary>
    ///  .net开发中计算的都是标准时区的差，但java的解析时间跟时区有关，
    ///  而我们的java服务器系统时区不是标准时区，解析时间会差8个小时。
    ///  传入java格式的(格林尼治时间)
    /// </summary>
    public class CustomDate
    {
        DateTime dt_1970 = new DateTime(1970, 1, 1);
        private DateTime date;

        public CustomDate(long milliseconds)
        {
            date = JavaTimeToCSharpTime(milliseconds);
        }
        public string timeForamt
        {
            get
            {
                return ToString();
            }
        }
        override public string ToString()
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public DateTime JavaTimeToCSharpTime(long milliseconds)
        {
            return dt_1970 + TimeSpan.FromMilliseconds(milliseconds) + TimeSpan.FromHours(8);
        }
        /// <summary>
        /// 隐式强转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(CustomDate value)
        {
            return value.ToString();
        }
    }
}
