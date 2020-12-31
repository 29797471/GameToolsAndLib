using CqBehavior.Task;
using CqCore;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevelopTool
{
    /// <summary>
    /// Fx.xaml 的交互逻辑
    /// </summary>
    public partial class CoordinateSystemWindow : Window
    {
        //public int XYRange
        //{
        //    get { return (int)GetValue(XYRangeProperty); }
        //    set
        //    {
        //        var a = GetBindingExpression(XYRangeProperty);
        //        if (a == null)
        //        {
        //            SetValue(XYRangeProperty, value);
        //        }
        //        else
        //        {
        //            AssemblyUtil.SetPropertyValue(a.DataItem, a.ParentBinding.Path.Path, value);
        //        }
        //    }
        //}

        //// Using a DependencyProperty as the backing store for XYRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty XYRangeProperty =
        //    DependencyProperty.Register("XYRange", typeof(int), typeof(CoordinateSystemWindow), new PropertyMetadata(0));



        //public int SmallScale
        //{
        //    get { return (int)GetValue(SmallScaleProperty); }
        //    set
        //    {
        //        var a = GetBindingExpression(SmallScaleProperty);
        //        if (a == null)
        //        {
        //            SetValue(SmallScaleProperty, value);
        //        }
        //        else
        //        {
        //            AssemblyUtil.SetPropertyValue(a.DataItem, a.ParentBinding.Path.Path, value);
        //        }
        //    }
        //}

        //// Using a DependencyProperty as the backing store for SmallScale.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SmallScaleProperty =
        //    DependencyProperty.Register("SmallScale", typeof(int), typeof(CoordinateSystemWindow), new PropertyMetadata(0));



        public CoordinateSystemWindow()
        {
            InitializeComponent();
            Loaded += (obj, e) => { ReDraw(null, null); };
        }

        /// <summary>
        /// 坐标轴上的大刻度长度
        /// </summary>
        const int bigMarkLen = 8;

        /// <summary>
        /// 坐标轴上的小刻度长度
        /// </summary>
        const int smallMarkLen = 5;
        CustomList<Equation> Equations;
        float XYAxisSacle;
        int smallMarkCount;
        int bigMarkCount;
        public void Init(CustomList<Equation> Equations, float XYAxisSacle,int smallMarkCount,int bigMarkCount)
        {
            this.Equations = Equations;
            this.XYAxisSacle = XYAxisSacle;
            this.smallMarkCount = smallMarkCount;
            this.bigMarkCount = bigMarkCount;
        }
        /// <summary>
        /// 绘制连续的线段
        /// </summary>
        private void BuildRegularPolygon(StreamGeometryContext ctx, Point[] values, bool isfilled, bool isClosed)
        {
            bool flag = true;//当产生奇点的时候断开前后连线,比如绘制y=1/x x=0时就是奇点
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Y <= mainPanel.Height && values[i].Y >= 0 &&
                    values[i].X <= mainPanel.Width && values[i].X >= 0)
                {
                    if (flag)
                    {
                        ctx.BeginFigure(values[i], isfilled, isClosed);
                        flag = false;
                    }
                    else ctx.LineTo(values[i], true, false);
                }
                else
                {
                    flag = true;
                }
            }
        }

        private void DrawFun(StreamGeometryContext ctx,Func<double, double> Fx, Func<double, double> Fy,int CalcCount)
        {
            Point[] points = new Point[CalcCount];

            Func<double, double, double, double> Ft = (x,nn,mm)=>(x-nn)/(mm-nn);

            double xAxisMin = -XYAxisSacle;
            double xAxisMax = XYAxisSacle;
           

            ///x,y坐标系转换
            Func <double, double, Point> ChangePoint = (x, y) =>
            {
                var tX = Ft(x, xAxisMin, xAxisMax);
                var tY = Ft(y, xAxisMin , xAxisMax);
                
                var outX= (double)MathUtil.LineLerp(0, mainPanel.Width, tX);
                var outY = (double)MathUtil.LineLerp(mainPanel.Height, 0,tY);
                return new Point(outX, outY);
            };

            for (double i = 0,t=0; i <CalcCount; i++,t+=1d/ (CalcCount-1))
            {
                points[(int)i] = ChangePoint(Fx(t),Fy(t));
            }

            BuildRegularPolygon(ctx, points, false, false);
        }

        private void ReDraw(object sender, RoutedEventArgs e)
        {
            if (Equations == null) return;
            DrawXY();
            using (StreamGeometryContext ctx = sg.Open())
            {
                foreach (var it in Equations)
                {
                    if (!it.Draw) continue;
                    switch (it.Index)
                    {
                        case 0:
                            {
                                var min = Convert.ToDouble(it.Xmin);
                                var max = Convert.ToDouble(it.Xmax);
                                var f = Arithmetic.Parse_Fx(it.Expr);
                                DrawFun(ctx, t => (double)MathUtil.LineLerp(min, max, t), 
                                    t => (double)f((double)MathUtil.LineLerp(min, max, t)),it.Sampling);
                            }
                            break;
                        case 1:
                            {
                                var min = Convert.ToDouble(it.Xmin) * Math.PI;
                                var max = Convert.ToDouble(it.Xmax) * Math.PI;
                                var fθ = Arithmetic.Parse_Fθ(it.Expr);
                                Func<double, double> ft = t => (double)MathUtil.LineLerp(min, max, t);
                                DrawFun(ctx, t => (double)fθ(ft(t)) * Math.Cos(ft(t)), t => (double)fθ(ft(t)) * Math.Sin(ft(t)), it.Sampling);
                            }
                            break;
                    }
                }
            }  
        }


        /// <summary>
        /// 绘制坐标轴标尺
        /// </summary>
        private void DrawXY()
        {

            Func<double, double, double, double> Ft = (x, nn, mm) => (x - nn) / (mm - nn);

            double xAxisMin = -XYAxisSacle;
            double xAxisMax = XYAxisSacle;
            
            ///x,y坐标系转换
            Func<double, double, Point> ChangePoint = (x, y) =>
            {
                var tX = Ft(x, xAxisMin, xAxisMax);
                var tY = Ft(y, xAxisMin, xAxisMax);

                var outX = (double)MathUtil.LineLerp(0, mainPanel.Width, tX);
                var outY = (double)MathUtil.LineLerp(mainPanel.Height, 0, tY);
                return new Point(outX, outY);
            };
            //画X轴刻度线
            using (StreamGeometryContext ctx = xGe.Open())
            {
                for (double x = -XYAxisSacle, count = 0; x <= XYAxisSacle; x += XYAxisSacle / bigMarkCount / smallMarkCount, count++)
                {
                    if (x == 0) continue;
                    var p = ChangePoint(x, 0);
                    ctx.BeginFigure(p, false, false);
                    p.Y += (count % smallMarkCount == 0) ? -bigMarkLen : -smallMarkLen;
                    ctx.LineTo(p, true, false);
                }
            }
            //画Y轴刻度线
            using (StreamGeometryContext ctx = yGe.Open())
            {
                for (double y = -XYAxisSacle, count = 0; y <= XYAxisSacle; y += XYAxisSacle / bigMarkCount / smallMarkCount, count++)
                {
                    if (y == 0) continue;
                    var p = ChangePoint(0, y);
                    ctx.BeginFigure(p, false, false);
                    p.X+= (count % smallMarkCount == 0) ? bigMarkLen : smallMarkLen;
                    ctx.LineTo(p, true, false);
                }
            }
            //画刻度文本
            xLeft.Content = (-XYAxisSacle).ToString();
            var pp = ChangePoint(-XYAxisSacle, 0);
            xLeft.Margin = new Thickness(pp.X,pp.Y, 0, 0);

            xRight.Content= XYAxisSacle.ToString();
            pp = ChangePoint(XYAxisSacle, 0);
            xRight.Margin = new Thickness(pp.X, pp.Y, 0, 0);

            yTop.Content = XYAxisSacle.ToString();
            pp = ChangePoint( 0, XYAxisSacle);
            yTop.Margin = new Thickness(pp.X, pp.Y, 0, 0);

            yBottom.Content = (-XYAxisSacle).ToString();
            pp = ChangePoint(0, -XYAxisSacle);
            yBottom.Margin = new Thickness(pp.X, pp.Y, 0, 0);
        }

    }
}
