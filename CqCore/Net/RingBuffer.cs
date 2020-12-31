using System;
using System.Threading;

/// <summary>
/// 环形队列
/// 生产者消费者模型
/// 一个高效率可复用的缓存区
/// 异步数据接收有可能收到的数据不是一个完整包，或者接收到的数据超过一个包的大小，
/// 因此我们需要把接收的数据进行缓存。
/// 异步发送我们也需要把每个发送的包加入到一个队列，然后通过队列逐个发送出去，
/// 如果每个都实时发送，有可能造成上一个数据包未发送完成，
/// 这时再调用SendAsync会抛出异常，提示SocketAsyncEventArgs正在进行异步操作，
/// 因此我们需要建立接收缓存和发送缓存。
/// </summary>
public class RingBuffer
{
     byte[] buffer { get; set; } // 存放内存的数组

    public int DataCount
    {
        get
        {
            if (DataStart == -1 || DataEnd==-1) return 0;
            if(DataStart > DataEnd)
            {
                return buffer.Length + DataEnd - DataStart+1;
            }
            else
            {
                return DataEnd - DataStart+1;
            }
        }
    }

    public int DataStart { get;private set; } // 数据起始索引
    public int DataEnd { get; private set; }  // 数据结束索引
    
    public RingBuffer(int bufferSize=1<< 12)
    {
        DataStart = -1; DataEnd = -1;
        buffer = new byte[bufferSize];
    }

    public byte this[int index]
    {
        get
        {
            if (index >= DataCount) throw new Exception("环形缓冲区异常，索引溢出");
            if (DataStart + index < buffer.Length)
            {
                return buffer[DataStart + index];
            }
            else
            {
                return buffer[(DataStart + index) - buffer.Length];
            }
        }
    }

    /// <summary>
    /// 获得剩余可复用字节数
    /// </summary>
    /// <returns></returns>
    public int GetReserveCount() 
    {
        return buffer.Length - DataCount;
    }

    public void Clear()
    {
        DataEnd = DataStart;
    }

    /// <summary>
    /// 将缓冲数据通过网络数据流发送出去
    /// </summary>
    public void Send(System.IO.Stream stream)
    {
        if (DataStart > DataEnd)
        {
            stream.Write(buffer, DataStart, buffer.Length - DataStart);
            stream.Write(buffer, 0, DataEnd + 1);
        }
        else
        {
            stream.Write(buffer, DataStart, DataCount);
        }
        DataStart = -1;
        DataEnd = -1;
    }
    /// <summary>
    /// 从网络数据流接收数据到缓冲
    /// 如果接收到0,证明已经断开
    /// </summary>
    public bool Receive(System.IO.Stream stream)
    {
        
        if (GetReserveCount()==0)//缓存已满
        {
            return true;
        }
        else if(DataStart==-1)
        {
            var len=stream.Read(buffer, 0, buffer.Length);
            if (len == 0) return false;
            DataStart = 0;
            DataEnd = len - 1;
        }
        else if (DataStart > DataEnd)//|-------end-------start-------|
        {
            var len=stream.Read(buffer, DataEnd+1, DataStart- DataEnd-1);
            if (len == 0) return false;
            DataEnd += len;
        }
        else//|-----start---------end-------|
        {
            if(DataEnd==buffer.Length-1)
            {
                var len=stream.Read(buffer, 0, DataStart-1);
                if(len==0) return false;

                DataEnd = len - 1;

            }
            else
            {
                var len=stream.Read(buffer, DataEnd+1, buffer.Length - DataEnd);
                if (len == 0) return false;

                DataEnd += len;
            }
        }
        return true;
    }

