using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 因果关系<para/>
    /// 2输入1输出
    /// </summary>
    public class Causality2_1<TKey1, TKey2, TValue>
    {
        Dictionary<TKey1, Dictionary<TKey2, TValue>> dic;

        /// <summary>
        /// 双键字典
        /// </summary>
        public Causality2_1()
        {
            dic = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
        }

        /// <summary>
        /// 由双键获取值
        /// </summary>
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (!ContainsKey(key1,key2)) return default(TValue);
                return dic[key1][key2];
            }
            set
            {
                if (!dic.ContainsKey(key1))dic[key1] = new Dictionary<TKey2, TValue>();
                dic[key1][key2] = value;
            }
        }

        /// <summary>
        /// 是否有这个双键映射
        /// </summary>
        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return dic.ContainsKey(key1) && dic[key1].ContainsKey(key2);
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            dic.Clear();
        }
    }
}
