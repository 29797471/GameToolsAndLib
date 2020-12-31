using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace CqCore
{
    /// <summary>
    /// 套接字客户端
    /// 异步数据接收有可能收到的数据不是一个完整包，或者接收到的数据超过一个包的大小，
    /// 因此我们需要把接收的数据进行缓存。
    /// 异步发送我们也需要把每个发送的包加入到一个队列，然后通过队列逐个发送出去，
    /// 如果每个都实时发送，有可能造成上一个数据包未发送完成，
    /// 这时再调用SendAsync会抛出异常，提示SocketAsyncEventArgs正在进行异步操作，
    /// 因此我们需要建立接收缓存和发送缓存。
    /// https://blog.csdn.net/snow_5288/article/details/72794306
    /// </summary>
    public class QClient
    {
        const int wait = 50;//ms
        const int checkConnectLoopTime = 500;//ms
        Socket socket;
        RingBuffer receiveBuff;
        RingBuffer sendBuff;

        Queue<byte[]> sendQueue;
        public Queue<byte[]> receiveQueue;

        public Socket Client
        {
            get
            {
                return socket;
            }
        }
        public bool Connected
        {
            get
            {
                return socket!=null && socket.Connected;
            }
        }
        /// <summary>
        /// 远程主机断开连接
        /// </summary>
        public event Action OnDisConnect;

        public QClient(Socket client = null)
        {
            receiveBuff = new RingBuffer();
            sendBuff = new RingBuffer();
            sendQueue = new Queue<byte[]>();
            receiveQueue = new Queue<byte[]>();
            if (client == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                socket = client;
                StartLoop();
            }
        }

        void DisConnect()
        {
            if (OnDisConnect != null)
            {
                OnDisConnect();
            }
            if(handle!=null)
            {
                handle.CancelAll();
                handle = null;
            }
        }

        CancelHandle handle;
        /// <summary>
        /// 开启收发循环
        /// 1.接收流数据到接收缓冲
        /// 2.从发送缓冲发送数据到流
        /// </summary>
        void StartLoop()
        {
            handle = new CancelHandle();

            //检查是否断开连接
            ThreadUtil.PoolCall(() =>
            {
                while (socket.Connected)
                {
                    Thread.Sleep(checkConnectLoopTime);
                }
                DisConnect();
            });

            var stream = new NetworkStream(socket);

            #region 生产者进程
            //生产者线程
            ThreadUtil.PoolCall(() =>
            {
                while (true)
                {
                    if (sendQueue.Count == 0)
                    {
                        Thread.Sleep(wait);
                    }
                    else
                    {
                        var bytes = sendQueue.Dequeue();
                        // Console.WriteLine("原始大小" + bytes.Length);
                        //var temp_crc = CompressCRC.compress(bytes);
                        //Console.WriteLine("crc压缩优化" + (bytes.Length-temp_crc.Length));
                        //var temp_zip = Zip.Compress(bytes);
                        //Console.WriteLine("zip压缩优化" + (bytes.Length - temp_zip.Length));
                        //包体过小不压缩
                        if (bytes.Length < 200)
                        {
                            sendBuff.LoopInput(BitConverter.GetBytes(bytes.Length));
                            sendBuff.LoopInput(new byte[] { 0 });
                            sendBuff.LoopInput(bytes);
                        }
                        else
                        {
                            bytes = CompressCRC.compress(bytes);
                            //bytes = Zip.Compress(bytes);

                            sendBuff.LoopInput(BitConverter.GetBytes(bytes.Length));
                            sendBuff.LoopInput(new byte[] { 1 });
                            sendBuff.LoopInput(bytes);
                        }
                    }
                }
            }, handle);
            //发送线程
            ThreadUtil.PoolCall(() =>
            {
                while (socket.Connected)
                {
                    while (sendBuff.DataCount == 0)
                    {
                        Thread.Sleep(wait);
                    }
                    lock (sendBuff)
                    {
                        try
                        {
                            sendBuff.Send(stream);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }, handle);
            #endregion

            #region 消费者进程
            //接收线程
            ThreadUtil.PoolCall(() =>
            {
                while (true)
                {
                    while (receiveBuff.GetReserveCount() == 0)
                    {
                        Thread.Sleep(wait);
                    }
                    lock (receiveBuff)
                    {
                        try
                        {
                            var bl=receiveBuff.Receive(stream);
                            if (!bl)
                            {
                                Close();
                            }
                        }
                        catch (Exception )
                        {
                        }
                    }
                }
            }, handle);
            //消费线程
            ThreadUtil.PoolCall(() =>
            {
                while (true)
                {
                    //读一个长度
                    var len = BitConverter.ToInt32(receiveBuff.LoopOutput(4), 0);
                    
                    //Console.WriteLine("读:" + len);

                    //读是否压缩
                    var isCompress = receiveBuff.LoopOutput(1);

                    //读取数据内容
                    var bytes = receiveBuff.LoopOutput(len);
                    if(bytes.Length==0)
                    {
                        throw new Exception("错误");
                    }
                    if (isCompress[0] == 1)
                    {
                        //Console.WriteLine("解压前" + bytes.Length);
                        bytes = CompressCRC.unCompress(bytes);
                        //Console.WriteLine("解压后" + bytes.Length);
                        //var temp = Zip.Compress(bytes);
                    }

                    lock (receiveQueue)
                    {
                        receiveQueue.Enqueue(bytes);
                    }
                }
            }, handle);
            #endregion
            
        }

        /// <summary>
        /// 异步连接服务器
        /// </summary>
        public void Connect(string host, int port, System.Action<bool> OnComplete)
        {
            if(socket==null) socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ThreadUtil.PoolCall(() =>
            {
                try
                {
                    socket.Connect(host, port);

                }
                catch (Exception)
                {
                }

                if (socket.Connected)
                {
                    
                    StartLoop();
                    
                }
                if (OnComplete != null) OnComplete(socket.Connected);
            });
        }

        public void Send(byte[] data)
        {
            sendQueue.Enqueue(data);
        }

        public void Close()
        {
            DisConnect();
            if(socket!=null)
            {
                socket.Close();
                socket = null;
            }
        }
    }
}
