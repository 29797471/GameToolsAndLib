using P2P;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// P2P服务端
    /// 1.给连接的p2p客户端分配id
    /// 2.维护所有P2P客户端列表
    /// 3.提供p2p客户端相互通信
    /// </summary>
    public class P2PServer
    {
        ObjServer x;

        /// <summary>
        /// 注册了的p2p客户端列表
        /// </summary>
        Dictionary<int, ClientInfo> regDic;
        public P2PServer(int port)
        {
            AssemblyUtil.RegType(typeof(CRegP2PClient).Assembly.GetTypesByNamespace("P2P"));
            x = new ObjServer(port);
            regDic = new Dictionary<int, ClientInfo>();
            CqDebug.LogInCoroutine("IP=" + NetUtil.LocalIP + "\tport=" + port);
        }
        public void Start(ICancelHandle handle =null)
        {
            x.Start(handle);
            var thisType = GetType();
            var getMethod = new Causality<Type, MethodInfo>(t => 
            {
                return thisType.GetMethod("OnReceive", new Type[] {typeof(ObjClient), t });
            });
            CqDebug.LogInCoroutine("P2P服务器启动成功");

            Action<ObjClient> OnAcceptClient = (client) =>
            {
                var clientId=client.GetHashCode();
                
                client.OnReceive += (e) =>
                {
                    var fun = getMethod.Call(e.GetType());
                    fun.Invoke(this, new object[] { client,e });
                };
                client.OnDisConnect += () =>
                {
                    if(regDic.ContainsKey(clientId))
                    {
                        regDic.Remove(clientId);
                        ForEach(it =>
                        {
                            it.Send(new SUpdateP2P() { opr = P2PState.Del, id = clientId });
                        });
                        UpdateList();
                    }
                    client.Clear();

                    CheckCount();
                };
            };
            x.OnAccept += OnAcceptClient;
            if(handle!=null)
            {
                handle.CancelAct += () => x.OnAccept -= OnAcceptClient;
            }
        }
        

        public void OnReceive(ObjClient client,CRegP2PClient data)
        {
            var clientId = client.GetHashCode();
            regDic[clientId] = data.info;
            client.Send(new SRegP2PClient() { id= clientId , dic= regDic});
            ForEach(it =>
            {
                if(it!=client)
                {
                    it.Send(new SUpdateP2P() { opr = P2PState.Add, id = clientId, clientInfo = data.info });
                }
            });
            UpdateList();
            CheckCount();
        }

        /// <summary>
        /// 遍历p2p客户端
        /// </summary>
        void ForEach(System.Action<ObjClient> callBack)
        {
            x.List.ForEach(it =>
            {
                if(regDic.ContainsKey(it.GetHashCode()))
                {
                    callBack(it);
                }
            });
        }
        public void OnReceive(ObjClient client, P2PMsg data)
        {
            var dstClient=x.List.Find(it => it.GetHashCode() == data.dstId);
            if(dstClient!=null)
            {
                dstClient.Send(data);
            }
        }
        void UpdateList()
        {
            CqDebug.LogInCoroutine("注册列表\n" + Torsion.Serialize(regDic));
        }
        void CheckCount()
        {
            CqDebug.LogInCoroutine("线程数:" + System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
            if(System.Diagnostics.Process.GetCurrentProcess().Threads.Count>80)
            {
                Environment.Exit(0);
            }
        }
    }
}
