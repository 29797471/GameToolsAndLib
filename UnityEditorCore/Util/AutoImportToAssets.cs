//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using UnityEditorCore;
//using System.Linq;
//using System.Collections.Generic;

///// <summary>
///// 当xml文件添加或者更新到项目时,自动更新或者生成asset
///// </summary>
//public class AutoImportToAssets : AssetPostprocessor
//{
    

//    /// <summary>
//    /// Assets目录文件变更的回调
//    /// </summary>
//    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//    {
//        return;
//        List<string> b = new List<string>();
//        foreach (var path in importedAssets)
//        {
//            if (path.Contains(".cqf"))
//            {
//                try
//                {
//                    UpdateAsset(path);
//                }
//                catch (System.Exception e)
//                {
//                    Debug.LogException(e);
//                    b.Add(path);
//                }
//            }
//            AssetDatabase.Refresh();
//        }

//        if (b.Count>0)
//        {
//            PlayerPrefs.SetString(Startup.key, Torsion.Serialize(b));
//        }
//    }
//    /// <summary>
//    /// 单个cqf文件对应的asset更新或者生成
//    /// </summary>
//    public static void UpdateAsset(string cqfPath)
//    {
//        if (string.IsNullOrEmpty(cqfPath))
//        {
//            cqfPath = EditorUtility.OpenFilePanel("choose cqf file", "", "cqf");
//        }
//        var configFolderPath = Path.GetDirectoryName(cqfPath);///获取文件所在的文件夹路径
//        string shortName = Path.GetFileNameWithoutExtension(cqfPath);
//        string assetPath = configFolderPath + "/" + shortName + ".asset";
//        AssetDatabase.DeleteAsset(assetPath);
//        Object asset = ScriptableObject.CreateInstance("Config_" + shortName);
//        //string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPath);

//        (asset as IConfig).ImportCqfData(cqfPath);//(Resources.Load(shortName) as TextAsset).text
//        AssetDatabase.CreateAsset(asset, assetPath);
//        //AssetDatabase.SaveAssets();
//        FileUtil.DeleteFileOrDirectory(cqfPath);
//        FileUtil.DeleteFileOrDirectory(cqfPath + ".meta");
//        //EditorUtility.FocusProjectWindow();
//        //Selection.activeObject = asset;

//        Debug.Log("成功写入" + assetPath);
//    }
//}
