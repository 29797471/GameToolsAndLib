using CqCore;
using DevelopTool;
using System.Diagnostics;

/// <summary>
/// 快捷方式结构
/// </summary>
[Editor]
public class ShortcutData : NotifyObject
{
    [MenuItem("打开文件位置")]
    public void OpenFolder()
    {
        ProcessUtil.OpenFileOrFolderByExplorer(Btn.ShortcutPath);
    }

    [MenuItem("删除")]
    public void Delete()
    {
        ShortcutModel.instance.NodeList.Remove(this);
        ShortcutModel.instance.Update("NodeList");
    }

    [MenuItem("编辑")]
    public void Editing()
    {
        Btn.IsEditing = true;
    }
    [MenuItem("完成编辑",null,System.Windows.Input.Key.Enter)]
    public void CompleteEditing()
    {
        Btn.IsEditing = false;
    }
    public void Exec(object obj)
    {
        FileOpr.RunByRelativePath(Btn.ShortcutPath);
    }

    [Button, Click("Exec"),ToolTip(AttributeTarget.Self, "ShortcutPath")]
    public ShortBtn Btn
    {
        get
        {
            return mBtn;
        }
        set
        {
            mBtn = value;
            Update("Btn");
        }
    }
    public ShortBtn mBtn;

    public string ShortcutPath
    {
        get { return Btn.ShortcutPath; }
    }
     
    

}