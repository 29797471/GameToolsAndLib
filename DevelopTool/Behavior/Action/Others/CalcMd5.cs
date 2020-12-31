using CqCore;
using WinCore;

namespace CqBehavior.Task
{
    [Editor("计算md5")]
    [MenuItemPath("添加/其他/计算md5")]
    public class CalcMd5 : CqBehaviorNode
    {
        [MinWidth(100)]
        [TextBox("源")]
        [Priority(1)]
        public string Command { get { return mCommand; } set { mCommand = value; Update("Command"); } }
        public string mCommand;

        [Button, Click("Calc")]
        [Priority(2)]
        public string Btn { get { return "计算Md5"; } }
        public void Calc(object obj)
        {
            ResultStr = StringUtil.Md5Sum(Command);
            ClipboardUtil.CopyObject(mResultStr);
        }
        [Button, Click("Calc1")]
        [Priority(2,1)]
        public string Btn2 { get { return "自定义8位算法"; } }

        [Button, Click("Calc2")]
        [Priority(2, 2)]
        public string Btn3 { get { return "自定义12位算法"; } }

        [Button, Click("Calc3")]
        [Priority(2, 3)]
        public string Btn4 { get { return "自定义12位算法(新)"; } }

        [TextBox("结果"),MinWidth(100)]
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

        //生成8位数字大小写字母组合
        public void Calc1(object obj)
        {
            int times = 2;
            string _temmp= Command;
            for(int i=0;i<times;i++)
            {
                _temmp = StringUtil.Md5Sum(_temmp);
            }
            var temp = _temmp.ToCharArray();
            var x = new char[8];

            for (int i=0;i<8;i++)
            {
                for(int j=0;j<4;j++)
                {
                    x[i] += temp[i * 4 + j];
                }
                switch(i)
                {
                    case 0:
                        x[i] = ToUpperWord(x[i]);
                        break;
                    case 1:
                        x[i] = ToLowerWord(x[i]);
                        break;
                    case 7:
                        x[i] = ToNumber(x[i]);
                        break;
                    default:
                        x[i] = ToNumberWord(x[i]);
                        break;
                }
                
            }
            ResultStr = new string(x);
            ClipboardUtil.CopyObject(mResultStr);
        }
        //(种类+帐号)生成由数字大小写字母组成的12位字符串
        public void Calc2(object obj)
        {
            int digit = 12;
            var _temmp = "";
            var SeedStr = Command;
            for (int i = 0; i < digit; i++)
            {
                SeedStr += Command.Substring(i%Command.Length,1);
                CqRandom.Seed = SeedStr.GetHashCode();
                _temmp += ToNumberWord(RandomUtil.Random(0, 26+26+10));
            }
            ResultStr = _temmp;
            ClipboardUtil.CopyObject(mResultStr);
        }
        /// <summary>
        /// (种类+帐号)生成12包含数字大小写字母的组合(新)
        /// 
        /// </summary>
        public void Calc3(object obj)
        {
            int digit = 12;
            var _temmp = new System.Collections.Generic.List<char>();
            var SeedStr = Command;
            var list = new System.Collections.Generic.List<int>();
            for (int i = 0; i < digit; i++)
            {
                SeedStr += Command.Substring(i % Command.Length, 1);
                CqRandom.Seed = SeedStr.GetHashCode();
                _temmp.Add(ToNumberWord(RandomUtil.Random(0, 26 + 26 + 10)));
                list.Add(i);
            }
            RandomUtil.RandomShuffle(list);
            var index = list[0];
            _temmp[index] = (char)(48 + RandomUtil.Random(0,10));
            index = list[1];
            _temmp[index] = (char)(65 + RandomUtil.Random(0, 26));
            index = list[2];
            _temmp[index] = (char)(97 + RandomUtil.Random(0, 26));
            ResultStr = string.Join("",_temmp);
            ClipboardUtil.CopyObject(mResultStr);
        }
        /// <summary>
        /// 转数字大小写字母
        /// </summary>
        char ToNumberWord(int c)
        {
            var n = 26 + 26 + 10;
            var d = c % n;
            if (d < 10)
            {
                return (char)(48 + d);
            }
            else
            {
                d -= 10;
                if (d < 26)
                {
                    return (char)(65 + d);
                }
                else
                {
                    d -= 26;
                    return (char)(97 + d);
                }
            }
        }
        char ToNumber(char c)
        {
            var d = c % 10;
            return (char)(48 + d);
        }
        char ToUpperWord(char c)
        {
            var d = c % 26;
            return (char)(65 + d);
        }
        char ToLowerWord(char c)
        {
            var d = c % 26;
            return (char)(97 + d);
        }
    }
}

