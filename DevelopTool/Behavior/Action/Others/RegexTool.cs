using CqCore;
using System;
using System.Windows;
using WinCore;

namespace CqBehavior.Task
{
    [Editor("正则表达式测试工具")]
    [MenuItemPath("添加/行为节点/其它/正则表达式测试工具")]
    public class RegexTool: CqBehaviorNode
    {
        [Priority(2)]
        [TextBox("测试表达式"), MinWidth(100), TextWrapping(TextWrapping.Wrap), AcceptsReturn(true)]
        public string Pattern
        {
            get { return mPattern; }
            set { mPattern = value; Update("Pattern"); }
        }
        public string mPattern;

        [Priority(7)]
        [TextBox("测试内容"), MinWidth(100), TextWrapping(TextWrapping.Wrap), AcceptsReturn(true)]
        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; Update("Content"); }
        }
        public string m_Content;

        [Priority(8)]
        [Button,Click("Test")]
        public string Btn
        {
            get
            {
                return "测试";
            }
        }

        public void Test(object obj)
        {
            try
            {
                var bl = RegexUtil.IsMatch(Content, Pattern);
                if (bl)
                {
                    var group = RegexUtil.Matches(Content, Pattern);
                    string content = "";
                    for (int j=0;j< group.Count;j++)
                    {
                        var g = group[j].Groups;
                        content += "匹配组"+j+":" + "\n";
                        for (int i = 0; i < g.Count; i++)
                        {
                            var it = g[i].Value;
                            content += "\t"+i + ":" + it + "\n";
                        }
                    }
                    CustomMessageBox.Show(content);
                }
                else
                {
                    CustomMessageBox.Show("不匹配");
                }
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.Message);
            }
            
            
        }
    }
}
