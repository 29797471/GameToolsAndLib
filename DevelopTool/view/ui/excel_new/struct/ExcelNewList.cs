using CqCore;
using DevelopTool;
using System.Collections.ObjectModel;

/// <summary>
/// 附带右键添加删除菜单的列表结构
/// </summary>
[Editor()]
public class ExcelNewList<T> : ObservableCollection<T>
{
    [MenuItem("打开文件(双击)"), Priority(1)]
    public void OnOpenExcel(ExcelNewData obj)
    {
        if (obj != null)
        {
            FileOpr.RunByRelativePath(obj.Path);
        }
    }

    [MenuItem("打开Excel位置"), Priority(2)]
    public void OnOpenFolder(ExcelNewData obj)
    {
        if (obj != null)
        {
            ProcessUtil.OpenFileOrFolderByExplorer(obj.Path);
        }
    }
    [MenuItem("打开配置dat位置"), Priority(3)]
    public void OnOpenConfig(ExcelNewData obj)
    {
        if (obj != null)
        {
            ProcessUtil.OpenFileOrFolderByExplorer(
                ExcelNewModel.instance.setting.ExcelDatPath + @"\" + obj.ShortName + ".dat");
        }
    }
    //[MenuItem("重新载入Excel"), Priority(4)]
    //public void OnUpdateExcel(ExcelData obj)
    //{
    //    if (obj != null)
    //    {
    //        obj.ReLoad();
    //    }
    //}
}
