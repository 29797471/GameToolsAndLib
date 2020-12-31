
using WinCore;
using System.Windows.Input;
using System.Collections.ObjectModel;
/// <summary>
/// 附带右键添加删除菜单的列表结构
/// </summary>
[Editor()]
public class CustomList<T> : ObservableCollection<T> where T : class,new()
{
    /// <summary>
    /// 在obj位置插入新元素
    /// </summary>
    /// <param name="obj"></param>
    [Priority(1)]
    [MenuItem("添加(+)", "/WinCore;component/Res/add.ico", System.Windows.Input.Key.Add)]
    public void AddItem(object obj)
    {
        var index = IndexOf(obj as T);
        if(index==-1) Add(new T());
        else Insert(index, new T());
        EventMgr.MsgPrint.Notify("添加:"+typeof(T).ToString(), 5);
    }

    [Priority(2)]
    [MenuItem("删除(Del)", "/WinCore;component/Res/delete.ico", Key.Delete)]
    public void DelItem(object obj)
    {
        if (obj != null)
        {
            Remove((T)obj);
            EventMgr.MsgPrint.Notify("删除:"+obj.ToString(), 5);
        }
    }

    [Priority(3)]
    [MenuItem("复制(Ctrl+C)", "/WinCore;component/Res/copy.ico", Key.C, ModifierKeys.Control)]
    public void CopyItem(object obj)
    {
        ClipboardUtil.CopyObject(obj);
    }

    [Priority(4)]
    [MenuItem("粘贴(Ctrl+V)", "/WinCore;component/Res/paste.ico", Key.V, ModifierKeys.Control)]
    public void PasteItem(object obj)
    {
        var clone = ClipboardUtil.PasteObject<T>();
        if (clone != null)
        {
            Insert(IndexOf((T)obj) + 1, clone);
        }
    }
}