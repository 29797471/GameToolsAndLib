using CqCore;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WinCore;
/// <summary>
/// 显示html文本
/// </summary>
public class HtmlTextAttribute : ControlPropertyAttribute
{
    public HtmlTextAttribute(string name = null,float width=0f) : base(name,width)
    {
    }
    public override FrameworkElement CreateFrameworkElement()
    {
        var ctl = new WebBrowser();
        var binding = new Binding(Info.Name);
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        if (Info.CanRead && !Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWay;
        }
        else if (!Info.CanRead && Info.CanWrite)
        {
            binding.Mode = BindingMode.OneWayToSource;
        }

        CancelHandle handle=new CancelHandle();
        var isBottom = false;
        var top = 0;
        SetPropertyChanged(ctl, () =>
        {
            handle.CancelAll();
            {
                isBottom = ctl.IsScrollBottom();
                top = ctl.GetScrollTop();
            }
            ctl.NavigateToUTF8String((string)Target);
            GlobalCoroutine.DelayCall(0.01f, () =>
            {
                handle = null;
                if (isBottom)
                {
                    ctl.ScrollToBottom();
                }
                else
                {
                    ctl.ScrollTo(top);
                }
            }, handle);
        });
        return ctl;
    }

}

public static class WebBrowserUtil
{
    public static bool IsScrollBottom(this WebBrowser ctl)
    {
        var doc = (mshtml.IHTMLDocument2)ctl.Document;
        if (doc != null && doc.body != null)
        {
            var scrollHeight = (int)(object)doc.body.getAttribute("scrollHeight");
            var scrollTop = (int)(object)doc.body.getAttribute("scrollTop");

            return (scrollHeight - scrollTop) <= doc.body.offsetHeight;
        }
        return false;
    }

    public static void ScrollToBottom(this WebBrowser ctl)
    {
        var doc = (mshtml.IHTMLDocument2)ctl.Document;
        if (doc != null && doc.body != null)
        {
            doc.parentWindow.scrollTo(0, (int)(object)doc.body.getAttribute("scrollHeight"));
        }
    }
    public static void ScrollTo(this WebBrowser ctl,int offset)
    {
        var doc = (mshtml.IHTMLDocument2)ctl.Document;
        if (doc != null && doc.body != null)
        {
            doc.parentWindow.scrollTo(0, offset);
        }
    }
    public static int GetScrollTop(this WebBrowser ctl)
    {
        var doc = (mshtml.IHTMLDocument2)ctl.Document;
        if (doc != null && doc.body != null)
        {
            return (int)(object)doc.body.getAttribute("scrollTop");
        }
        return 0;
    }
}

