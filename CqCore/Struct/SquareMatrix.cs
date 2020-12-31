using System;

namespace CqCore
{
    /// <summary>
    /// 方阵
    /// </summary>
    public class SquareMatrix:Matrix
    {
        public int Size
        {
            get
            {
                return Row;
            }
        }
        /// <summary>
        /// 方阵
        /// </summary>
        public SquareMatrix(int size):base(size,size)
        {

        }
        /// <summary>
        /// 方阵
        /// </summary>
        public SquareMatrix(double[,] data) : base(data)
        {
        }
        /// <summary>
        /// 递归计算行列式的值
        /// </summary>
        public double Determinant()
        {
            //二阶及以下行列式直接计算
            if (Size == 0) return 0;
            else if (Size == 1) return this[0,0];
            else if (Size == 2)
            {
                return this[0,0] * this[1,1] - this[0,1] * this[1,0];
            }

            //对第一行使用“加边法”递归计算行列式的值
            double dSum = 0, dSign = 1;
            for (int i = 0; i < Size; i++)
            {
                var sm = new SquareMatrix(Size - 1);

                for (int j = 0; j < sm.Size; j++)
                {
                    for (int k = 0; k < sm.Size; k++)
                    {
                        sm[j,k] = this[j + 1,(k >= i ? k + 1 : k)];
                    }
                }

                dSum += (this[0,i] * dSign * sm.Determinant());
                dSign = dSign * -1;
            }

            return dSum;
        }
        
        /// <summary>
        /// 单位方阵
        /// </summary>
        public static SquareMatrix Unit(int size)
        {
            var mat = new SquareMatrix(size);
            for (int i = 0; i < size; i++)
            {
                mat[i, i] = 1;
            }
            return mat;
        }

        /// <summary>
        /// 求矩阵的逆矩阵
        /// </summary>
        public SquareMatrix Inverse()
        {
            //计算矩阵行列式的值
            double dDeterminant = Determinant();
            if (Math.Abs(dDeterminant) <= 1E-6)
            {
                throw new Exception("矩阵不可逆");
            }

            //制作一个伴随矩阵大小的矩阵
            var result = AdjointMatrix();

            //矩阵的每项除以矩阵行列式的值，即为所求
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result[i,j] = result[i,j] / dDeterminant;
                }
            }

            return result;
        }

        /// <summary>
        /// 计算方阵的伴随矩阵
        /// </summary>
        public SquareMatrix AdjointMatrix()
        {
            //制作一个伴随矩阵大小的矩阵
            var result = new SquareMatrix(Size);
            //生成伴随矩阵
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    //存储代数余子式的矩阵（行、列数都比原矩阵少1）
                    var temp = new SquareMatrix(Size-1);

                    //生成代数余子式
                    for (int x = 0; x < temp.Size; x++)
                    {
                        for (int y = 0; y < temp.Size; y++)
                        {
                            temp[x,y] = this[ (x < i ? x : x + 1),(y < j ? y : y + 1)];
                        }
                    }

                    result[j,i] = ((i + j) % 2 == 0 ? 1 : -1) * temp.Determinant();
                }
            }

            return result;
        }
    }
}
