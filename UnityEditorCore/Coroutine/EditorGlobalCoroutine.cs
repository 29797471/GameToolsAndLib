using CqCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 编辑器模式下的协程支持
/// </summary>
[InitializeOnLoad]
public static class EditorGlobalCoroutine
{
    /// <summary>
    /// 初始化
    /// </summary>
    static EditorGlobalCoroutine()
    {
        EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        EditorApplication.update -= Update;
        EditorApplication.update += Update;
        ApplicationUtil.Init();

        GlobalMono.DefineGetCurrentToWaitFor();
    }
    
    

    private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {
        switch(obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                {
                    //Debug.Log("进入编辑模式");
                    break;
                }
            case PlayModeStateChange.ExitingEditMode:
                {
                    //Debug.Log("退出编辑模式");
                    break;
                }
        }
    }

    static void Update()
    {
        if (Application.isPlaying) return;
        GlobalCoroutine.Update(GlobalCoroutine.GetSpanTick(Time.realtimeSinceStartup));
    }
}
