using Business;
using CqCore;
using DevelopTool;
using System.Collections.Generic;

namespace CqBehavior.Task
{
    [Editor("小游戏")]
    [MenuItemPath("添加/其他/小游戏")]
    public class Games : CqBehaviorNode
    {
        
        private GuessNumber g;

        [Button("猜数字"),Width(100),Click]
        [Priority(1)]
        public GuessNumber G
        {
            get
            {
                if (g == null) g = new GuessNumber();
                return g;
            }
            set
            {
                g = value;
            }
        }

        [Button,Click("OnDrawDlg")]
        [Priority(2)]
        public string Btn { get { return "五子棋"; } }
        public void OnDrawDlg(object obj)
        {
            var dlg = new ChessWindow();
            dlg.ShowDialog();
        }
    }
}
[Editor("猜数字"), Width(400),Height(500)]
public class GuessNumber :NotifyObject
{
    public override string ToString()
    {
        return "猜数字";
    }
    [Label(),MinWidth(250)]
    [Priority(1)]
    public string Result { get { return m_Result; } set { m_Result = value; Update("Result"); } }
    public string m_Result;

    [MinWidth(150)]
    [TextBox("输入:")]
    [Priority(2)]
    public string Num { get { return mNum; } set { mNum = value; Update("Num"); } }
    public string mNum;

    [Priority(3)]
    [Button ,Click("Guess")]
    public string Btn1 { get { return "猜"; } }

    public void Guess(object obj)
    {
        if (x == null) Init();
        int a = 0, b = 0;
        for (int i = 0; i < 4; i++)
        {
            if (x[i] == Num[i])
            {
                a += 1;
            }
            else if (Num.Contains(x[i].ToString()))
            {
                b += 1;
            }
        }

        Result += "第" + index + "次\t" + Num + " " + a + "A" + b + "B\n";
        index++;
    }
    string x;
    int index;

    public void Init()
    {
        var list = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(i);
        }
        RandomUtil.RandomShuffle(list);
        x = list[0].ToString() + list[1] + list[2] + list[3];
        index = 1;

    }
}

