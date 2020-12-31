using CqCore;

namespace P2P
{

    public class LogMsgGet
    {
        public string condition;
        public string stackTrace;
        public LogType type;
    }

    
    /// <summary>
    /// 命令端向游戏端下达指令
    /// </summary>
    public class OrderSend
    {
        /// <summary>
        /// 0.断开向命令端发送日志<para/>
        /// 1.记录命令端添加到日志发送列表<para/>
        /// 2.执行脚本命令<para/>
        /// 3.生成内存快照后发送到命令端
        /// 4.生成Hierarchy树
        /// </summary>
        public int opr;

        public string command;
    }

    /// <summary>
    /// 返回游戏端信息
    /// </summary>
    public class OrderBack
    {
        /// <summary>
        /// 操作返回
        /// </summary>
        public int opr;

        /// <summary>
        /// 操作数据返回
        /// </summary>
        public string data;

        /// <summary>
        /// 当前游戏端状态
        /// </summary>
        public string state;
    }
}