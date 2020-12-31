using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;
using WinCore;

namespace CqBehavior.Task
{
    [MenuItemPath("添加/条件节点/颜色判断")]
    [Editor("颜色判断")]
    public class PixelCompare : CqBehaviorNode
    {

        [Width(200)]
        [UnderLine("颜色1"),Click]
        [Priority(1)]
        public ColorVariable ColorVal1
        {
            get { if (mColorVal1 == null) mColorVal1 = new ColorVariable(); /*mColorVal1.SetRoot(this);*/ return mColorVal1; }
            set { mColorVal1 = value; Update("ColorVal1"); }
        }
        public ColorVariable mColorVal1;

        [Width(200)]
        [UnderLine("颜色2"), Click]
        [Priority(2)]
        public ColorVariable ColorVal2
        {
            get { if (mColorVal2 == null) mColorVal2 = new ColorVariable(); /*mColorVal2.SetRoot(this);*/ return mColorVal2; }
            set { mColorVal2 = value; Update("ColorVal2"); }
        }
        public ColorVariable mColorVal2;

        [MinWidth(100)]
        [Margin(5, 0)]
        [TextBox("误差")]
        [Priority(3)]
        public int Error
        {
            get {  return mError; }
            set { mError = value; Update("Error"); }
        }
        public int mError;

        //[Visibility("CompareVal",  BindingAttribute.CustomConvert.BoolNotToVisibility,null,true)]
        //[ColorBox("采样的颜色")]
        //public Color ScreenColor
        //{
        //    get
        //    {
        //        return WinUtil.ToMediaColor(mScreenPointColor);
        //    }
        //    set
        //    {
        //        mScreenPointColor = value.ToString();
        //        Update("ScreenColor");
        //    }
        //}

        //public string mScreenPointColor = "#000000";

        

        protected override IEnumerator OnExecute()
        {
            yield return null;
            Result=ColorLookSame(ColorVal1.GetValue(), ColorVal2.GetValue());
        }
        public bool ColorLookSame(Color c1, Color c2, int error = 5)
        {
            return (Math.Abs(c1.A - c2.A) <= error) && (Math.Abs(c1.R - c2.R) <= error) && (Math.Abs(c1.G - c2.G) <= error) && (Math.Abs(c1.B - c2.B) <= error);
        }
    }
}



