
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.Callbacks;

///// <summary>
///// 自定义资源在UnityEditor下的打开方式
///// </summary>
//public class CustomAssetHandler
//{

//    [OnOpenAssetAttribute(1)]
//    public static bool step1(int instanceID, int line)
//    {
//        string name = EditorUtility.InstanceIDToObject(instanceID).name;
//        Debug.Log("Open Asset step: 1 (" + name + ")");
//        return false; // we did not handle the open
//    }

//    // step2 has an attribute with index 2, so will be called after step1
//    [OnOpenAssetAttribute(2)]
//    public static bool step2(int instanceID, int line)
//    {

//        string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
//        string name = Application.dataPath + "/" + path.Replace("Assets/", "");


//        if (name.EndsWith(".cqf") || name.EndsWith(".dat"))
//        {
//            System.Diagnostics.Process process = new System.Diagnostics.Process();
//            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
//            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
//            startInfo.FileName = "C:\\Windows\\System32\\notepad.exe";
//            startInfo.Arguments = name;
//            process.StartInfo = startInfo;
//            process.Start();
//            return true;
//        }
//        // Debug.Log("Open Asset step: 2 (" + name + ")");
//        return false; // we did not handle the open
//    }
//}
