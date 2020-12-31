using CqCore;
using WinCore;

namespace CqBehavior.Task
{
    [Editor("个税计算器")]
    [MenuItemPath("添加/其他/个税计算器")]
    public class CalcTaxes : CqBehaviorNode
    {
        [MinWidth(100)]
        [TextBox("税前基本工资(月入)")]
        [Priority(1)]
        public float Command 
        {
            get { return mCommand; } 
            set 
            { 
                mCommand = value;
                Update("Command");
                ReCalc();
            }
        }
        public float mCommand;

        [MinWidth(100)]
        [TextBox("年终奖(月)")]
        [Priority(1,1)]
        public int MonthCount
        {
            get { return mMonthCount; }
            set
            {
                mMonthCount = value;
                Update("MonthCount");
                ReCalc();
            }
        }
        public int mMonthCount=0;

        void ReCalc()
        {
            var total = (mCommand - 5000 - Insurance - OtherCost) * 12+ MonthCount* Command;
            float x = Calc(total);
            ResultStr = "应纳税所得额:" + total;
            ResultStr += "\n累计应缴税款:" + x;
            ResultStr += "\n税后(年平均月入):" + (mCommand*(12+ MonthCount) - x  - Insurance*12)/12;
        }

        [MinWidth(100)]
        [TextBox("各项社会保险费")]
        [Priority(2,0)]
        public float Insurance
        {
            get { return mInsurance; }
            set
            {
                mInsurance = value;
                Update("Insurance");
                ReCalc();
            }
        }
        public float mInsurance;

        [MinWidth(100)]
        [TextBox("专项附加扣除")]
        [Priority(2, 1)]
        public float OtherCost
        {
            get { return mOtherCost; }
            set
            {
                mOtherCost = value;
                Update("OtherCost");
                ReCalc();
            }
        }
        public float mOtherCost;

        /// <summary>
        /// 累积应交税款
        /// </summary>
        /// <param name="x">年应纳税所得额</param>
        /// <returns></returns>
        public float Calc(float total)
        {
            int[] aa = new int[] { 36000, 144000, 300000, 420000, 660000, 960000 ,int.MaxValue};
            float[] bb = new float[] { 0.03f, 0.1f, 0.2f, 0.25f, 0.3f, 0.35f, 0.45f };
            
            if (total < 0) return 0;
            var calc = 0f;//应纳税款
            for (int i=0;i< aa.Length; i++)
            {
                var delta = 0f;
                if (i == 0) delta = aa[i];
                else delta = aa[i] - aa[i - 1];
                if (total< delta)
                {
                    calc+= total * bb[i];
                    return calc;
                }
                calc+= delta * bb[i];
                total -= delta;
            }
            return calc;
        }
        

        [TextBox("个税结果:"), MinWidth(100),IsEnabled(false)]
        [AcceptsReturn(true)]
        [Priority(3)]
        public string ResultStr
        {
            get { return mResultStr; }
            set
            {
                mResultStr = value;
                Update("ResultStr");
            }
        }
        public string mResultStr;

    }
}


