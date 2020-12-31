namespace CqCore
{
    /// <summary>
    /// 数轴
    /// </summary>
    public class NumberAxis
    {
        double[] data;
        public NumberAxis(double[] data)
        {
            this.data = data;
        }
        /// <summary>
        /// 逆序数
        /// </summary>
        public int InverseNumber()
        {
            int count = 0;
            for(int i=0;i<data.Length-1;i++)
            {
                for (int j = i + 1; j < data.Length; j++)
                {
                    if (data[i] > data[j]) count++;
                }
            }
            return count;
        }
    }
}
