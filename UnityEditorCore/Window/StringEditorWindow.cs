using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 文本输入弹窗
/// </summary>
public class StringEditorWindow : EditorWindow
{
    Action<string> OnResult;
    public static void Open(string title, Action<string> OnResult)
    {
        var it = GetWindow<StringEditorWindow>(false, title, true);
        it.ShowPopup();
        it.OnResult = OnResult;
    }
    string inputText;
    void OnGUI()
    {
        inputText = EditorGUILayout.TextField(inputText);
        if (GUILayout.Button("确定"))
        {
            OnResult(inputText);
            OnResult = null;
            this.Close();
        }
    }
    private void OnDisable()
    {
        //Debug.Log("OnDisable", this);
        if (OnResult != null)
        {
            OnResult(null);
        }
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    void OnDestroy()
    {
        //Debug.Log("OnDestroy", this);
    }
}
