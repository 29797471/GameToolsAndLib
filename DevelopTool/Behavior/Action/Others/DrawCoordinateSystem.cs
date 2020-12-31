using Business;
using CqCore;
using DevelopTool;
using System.Collections.Generic;

namespace CqBehavior.Task
{
    [Editor("绘制2D函数图像")]
    [MenuItemPath("添加/其他/绘制2D函数图像")]
    public class DrawCoordinateSystem : CqBehaviorNode
    {
        //[Priority(1)]
        ////DrawCartesianOrPolar
        //[TextBox("计算点数数量", 150), MinWidth(100)]
        //public int CalcCount
        //{
        //    get { return mCalcCount; }
        //    set { mCalcCount = value; Update("CalcCount"); }
        //}
        //public int mCalcCount;

        //[Priority(2,1)]
        //[Slider("横纵坐标范围",150),Minimum(1),Maximum(10),Width(100)]
        //public int XYRange
        //{
        //    get { return mXYRange; }
        //    set { mXYRange = value; Update("XYRange"); Update("ShowXYRange"); }
        //}
        //public int mXYRange;

        //[Priority(2,2)]
        //[TextBox(),IsEnabled(false)]
        //public int ShowXYRange { get { return XYRange; } }

        //[Priority(3, 1)]
        //[Slider("小刻度",150), Minimum(1), Maximum(10), Width(100)]
        //public int SmallScale
        //{
        //    get { return mSmallScale; }
        //    set { mSmallScale = value; Update("SmallScale"); Update("ShowSmallScale"); }
        //}
        //public int mSmallScale;

        //[Priority(3, 2)]
        //[TextBox(), IsEnabled(false)]
        //public int ShowSmallScale { get { return SmallScale; } }

        [Priority(4)]
        [ListBox("函数列表"), MinWidth(550), Height(200), DoubleSelectedValue(DoubleClickStyle.OpenEditorWindow)]
        public CustomList<Equation> Equations
        {
            get { if (mEquations == null) mEquations = new CustomList<Equation>(); return mEquations; }
            set { mEquations = value; Update("Equations"); }
        }
        public CustomList<Equation> mEquations;

        /// <summary>
        /// 绘制在屏幕上的点数
        /// </summary>
        [Priority(5)]
        [TextBox("坐标轴长度"), MinWidth(150)]
        public float XYAxisSacle
        {
            get { return mXYAxisSacle; }
            set { mXYAxisSacle = value; Update("XYAxisSacle"); }
        }
        public float mXYAxisSacle = 1;

        public int mSmallMarkCount=10;

        [Priority(5,1)]
        [TextBox("小刻度数量"), MinWidth(150)]
        public int SmallMarkCount
        {
            get { return mSmallMarkCount; }
            set { mSmallMarkCount = value; Update("SmallMarkCount"); }
        }

        public int mBigMarkCount=5;

        [Priority(5, 2)]
        [TextBox("大刻度数量"), MinWidth(150)]
        public int BigMarkCount
        {
            get { return mBigMarkCount; }
            set { mBigMarkCount = value; Update("BigMarkCount"); }
        }
        [Button, Click("OnDraw")]
        [Priority(6)]
        public string Btn { get { return "绘图"; } }
        public void OnDraw(object obj)
        {
            var dlg = new CoordinateSystemWindow();
            dlg.Init(Equations, XYAxisSacle, mSmallMarkCount, mBigMarkCount);
            dlg.ShowDialog();
        }
    }
    [Editor("方程"), Width(500), Height(400)]
    public class Equation:NotifyObject
    {
        public override string ToString()
        {
            switch(Index)
            {
                case 0:
                    return "y=" + Expr + "\t\tx∈[" + Xmin + "," + Xmax + "]"+ (Draw? "\t\t√" : "");
                case 1:
                    return "ρ=" + Expr + "\t\tθ∈[" + Xmin + "," + Xmax + "]*π" + (Draw ? "\t\t√" : "");
            }
            return null;
        }

        [Priority(1)]
        [RadioButtonGroup("Index", 60), Margin(30, 0)]
        public string[] Group
        {
            get
            {
                return new string[] { "直角坐标方程", "极坐标方程" };
            }
        }
        public int Index
        {
            get { return mIndex; }
            set { mIndex = value; Update("Index"); Update("YStart"); Update("XStart");Update("XEnd");Expr=Expr.Replace(Index==0? "θ":"x", Index == 0 ? "x" : "θ"); }
        }
        public int mIndex;

        [Priority(2, 0)]
        [Label()]
        public string YStart { get { return Index == 0 ? "y=" : "ρ="; } }

        [Priority(2,1)]
        [TextBox(),MinWidth(150)]
        public string Expr
        {
            get { if (mExpr == null) mExpr = "x"; return mExpr; }
            set { mExpr = value; Update("Expr"); }
        }
        public string mExpr;

        [Priority(3, 0)]
        [Label()]
        public string XStart { get { return Index == 0 ? "x∈[" : "θ∈["; } }

        [Priority(3,1)]
        [TextBox(), MinWidth(50),HorizontalAlignment( System.Windows.HorizontalAlignment.Center)]
        public double Xmin
        {
            get { return mXmin; }
            set { mXmin = value; Update("Xmin"); }
        }
        public double mXmin=-4;

        [Priority(3, 2)]
        [Label()]
        public string XMiddle { get { return ","; } }

        [Priority(3, 3)]
        [TextBox(), MinWidth(50), HorizontalAlignment(System.Windows.HorizontalAlignment.Center)]
        public double Xmax
        {
            get { return mXmax; }
            set { mXmax = value; Update("Xmax"); }
        }
        public double mXmax=4;

        [Priority(3,4)]
        [Label()]
        public string XEnd { get { return Index==0?"]":"]*π"; } }


        /// <summary>
        /// 绘制在屏幕上的点数
        /// </summary>
        [Priority(4)]
        [TextBox("采样数量")]
        public int Sampling
        {
            get { return mSampling; }
            set { mSampling = value;Update("Sampling"); }
        }
        public int mSampling=1000;

        

        [Priority(6)]
        [CheckBox("绘制")]
        public bool Draw
        {
            get { return mDraw; }
            set { mDraw = value; Update("Draw"); }
        }
        public bool mDraw=true;


    }

}




