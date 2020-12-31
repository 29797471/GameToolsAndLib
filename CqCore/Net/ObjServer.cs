using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CqCore
{
    /// <summary>
    /// T服务端
    /// </summary>
    public class ObjServer
    {
        Socket mTcpListener;

        List<ObjClient> mList;
        int backlog = 10;
        /// <summary>
        /// 连接的客户端列表
        /// </summary>
        public List<ObjClient> List { get { return mList; } }

        public int Port { get; private set; }

        /// <summary>
        /// 当接入一个客户端时自动开启收发循环
        /// </summary>
        public event System.Action<ObjClient> OnAccept;
        public ObjServer(int port, string ip = null)
        {
            Port = port;
            mTcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (ip == null)
            {
                ip = NetUtil.LocalIP;
            }
            mTcpListener.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

            mTcpListener.Listen(backlog);
            mList = new List<ObjClient>();
        }
        public void Start(ICancelHandle handle =null)
        {
            ThreadUtil.PoolCall(() =>
            {
                while (true)
                {
                    //if (mTcpListener.Pending())
                    {
                        var tcpClient= mTcpListener.Accept();
                        var client = new ObjClient(tcpClient);
                        mList.Add(client);
                        System.Action fun = null;
                        fun=() =>
                        {
                            client.OnDisConnect -= fun;
                            mList.Remove(client);
                        };
                        client.OnDisConnect += fun;
                        if (OnAccept != null) OnAccept(client);
                    }
                }
            }, handle);
        }
    }
}
