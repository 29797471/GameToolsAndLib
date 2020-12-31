using System;
using System.Windows;

namespace WinCore
{
    public static class ClipboardUtil
    {
        public static void CopyObject(object obj)
        {
            if (obj != null)
            {
                
                if(obj.GetType()==typeof(string))
                {
                    try
                    {
                        //Clipboard.SetText(obj.ToString());
                        ///使用Clipboard.SetText 有时会报错
                        Clipboard.SetDataObject(obj, true);
                        EventMgr.MsgPrint.Notify("复制:" + obj.ToString(), 5);
                    }
                    catch (Exception)
                    {
                        EventMgr.MsgPrint.Notify("复制失败", 5);
                    }
                }
                else
                {
                    try
                    {
                        ///使用Clipboard.SetText 有时会报错
                        Clipboard.SetDataObject(Torsion.Serialize(obj), true);
                        EventMgr.MsgPrint.Notify("复制:" + obj.ToString(), 5);
                    }
                    catch (Exception)
                    {
                        EventMgr.MsgPrint.Notify("复制失败", 5);
                    }
                    
                }
                
            }
        }
        public static T PasteObject<T>()
        {
            return (T)PasteObject(typeof(T));
        }
        public static object PasteObject(Type type)
        {
            var clone = Torsion.TryDeserialize(Clipboard.GetText(),type);
            if (clone != null) EventMgr.MsgPrint.Notify("粘贴:" + clone.ToString(), 5);
            return clone;
        }
    }
}
