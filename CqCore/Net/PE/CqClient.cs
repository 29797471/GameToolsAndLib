using CqCore;
using System;
using System.Net;
using System.Net.Sockets;

namespace PENet
{
    public class CqClient
    {
        Socket skt;
        PESession session;

        /// <summary>
        /// Launch Client
        /// </summary>
        public PESession Connect(string ip, int port)
        {
            try
            {
                skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                session = new PESession();
                skt.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ServerConnectCB), skt);
                CqDebug.Log("\nClient Start Success!\nConnecting To Server......");
                return session;
            }
            catch (Exception e)
            {
                CqDebug.Log("Connect:"+e);
                return null;
            }
        }

        void ServerConnectCB(IAsyncResult ar)
        {
            try
            {
                skt.EndConnect(ar);
                session.StartRcvData(skt, null);
            }
            catch (Exception e)
            {
                CqDebug.Log("ServerConnectCB:" + e);
            }
        }
    }
}
