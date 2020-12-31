using CqCore;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Svn窗口操作扩展
/// </summary>
public class SVNUtil : Editor
{

    /// <summary>
    /// 打开SVN提交窗口,提交与Project面板选中文件列表以及所有依赖文件.
    /// </summary>
    /// <param name="ConverterPath">相对路径转换函数</param>
    /// <param name="ignoreList">忽略文件类型</param>
    public static void UpLoadSvn(System.Converter<string,string> ConverterPath = null,string[] ignoreList=null)
    {
        AssetDatabase.SaveAssets();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var strs = AssetDatabase.GetDependencies(path).ToList();
        //排除cs,shader文件
        if(ignoreList!=null)
        {
            strs = strs.FindAll(x => !ignoreList.Contains(FileOpr.GetNameByExtension(x)));
        }
        if(ConverterPath != null)
        {
            strs = strs.ConvertAll(ConverterPath);
        }
        var files = new List<string>();
        //添加unityMeta文件
        foreach(var file in strs)
        {
            files.Add(file);
            files.Add(file + ".meta");
        }
        Debug.Log(files.Count + "\n" + Torsion.Serialize(files));

        ProcessUtil.SVNCommit(files);
    }
}
