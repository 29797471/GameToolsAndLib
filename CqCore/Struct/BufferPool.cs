using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 缓冲池
    /// 在编写网络应用的时候数据缓冲区是应该比较常用的方式，
    /// 主要用构建一个内存区用于存储发送的数据和接收的数据；
    /// 为了更好的利用已有数据缓冲区所以构造一个缓冲池来存放相关数据方便不同连接更好地利用缓冲区，
    /// 节省不停的构造新的缓冲区所带的损耗问题。
    /// </summary>
    public class BufferPool : IDisposable
    {
        private static List<BufferPool> mPools = new List<BufferPool>();
        private static int mIndex = 0;
        public static void Setup(int pools, int buffers)
        {
            Setup(pools, buffers, 2048);
        }
        public static void Setup(int pools, int buffers, int bufferlength)
        {
            lock (mPools)
            {
                for (int i = 0; i < pools; i++)
                {
                    mPools.Add(new BufferPool(buffers, bufferlength));
                }
            }
        }
        public static void Clean()
        {
            lock (mPools)
            {
                foreach (BufferPool item in mPools)
                {
                    item.Dispose();
                }
                mPools.Clear();
            }
        }
        public static BufferPool GetPool()
        {
            lock (mPools)
            {
                if (mIndex == mPools.Count)
                {
                    mIndex = 0;
                }
                return mPools[mIndex];
            }
        }
        Queue<DataBuffer> mBuffers;
        private int mBufferLength;
        public BufferPool(int count, int bufferlength)
        {
            mBufferLength = bufferlength;
            mBuffers = new Queue<DataBuffer>(count);
            for (int i = 0; i < count; i++)
            {
                mBuffers.Enqueue(createBuffer(bufferlength));
            }
        }
        private DataBuffer createBuffer(int length)
        {
            DataBuffer item = new DataBuffer(length);
            item.Pool = this;
            return item;
        }
        public DataBuffer Pop()
        {
            lock (mBuffers)
            {
                return mBuffers.Count > 0 ? mBuffers.Dequeue() : createBuffer(mBufferLength);
            }
        }
        public void Push(DataBuffer buffer)
        {
            lock (mBuffers)
            {
                mBuffers.Enqueue(buffer);
            }
        }
        private bool mDisposed = false;
        private void OnDispose()
        {
            lock (mBuffers)
            {
                while (mBuffers.Count > 0)
                {
                    mBuffers.Dequeue().Pool = null;
                }
            }
        }
        public void Dispose()
        {
            lock (this)
            {
                if (!mDisposed)
                {
                    OnDispose();
                    mDisposed = true;
                }
            }
        }
    }
}