    /// <summary>
    /// 由线程调用
    /// 循环将数据写入缓冲
    /// </summary>
    /// <param name="data"></param>
    public void LoopInput(byte[] data)
    {
        var temp = DataCount;
        int copyCount = 0;
        //发送数据
        while (copyCount < data.Length)
        {
            int leftCount = 0;
            ab:
            lock (this)
            {
                leftCount = GetReserveCount();
            }
            if (leftCount == 0)
            {
                Thread.Sleep(100);
                goto ab;
            }
            lock (this)
            {
                if (data.Length - copyCount <= leftCount)
                {
                    InputData(data, copyCount, data.Length - copyCount);
                    copyCount = data.Length;
                }
                else
                {
                    InputData(data, copyCount, leftCount);
                    copyCount += leftCount;
                }
            }
        }
        //Console.WriteLine(string.Format("填充:{0}+{1}={2}", data.Length , temp, DataCount));
    }

    /// <summary>
    /// 由线程调用
    /// 循环读取缓冲中的数据
    /// </summary>
    public byte[] LoopOutput(int len)
    {
        var temp = DataCount;
        //读取数据内容
        var data = new byte[len];
        var copyCount = 0;
        //接收数据
        while (copyCount < data.Length)
        {
            int leftCount = 0;
            ab:
            lock (this)
            {
                leftCount = DataCount;
            }
            if (leftCount == 0)
            {
                Thread.Sleep(100);
                goto ab;
            }
            lock (this)
            {
                if (data.Length - copyCount <= leftCount)
                {
                    OutputData(data, copyCount, data.Length - copyCount);
                    copyCount = data.Length;
                }
                else
                {
                    OutputData(data, copyCount, leftCount);
                    copyCount += leftCount;
                }
            }
        }
        //Console.WriteLine(string.Format("读取:{0}-{1}={2}", DataCount, len, DataCount));
        return data;
    }
    /// <summary>
    /// 将数据写入缓冲
    /// 写入空间不够返回false
    /// </summary>
    public bool InputData(byte[] _buffer, int offset, int count)
    {
        if(offset+count>_buffer.Length)
        {
            throw new Exception("读取数据缓冲参数有误");
        }
        var reserveCount = GetReserveCount();
        // 可用空间够使用
        if (reserveCount < count)
        {
            return false;
        }

        //起始数据索引大于结束索引时
        //       |---可读-----|
        //|-----end---------start-------|
        if(DataStart==-1)
        {
            DataStart = 0;
            Array.Copy(_buffer, offset, buffer, 0, count);
        }
        else if (DataStart>DataEnd)     
        {
            Array.Copy(_buffer, offset, buffer, DataEnd+1, count);
        }
        else
        {
            //|-----start---------end-------|
            if (DataEnd+count<buffer.Length)
            {
                Array.Copy(_buffer, offset, buffer, DataEnd+1, count);
            }
            else
            {
                var copyCount = buffer.Length - DataEnd;
                Array.Copy(_buffer, offset, buffer, DataEnd+1, copyCount);
                Array.Copy(_buffer, offset+ copyCount, buffer, 0, count- copyCount);
            }
        }
        DataEnd += count;
        if (DataEnd > buffer.Length)
        {
            DataEnd -= buffer.Length;
        }
        return true;
    }
    
    

    /// <summary>
    /// 将缓冲数据导出
    /// </summary>
    public bool OutputData(byte[] _buffer, int offset, int count)
    {
        if (offset + count > _buffer.Length)
        {
            throw new Exception("写入缓冲数据参数有误");
        }
        if (count > DataCount)
        {
            return false;
        }

        //起始数据索引大于结束索引时
        //       |---可读-----|
        //|-----end---------start-------|
        if (DataStart > DataEnd)
        {
            if (DataStart + count < buffer.Length)
            {
                Array.Copy(buffer, DataStart, _buffer, offset, count);
            }
            else
            {
                var copyCount = buffer.Length - DataStart;
                Array.Copy(buffer, DataStart, _buffer, offset, copyCount);
                Array.Copy(buffer, 0, _buffer, offset+ copyCount, count - copyCount);
            }
        }
        else//|-----start---------end-------|
        {
            Array.Copy(buffer, DataStart, _buffer, offset, count);
        }
        DataStart += count;
        
        if (DataStart > buffer.Length)
        {
            DataStart -= buffer.Length;
        }
        if (DataCount==buffer.Length)
        {
            DataStart = -1;
            DataEnd = -1;
        }
        return true;
    }
}