using System;

namespace CqCore
{
    /// <summary>
    ///常用算法类
    /// </summary>
    public static partial class MathUtil
    {
        /// <summary>
        /// 由3个点推导一元二次函数
        /// </summary>
        public static FunctionExpr OneVariableQuadraticExpr(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            Func<double, Matrix> XX = x => new Matrix(new double[1, 3] { { x * x, x, 1 } });
            SquareMatrix A = (XX(x1).Transpose() | XX(x2).Transpose() | XX(x3).Transpose()).Transpose().ToSquareMatrix();
            Matrix B = new Matrix(new double[1, 3] { { y1, y2, y3 } }).Transpose();
            var a = A.Inverse() * B;

            return new FunctionExpr(string.Format("x^2*({0})+x*({1})+({2})",a[0,0],a[1,0],a[2,0]), x => (XX(x) * a)[0, 0]);
        }
    }
	public class FunctionExpr
    {
        Func<double, double> mFun;
        public Func<double, double> Fun
        {
            get { return mFun; }
        }
        string mExpr;
        public string Expr
        {
            get { return mExpr; }
        }

        public FunctionExpr(string expr, Func<double, double> fun=null)
        {
            mExpr = expr;
			if(fun==null)
            {
                var tFun = Arithmetic.Parse_Fx(expr);
                mFun = x=>(double)tFun(x);
            }
			else
            {
                mFun = fun;
            }
        }
    }
}