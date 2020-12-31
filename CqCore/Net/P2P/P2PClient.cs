using P2P;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// P2P客户端
    /// 1.向服务端注册
    /// 2.p2p客户端通过服务端相互通信
    /// </summary>
    public class P2PClient
    {
        /// <summary>
        /// 当前所有的p2p客户端
        /// </summary>
        public Dictionary<int, ClientInfo> dic;

        public event Action OnP2PListChange;
        public int id=-1;
        ObjClient client;

        string name;
        string group;
        public bool Connected
        {
            get
            {
                return client.Connected;
            }
        }
        /// <summary>
        /// 接收其他客户端发送的消息
        /// </summary>
        public event Action<int, object> OnReceiveMsg;

        /// <summary>
        /// 远程主机断开连接
        /// </summary>
        public event Action OnDisConnect
        {
            add
            {
                client.OnDisConnect += value;
            }
            remove
            {
                client.OnDisConnect -= value;
            }
        }

        public P2PClient(string name=null,string group=null)
        {
            this.name = name;
            this.group = group;
            AssemblyUtil.RegType(typeof(CRegP2PClient).Assembly.GetTypesByNamespace("P2P"));
            client = new ObjClient();
            dic = new Dictionary<int, ClientInfo>();
        }

        
        public void Connect(string host, int port,Action<bool> OnComplete=null)
        {
            var thisType = GetType();
            var getMethod = new Causality<Type, MethodInfo>(t =>
            {
                return thisType.GetMethod("OnReceive",
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null, CallingConventions.Any,
                    new Type[] { t }, null);
            });
            client.Connect(host, port, (bl) =>
            {
                if (bl)
                {
                    CqDebug.LogInCoroutine("连接到控制台服务端");

                    if(name!=null)
                    {
                        //CqDebug.Log(string.Format("注册名字({0})",name));
                    }
                    client.Send(new CRegP2PClient()
                    { 
                        info=new ClientInfo()
                        {
                            clientName = name,
                            groupName = group,
                        }
                    });
                }
                else
                {
                    CqDebug.LogInCoroutine("连接服务端失败");
                }
                if(OnComplete!=null)
                {
                    OnComplete(bl);
                }
            });
            client.OnReceive += (x) =>
            {
                var fun = getMethod.Call(x.GetType());
                if(fun==null)
                {
                    throw new Exception("找不到这个方法OnReceive(" + x.GetType()+")");
                }
                fun.Invoke(this, new object[] { x });
            };
        }
        void UpdateList()
        {
            //CqDebug.Log("列表更新" + Torsion.Serialize(dic));
            if (OnP2PListChange != null) OnP2PListChange();
        }
        void OnReceive(SRegP2PClient data)
        {
            id = data.id;
            dic = data.dic;
            UpdateList();
        }
        void OnReceive(P2PMsg data)
        {
            if (OnReceiveMsg!=null)
            {
                var p2pData = Torsion.Deserialize(data.msg);
                {
                    OnReceiveMsg(data.srcId, p2pData);
                }
            }
        }
        
        void OnReceive(SUpdateP2P data)
        {
            switch (data.opr)
            {
                case P2PState.Add:
                    {
                        dic[data.id] = data.clientInfo;
                        UpdateList();
                    }
                    break;
                case P2PState.Del:
                    {
                        dic.Remove(data.id);
                        UpdateList();
                    }
                    break;
            }
        }

        /// <summary>
        /// 向其他p2p客户端通信
        /// </summary>
        public void SendMsg(int dstId,object msg)
        {
            client.Send(new P2PMsg()
            {
                msg = Torsion.Serialize(msg,true,true),
                srcId = id,
                dstId = dstId,
            });
        }
        /// <summary>
        /// 主动断开连接
        /// </summary>
        public void Close()
        {
            client.Close();
        }

        public void Clear()
        {
            OnReceiveMsg = null;
            OnP2PListChange = null;
            client.Clear();
            client = null;
        }
    }
}
