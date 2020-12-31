using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 文本输入弹窗
/// </summary>
public class TitileContentEditorWindow : EditorWindow
{
    Action<string,string> OnResult;
    public static void Open(string edit_title,string content,Action<string,string> OnResult,string winTitle)
    {
        var it = GetWindow<TitileContentEditorWindow>(false, winTitle, true);
        it.ShowPopup();
        it.edit_title = edit_title;
        it.content = content;
        it.OnResult = OnResult;
    }
    string edit_title;
    string content;
    void OnGUI()
    {
        edit_title = EditorGUILayout.TextField(edit_title);
        content = EditorGUILayout.TextArea(content);
        if (GUILayout.Button("确定"))
        {
            OnResult(edit_title, content);
            OnResult = null;
            this.Close();
        }
    }
    private void OnDisable()
    {
        //Debug.Log("OnDisable", this);
        
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    void OnDestroy()
    {
        //Debug.Log("OnDestroy", this);
    }
}
