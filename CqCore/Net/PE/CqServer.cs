/****************************************************
	文件：PESocket.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/10/30 11:20   	
	功能：PESocekt核心类
*****************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using CqCore;

namespace PENet {
    public class CqServer
        {
        private Socket skt = null;
        public int backlog = 10;
        List<PESession> sessionLst = new List<PESession>();

        public CqServer() 
        {
            skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port) 
        {
            try 
            {
                skt.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                skt.Listen(backlog);
                skt.BeginAccept(new AsyncCallback(ClientConnectCB), skt);
                CqDebug.Log("\nServer Start Success!\nWaiting for Connecting......");
            }
            catch (Exception e) 
            {
                CqDebug.Log(e.Message);
            }
        }

        void ClientConnectCB(IAsyncResult ar) {
            try {
                Socket clientSkt = skt.EndAccept(ar);
                PESession session = new PESession();
                sessionLst.Add(session);
                session.StartRcvData(clientSkt, () => {
                    if (sessionLst.Contains(session)) {
                        sessionLst.Remove(session);
                    }
                });
            }
            catch (Exception e) {
                CqDebug.Log(e.Message);
            }
            skt.BeginAccept(new AsyncCallback(ClientConnectCB), skt);
        }

        

        public void Close() {
            if (skt != null) {
                skt.Close();
            }
        }

        public List<PESession> GetSesstionLst() {
            return sessionLst;
        }

        
    }
}