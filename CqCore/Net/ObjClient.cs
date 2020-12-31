using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CqCore
{
    /// <summary>
    /// 对象发送客户端
    /// </summary>
    public class ObjClient
    {
        const int wait = 50;//ms
        /// <summary>
        /// 远程终结点
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return (IPEndPoint)qc.Client.RemoteEndPoint;
            }
        }

        /// <summary>
        /// 本地终结点
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return (IPEndPoint)qc.Client.LocalEndPoint;
            }
        }

        QClient qc;

        /// <summary>
        /// 远程主机断开连接
        /// </summary>
        public event Action OnDisConnect
        {
            add
            {
                qc.OnDisConnect += value;
            }
            remove
            {
                qc.OnDisConnect -= value;
            }
        }

        public bool Connected
        {
            get
            {
                return qc.Connected;
            }
        }
        /// <summary>
        /// 异步线程中获取到数据，业务逻辑切回自己的线程处理数据
        /// </summary>
        public event Action<object> OnReceive;

        public ObjClient(Socket client = null)
        {
            qc = new QClient(client);
            if (client != null && client.Connected)
            {
                StartLoop();
            }
        }

        /// <summary>
        /// 异步连接服务器
        /// 连接后开启收发循环
        /// </summary>
        public void Connect(string host, int port, Action<bool> OnComplete=null)
        {
            if (qc == null) qc = new QClient();
            qc.Connect(host, port, (bl) =>
            {
                if(bl)
                {
                    StartLoop();
                }
                OnComplete(bl);
            });
        }
        /// <summary>
        /// 开启循环从接收字节队列中转换成对象
        /// </summary>
        void StartLoop()
        {
            var handle = new CancelHandle();
            
            //接收线程
            ThreadUtil.PoolCall(() =>
            {
                while (true)
                {
                    while (qc.receiveQueue.Count == 0)
                    {
                        Thread.Sleep(wait);
                    }
                    lock (qc.receiveQueue)
                    {
                        try
                        {
                            var bytes = qc.receiveQueue.Dequeue();
                            //CqDebug.Log("接收到长度:"+bytes.Length);
                            var str = Encoding.UTF8.GetString(bytes);
                            //CqDebug.Log("接收内容:" + str);
                            if(OnReceive!=null)
                            {
                                var d = Torsion.Deserialize(str);
                                OnReceive(d);
                            }
                        }
                        catch (Exception e)
                        {
                            CqDebug.Log("!!"+e,LogType.Error);
                            throw e;
                        }
                    }
                }
            }, handle);
            System.Action OnDisConnect=null;
            OnDisConnect = () =>
            {
                handle.CancelAll();
                qc.OnDisConnect -= OnDisConnect;
            };
            qc.OnDisConnect += OnDisConnect;
        }

        /// <summary>
        /// 发送数据对象
        /// </summary>
        public void Send(object value)
        {
            var str = Torsion.Serialize(value,true,true);
            var bytes = Encoding.UTF8.GetBytes(str);
            qc.Send(bytes);
        }

        /// <summary>
        /// 主动断开连接
        /// </summary>
        public void Close()
        {
            qc.Close();
        }
        public void Clear()
        {
            OnReceive = null;
            qc = null;
            GC.Collect();
        }
    }
}
