namespace System
{
    public static class ArrayUtil
    {
        /// <summary>
        /// 循环拷贝
        /// </summary>
        public static void RoundCopy<T>(T[] sourceArray, int sourceIndex, T[] destinationArray, int destinationIndex, int length)
        {
            if(sourceIndex>= sourceArray.Length || destinationIndex>=destinationArray.Length || length<=0)
            {
                throw new Exception("传入参数有误");
            }

            for (int i = 0; i < length; i++)
            {
                destinationArray[destinationIndex] = sourceArray[sourceIndex];
                sourceIndex++;
                destinationIndex++;
                if (sourceIndex == sourceArray.Length) sourceIndex = 0;
                if (destinationIndex == destinationArray.Length) destinationIndex = 0;
            }
        }
        /// <summary>
        /// 将一个数组拆成另外一个数组
        /// </summary>
        /// <param name="originbyte">原始数组，被拆分的数组</param>
        /// <param name="startIndex">从原始数组第几个元素开始</param>
        /// <param name="endIndex">从原始数组第几个元素结束</param>
        /// <returns></returns>
        public static byte[] SplitByteArray(byte[] originbyte, int startIndex, int endIndex)
        {
            byte[] result = new byte[endIndex- startIndex+1];
            System.Array.Copy(originbyte, startIndex, result, 0, result.Length);
            return result;
        }


    }
}
