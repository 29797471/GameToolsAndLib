/****************************************************
	文件：PESession.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/10/30 11:20   	
	功能：网络会话管理
*****************************************************/

using CqCore;
using System;
using System.Net.Sockets;

namespace PENet
{
    public class PESession
    {
        /// <summary>
        /// Add length info to package
        /// </summary>
        public static byte[] PackLenInfo(byte[] data)
        {
            int len = data.Length;
            byte[] pkg = new byte[len + 4];
            byte[] head = BitConverter.GetBytes(len);
            head.CopyTo(pkg, 0);
            data.CopyTo(pkg, 4);
            return pkg;
        }

        private Socket skt;
        private Action closeCB;

        #region Recevie
        public void StartRcvData(Socket skt, Action closeCB) {
            try {
                this.skt = skt;
                this.closeCB = closeCB;

                if (OnConnected != null) OnConnected();

                PEPkg pack = new PEPkg();
                skt.BeginReceive(
                    pack.headBuff,
                    0,
                    pack.headLen,
                    SocketFlags.None,
                    new AsyncCallback(RcvHeadData),
                    pack);
            }
            catch (Exception e) {
                CqDebug.Log("StartRcvData:" + e.Message);
            }
        }

        private void RcvHeadData(IAsyncResult ar) {
            try {
                PEPkg pack = (PEPkg)ar.AsyncState;
                if (skt.Available == 0) {
                    OnDisConnected();
                    Clear();
                    return;
                }
                int len = skt.EndReceive(ar);
                if (len > 0) {
                    pack.headIndex += len;
                    if (pack.headIndex < pack.headLen) {
                        skt.BeginReceive(
                            pack.headBuff,
                            pack.headIndex,
                            pack.headLen - pack.headIndex,
                            SocketFlags.None,
                            new AsyncCallback(RcvHeadData),
                            pack);
                    }
                    else {
                        pack.InitBodyBuff();
                        skt.BeginReceive(pack.bodyBuff,
                            0,
                            pack.bodyLen,
                            SocketFlags.None,
                            new AsyncCallback(RcvBodyData),
                            pack);
                    }
                }
                else {
                    OnDisConnected();
                    Clear();
                }
            }
            catch (Exception e) {
                CqDebug.Log("RcvHeadError:" + e.Message);
            }
        }

        private void RcvBodyData(IAsyncResult ar) {
            try {
                PEPkg pack = (PEPkg)ar.AsyncState;
                int len = skt.EndReceive(ar);
                if (len > 0) {
                    pack.bodyIndex += len;
                    if (pack.bodyIndex < pack.bodyLen) {
                        skt.BeginReceive(pack.bodyBuff,
                            pack.bodyIndex,
                            pack.bodyLen - pack.bodyIndex,
                            SocketFlags.None,
                            new AsyncCallback(RcvBodyData),
                            pack);
                    }
                    else {
                        OnReciveBytes(pack.bodyBuff);

                        //loop recive
                        pack.ResetData();
                        skt.BeginReceive(
                            pack.headBuff,
                            0,
                            pack.headLen,
                            SocketFlags.None,
                            new AsyncCallback(RcvHeadData),
                            pack);
                    }
                }
                else {
                    OnDisConnected();
                    Clear();
                }
            }
            catch (Exception e) {
                CqDebug.Log("RcvBodyError:" + e.Message);
            }
        }
        #endregion

        #region Send

        /// <summary>
        /// Send binary data
        /// </summary>
        public void SendBytes(byte[] data) {
            NetworkStream ns = null;
            try {
                ns = new NetworkStream(skt);
                if (ns.CanWrite) {
                    ns.BeginWrite(
                        data,
                        0,
                        data.Length,
                        new AsyncCallback(SendCB),
                        ns);
                }
            }
            catch (Exception e) {
                CqDebug.Log("SndMsgError:" + e.Message);
            }
        }

        private void SendCB(IAsyncResult ar) {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            try {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
            }
            catch (Exception e) {
                CqDebug.Log("SndMsgError:" + e.Message);
            }
        }
        #endregion

        /// <summary>
        /// Release Resource
        /// </summary>
        private void Clear() {
            if (closeCB != null) {
                closeCB();
            }
            OnConnected = null;
            OnReciveBytes = null;
            OnDisConnected = null;
            skt.Close();
        }

        /// <summary>
        /// Connect network
        /// </summary>
        public event Action OnConnected = () => { CqDebug.LogInCoroutine("OnConnected"); };

        /// <summary>
        /// Receive network message
        /// </summary>
        public event Action<byte[]> OnReciveBytes= (bytes) => { CqDebug.LogInCoroutine("OnReciveBytes:"+bytes.Length); };

        /// <summary>
        /// Disconnect network
        /// </summary>
        public event Action OnDisConnected = () => { CqDebug.LogInCoroutine("OnDisConnected"); };
    }
}