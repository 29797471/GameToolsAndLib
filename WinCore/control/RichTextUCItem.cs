using System;
using System.Windows;
using System.Windows.Media;

namespace WinCore
{
    /// <summary>
    /// 文本,颜色,带点击事件
    /// </summary>
    public class RichTextUCItem:NotifyObject
    {
        public string Text
        {
            get { if (CallBack != null) return ""; return mText; }
        }
        public string mText;
        public Brush Color;
        public Action CallBack;
        public string UnderlineText
        {
            get
            {
                if (CallBack == null) return "";
                return mText;
            }
        }
        
    }
}
