using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace CqCore
{
    /// <summary>
    /// 局域网遍历
    /// </summary>
    public static class PingUtil
    {
        public static void EnumComputers()
        {
            try
            {
                for (int i = 1; i <= 255; i++)
                {
                    Ping myPing;
                    myPing = new Ping();
                    myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);

                    string pingIP = "192.168.0." + i.ToString();
                    myPing.SendAsync(pingIP, 1000, null);
                }
            }
            catch
            {
            }
        }

        static void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine(e.Reply.Address.ToString() + "|" + Dns.GetHostEntry(IPAddress.Parse(e.Reply.Address.ToString())).HostName);
            }

        }

        /// <summary>
        /// 传入域名返回对应的数字IPv4
        /// </summary>
        public static string GetIPv4(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address.ToString();
        }

        /// <summary>
        /// 再另一线程中下载完成后在主线程中返回
        /// </summary>
        public static IEnumerator GetIPv4(string domain,  AsyncReturn<string> cqReturn)
        {
            yield return GlobalCoroutine.ThreadPoolCall(() =>
            {
                cqReturn.data = GetIPv4(domain);
            });
        }
    }
}
