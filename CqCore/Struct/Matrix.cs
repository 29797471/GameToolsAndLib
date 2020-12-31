using System;

namespace CqCore
{
    /// <summary>
    /// 矩阵
    /// </summary>
    public class Matrix
    {
        double[,] data;
        public double this[int row, int col]
        {
            get
            {
                return data[row, col];
            }
            set
            {
                data[row, col] = value;
            }
        }
        //public double[] this[int row]
        //{
        //    get
        //    {
        //        var temp = new double[Col];
        //        for (int j=0;j<Col;j++)
        //        {
        //            temp[j] = data[row, j];
        //        }
        //        return temp;
        //    }
        //}
        public int Row
        {
            get
            {
                return data.GetLength(0);
            }
        }
        public int Col
        {
            get
            {
                return data.GetLength(1);
            }
        }
        //用行数和列数初始化矩阵，row为行数，col为列数
        public Matrix(int row, int col)
        {
            data = new double[row, col];
        }
        /// <summary>
        /// 矩阵
        /// </summary>
        public Matrix(double[,] data)
        {
            this.data = data;
            data = Clone().data;
        }
        public override bool Equals(object obj)
        {
            var mat = obj as Matrix;
            if (mat != null)
            {
                return data.Equals(mat.data);
            }
            return false;
        }
        /// <summary>
        /// //判断矩阵相等
        /// </summary>
        public static bool operator ==(Matrix a, Matrix b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// //判断矩阵不等
        /// </summary>
        public static bool operator !=(Matrix a, Matrix b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// 矩阵相加
        /// </summary>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            return CalcMatrix(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// 矩阵相减
        /// </summary>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            return CalcMatrix(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// 矩阵与常数相乘
        /// </summary>
        public static Matrix operator *(Matrix a, double f)
        {
            int row = a.Row;
            int col = a.Col;
            var mat = new Matrix(row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mat[i, j] = a[i, j] * f;
                }
            }
            return mat;
        }
        public static Matrix operator *(double factor, Matrix matrix)
        {
            return matrix * factor;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Col != b.Row)
            {
                return null;
            }
            int n = a.Col;
            var mat = new Matrix(a.Row, b.Col);
            for (int i = 0; i < mat.Row; i++)
            {
                for (int j = 0; j < mat.Col; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        mat[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return mat;
        }

        /// <summary>
        /// 两矩阵形成的增广矩阵
        /// </summary>
        public static Matrix operator |(Matrix a, Matrix b)
        {
            if (a.Row != b.Row) return null;
            var mat = new Matrix(a.Row, a.Col + b.Col);
            for (int i = 0; i < mat.Row; i++)
            {
                int j = 0;
                for (; j < a.Col; j++)
                {
                    mat[i, j] = a[i, j];
                }
                for (int t = 0; j < mat.Col; j++, t++)
                {
                    mat[i, j] = b[i, t];
                }
            }
            return mat;
        }


        /// <summary>
        /// 转置
        /// </summary>
        public Matrix Transpose()
        {
            var mat = new Matrix(Col, Row);
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    mat[j, i] = this[i, j];
                }
            }
            return mat;
        }



        /// <summary>
        /// 伴随矩阵
        /// </summary>
        public Matrix Adjoint()
        {
            var mat = new Matrix(Col, Row);
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    mat[j, i] = this[i, j];
                }
            }
            return mat;
        }
        /// <summary>
        /// 计算矩阵的秩
        /// 计算矩阵的秩，即把矩阵进行行初等变换，得出的行最简矩阵的非零行数。过程如下
        ///1）将矩阵各行按第一个非零元素出现的位置升序排列（Operation1函数）
        ///2）查看矩阵是否为行最简矩阵（isFinished函数），是则到第6步，不是则到第3步
        ///3）如果有两行第一个非零元素出现的位置相同，则做消法变换，让下面行的第一个非零元素位置后移（Operation2函数）
        ///4）将矩阵各行按第一个非零元素出现的位置升序排列（Operation1函数）
        ///5）返回第2步
        ///6）判断误差，对趋近与0的元素（如1E-5）按0处理，以免在第7步误判（Operation3函数）
        ///7）统计非零行的数目（Operation4函数），即为矩阵的秩
        /// </summary>
        public int Rank()
        {
            //matrix为空则直接默认已经是最简形式
            if (data == null || Row == 0) return 0;

            //复制一个matrix到copy，之后因计算需要改动矩阵时并不改动matrix本身
            var mat = Clone();

            //先以最左侧非零项的位置进行行排序
            mat.Operation1();

            //循环化简矩阵
            while (!mat.isFinished())
            {
                mat.Operation2();
                mat.Operation1();
            }

            //过于趋近0的项，视作0，减小误差
            mat.Operation3();

            //行最简矩阵的秩即为所求
            return mat.Operation4();
        }
        
        
        /// <summary>
        /// a,b 做代数计算
        /// </summary>
        public static Matrix CalcMatrix(Matrix a, Matrix b, System.Func<double, double, double> fun)
        {
            int row = a.Row;
            int col = a.Col;
            var mat = new Matrix(row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mat[i, j] = fun(a[i, j], b[i, j]);
                }
            }
            return mat;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public Matrix Clone()
        {
            var mat = new Matrix(Row, Col);
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    mat[i, j] = this[i, j];
                }
            }
            return mat;
        }
        #region 初等变换
        /// <summary>
        /// 判断矩阵是否变换到最简形式（非零行数达到最少）
        /// </summary>
        private bool isFinished()
        {
            //统计每行第一个非零元素的出现位置
            int[] counter = new int[Row];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (this[i, j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }

            //后面行的非零元素出现位置必须在前面行的后面，全零行除外
            for (int i = 1; i < Row; i++)
            {
                if (counter[i] <= counter[i - 1] && counter[i] != Col)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 排序（按左侧最前非零位位置自上而下升序排列）
        /// </summary>
        private void Operation1()
        {
            //统计每行第一个非零元素的出现位置
            var counter = new int[Row];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (this[i, j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }

            //按每行非零元素的出现位置升序排列
            for (int i = 0; i < Row; i++)
            {
                for (int j = i; j < Row; j++)
                {
                    if (counter[i] > counter[j])
                    {
                        ExchangeRow(i, j);
                    }
                }
            }
        }
        /// <summary>
        /// 行初等变换（左侧最前非零位位置最靠前的行，只保留一个）
        /// </summary>
        private void Operation2()
        {
            //统计每行第一个非零元素的出现位置
            int[] counter = new int[Row];
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (this[i,j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }

            for (int i = 1; i < counter.Length; i++)
            {
                if (counter[i] == counter[i - 1] && counter[i] != Row)
                {
                    double a = this[i - 1,counter[i - 1]];
                    double b = this[i,counter[i]]; //counter[i]==counter[i-1]

                    this[i,counter[i]] = 0;
                    for (int j = counter[i] + 1; j < Col; j++)
                    {
                        double c = this[i - 1,j];
                        this[i,j] -= (c * b / a);
                    }

                    break;
                }
            }
        }
        /// <summary>
        /// 将和0非常接近的数字视为0
        /// </summary>
        private void Operation3()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (Math.Abs(this[i,j]) <= 0.0001f)
                    {
                        this[i,j] = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 计算行最简矩阵的秩
        /// </summary>
        private int Operation4()
        {
            int rank = -1;
            bool isAllZero = true;
            for (int i = 0; i < Row; i++)
            {
                isAllZero = true;

                //查看当前行有没有0
                for (int j = 0; j < Col; j++)
                {
                    if (this[i,j] != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                //若第i行全为0，则矩阵的秩为i
                if (isAllZero)
                {
                    rank = i;
                    break;
                }
            }
            //满秩矩阵的情况
            if (rank == -1)
            {
                rank = Row;
            }

            return rank;
        }
        //初等变换：交换两行 返回变换后的矩阵本身
        public Matrix ExchangeRow(int a, int b)//(i,j从1开始)
        {
            var mat = Clone();
            for (int j = 0; j < Col; j++)
            {
                Swap(a, j, b, j);
            }
            return mat;
        }
        //初等变换：交换两列 返回变换后的矩阵本身
        public Matrix ExchangeCol(int a, int b)//(i,j从1开始)
        {
            var mat = Clone();
            for (int i = 0; i < Row; i++)
            {
                Swap(i, a, i, b);
            }
            return mat;
        }
        /// <summary>
        /// 交换两个元素
        /// </summary>
        public void Swap(int ia, int ja, int ib, int jb)
        {
            var temp = this[ia, ja];
            this[ia, ja] = this[ib, jb];
            this[ib, jb] = temp;
        }
        #endregion
        public SquareMatrix ToSquareMatrix()
        {
            if (Row != Col) return null;
            return new SquareMatrix(data);
        }

    }
}
