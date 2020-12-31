using System;

namespace CqCore
{
    /// <summary>
    /// 缓冲区
    /// </summary>
    public class DataBuffer : IDisposable
    {
        public byte[] Data;
        private int mLength;
        private int mPostion = 0;
        internal int mCount = 0;

        public DataBuffer(byte[] data)
        {
            Data = data;
            mLength = data.Length;
            mPostion = 0;
            mCount = data.Length;
        }

        public DataBuffer(int length)
        {
            mLength = length;
            Data = new byte[length];
        }
        public void From(Array source, int index, int count)
        {
            Array.Copy(source, index, Data, 0, count);
            mPostion = 0;
            mCount = count;
        }
        public int Write(byte[] data)
        {
            return Write(data, 0);
        }
        public int Write(byte[] data, int index)
        {
            int count = 0;
            if (mPostion + (data.Length - index) > mLength)
            {
                count = mLength - mPostion;
            }
            else
            {
                count = data.Length - index;
            }
            if (count > 0)
            {
                Array.Copy(data, index, Data, mPostion, count);

                mPostion += count;
                mCount += count;
            }
            return count;
        }
        public ArraySegment<byte> Read(int count)
        {
            int end = count;
            if (mPostion + count > mCount)
                end = mCount - mPostion;

            ArraySegment<byte> result = new ArraySegment<byte>(Data, mPostion, end);
            mPostion += end;
            return result;
        }
        public void Seek(int position=0)
        {
            mPostion = position;
        }
        public ArraySegment<byte> GetSegment()
        {
            return new ArraySegment<byte>(Data, 0, mCount);
        }
        internal BufferPool Pool
        {
            get;
            set;
        }
        public void Dispose()
        {
            if (Pool != null)
            {
                mPostion = 0;
                mCount = 0;
                Pool.Push(this);
            }
        }
    }
}
