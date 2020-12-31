using System.Collections.Generic;

namespace P2P
{
    /// <summary>
    /// 注册游戏客户端,当名称为空时其它客户端不可见
    /// </summary>
    public class CRegP2PClient
    {
        public ClientInfo info;
    }

    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientInfo
    {
        public string clientName;
        public string groupName;
        public override string ToString()
        {
            return string.Format("{0}({1})", clientName,groupName);
        }
    }

    /// <summary>
    /// 返回注册的客户端列表
    /// </summary>
    public class SRegP2PClient
    {
        public int id;

        /// <summary>
        /// 当前注册了的客户端列表
        /// </summary>
        public Dictionary<int, ClientInfo> dic;
    }

    /// <summary>
    /// 1.添加 2.删除
    /// </summary>
    public enum P2PState
    {
        None,
        Add,
        Del,
    }
    /// <summary>
    /// 更新其它P2P客户端
    /// </summary>
    public class SUpdateP2P
    {
        public P2PState opr;
        public int id;
        public ClientInfo clientInfo;
    }

    /// <summary>
    /// P2P客户端相互传递消息
    /// </summary>
    public class P2PMsg
    {
        public int srcId;
        public int dstId;
        public string msg;
    }

    /// <summary>
    /// 向其他客户端通知自己的开放端口,ip
    /// </summary>
    public class P2P_ClientServerInfo
    {
        public string ip;
        public int port;
        public int id;
    }
    /// <summary>
    /// 向其他客户端通知自己的id
    /// </summary>
    public class P2P_ClientId
    {
        public int id;
    }
}
