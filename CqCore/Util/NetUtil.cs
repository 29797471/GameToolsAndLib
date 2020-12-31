using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CqCore
{
    public static class NetUtil
    {
        

        /// <summary>
        /// PING测试
        /// </summary>
        public static bool ConnectTest(string ip, int port)
        {
            IPAddress ipp = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(ipp, port);
            try
            {
                TcpClient tcp = new TcpClient();
                tcp.Connect(point);
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 返回启用的网卡中的一个本地IP
        /// </summary>
        public static string LocalIP
        {
            get
            {
                var list = HostIPList;
                if (list.Count > 0) return list[0];
                return null;
            }
        }

        /// <summary>
        /// 返回IP列表
        /// </summary>
        public static List<string> HostIPList
        {
            get
            {
                return Host.AddressList.ToList()
                    .FindAll(x => x.AddressFamily == AddressFamily.InterNetwork)
                    .ConvertAll(y=>y.ToString());
            }
        }


        /// <summary>
        /// 返回主机
        /// </summary>
        public static IPHostEntry Host
        {
            get { return Dns.GetHostEntry(HostName); }
        }

        /// <summary>
        /// 返回主机名
        /// </summary>
        public static string HostName
        {
            get { return Dns.GetHostName(); }
        }
        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称.
        /// </summary>
        public static string MachineName
        {
            get { return Environment.MachineName; }

        }
        /// <summary>
        /// 获取平台标识符,版本和当前安装在操作系统上的Service Pack
        /// </summary>
        public static string VersionString
        {
            get { return Environment.OSVersion.VersionString; }
        }
        
    }
}
