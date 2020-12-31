using System.Collections.Generic;
using System.IO;

namespace ParserCore
{
    /// <summary>
    /// 用于 深度优先解析 的数据结构
    /// 支持 匹配 回溯
    /// </summary>
    public class BaseParser<T>
    {
        List<T> list;
        int pos;
        Backtracking backtracking;
        public T Value
        {
            get
            {
                return list[pos];
            }
        }
        public T GetOffsetValue(int offset)
        {
            return list[pos + offset];
        }
        public BaseParser(List<T> list)
        {
            this.list = list;
            backtracking = new Backtracking();


        }

        /// <summary>
        /// 返回true,表示到结尾了
        /// </summary>
        public bool Next()
        {
            pos++;
            return pos == list.Count;
        }
        public void EnterParser()
        {

            backtracking.Enter(pos);
        }
        /// <summary>
        /// 测试打印
        /// 用于序列化时显示处理到的位置
        /// </summary>
        public string TestPosPrev
        {
            get
            {
                return TestPrint(0, pos-1);
            }
        }
        /// <summary>
        /// 测试打印
        /// 用于序列化时显示处理到的位置
        /// </summary>
        public string TestPosNext
        {
            get
            {
                return TestPrint(pos, list.Count - 1);
            }
        }

        string TestPrint(int startIndex, int endIndex)
        {
            var sw = new StringWriter();
            for (var i = startIndex; i < endIndex; i++)
            {
                sw.Write(list[i].ToString());
                sw.Write(" ");
            }
            sw.Write(list[endIndex].ToString());
            return sw.ToString();
        }
        /// <summary>
        /// 解析失败时回溯
        /// </summary>
        public void Back()
        {
            pos = backtracking.Back();
        }


        public bool IsEnd()
        {
            return pos == list.Count;
        }
    }
}
