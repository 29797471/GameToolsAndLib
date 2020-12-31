using CqCore;
using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;


public class AutoActionWindow : EditorWindow
{
    //[MenuItem("Assets/自动化窗口 %#&a")]
    //[MenuItem("Window/自动化窗口 %#&a")]
    static void Open()
    {
        GetWindow<AutoActionWindow>("自动化窗口");
    }
    public AutoActionWindow()
    {
        dic = new Dictionary<string, List<AutoActionMenu>>();
        var types = AssemblyUtil.GetTypesByNamespace("Automation");
        
        foreach (var type in types)
        {
            var list = AssemblyUtil.GetMethodsAttributesInType<AutoActionMenu>(type);
            if (list == null) continue;
            foreach (AutoActionMenu it in list)
            {
                if (!dic.ContainsKey(it.menu))
                {
                    dic[it.menu] = new List<AutoActionMenu>();
                }
                dic[it.menu].Add(it);
            }
        }
        foreach (var list in dic)
        {
            list.Value.Sort(x => x.priority);
        }
    }


    Dictionary<string, List<AutoActionMenu>> dic;
    int doIndex;
    string doMenu;

    void DoneAction()
    {
        if (doIndex >= dic[doMenu].Count) return;
        var act = dic[doMenu][doIndex];
        act.Invoke();
        Debug.Log(act.Name);
        GlobalCoroutine.Call(Next);
    }
    void Next()
    {
        doIndex++;
        Repaint();
        GlobalCoroutine.Call(DoneAction);
    }
    void OnGUI()
    {
        BeginWindows();
        GUILayoutUtil.Vertical(() =>
        {
            EditorGUILayout.LabelField("");
            GUILayoutUtil.Horizontal(() =>
            {
                foreach (var it in dic)
                {
                    if (GUILayout.Button(it.Key))
                    {
                        doMenu = it.Key;
                        doIndex = 0;
                        GlobalCoroutine.Call(DoneAction);
                    }
                }
            });
        }, "操作选项", GUI.skin.box);

        GUILayoutUtil.Vertical(() =>
        {
            EditorGUILayout.LabelField("");
            if (doMenu != null)
            {
                int i = 0;
                foreach (var actionMenu in dic[doMenu])
                {
                    EditorGUILayout.LabelField((i < doIndex ? "√" : "...") + actionMenu.Name);

                    i++;
                }
            }

        }, "状态", GUI.skin.box);


        EndWindows();
    }

    //更新
    void Update()
    {

    }

    /// <summary>
    /// 当窗口获得焦点时调用一次
    /// </summary>
    void OnFocus()
    {
    }

    /// <summary>
    /// 当窗口丢失焦点时调用一次
    /// </summary>
    void OnLostFocus()
    {
    }
    /// <summary>
    /// 当Hierarchy视图中的任何对象发生改变时调用一次
    /// </summary>
    void OnHierarchyChange()
    {
    }
    /// <summary>
    /// 当Project视图中的资源发生改变时调用一次
    /// </summary>
    void OnProjectChange()
    {
    }
    /// <summary>
    /// 窗口面板的更新
    /// 这里开启窗口的重绘，不然窗口信息不会刷新
    /// </summary>
    void OnInspectorUpdate()
    {
        Repaint();
    }
    /// <summary>
    /// 当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
    /// 有可能是多选，
    /// </summary>
    void OnSelectionChange()
    {
        //这里开启一个循环打印选中游戏对象的名称
        foreach (Transform t in Selection.transforms)
        {
            //Debug.Log("OnSelectionChange" + t.name);
        }
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    void OnDestroy()
    {
    }
}
