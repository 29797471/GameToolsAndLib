using UnityEditor;
using UnityEngine;

/// <summary>
/// 二维bool数组编辑器
/// </summary>
public class FormMapWindow :EditorWindow
{
    public static void Edit(ref bool[,] _map,int initY=10,int initX=10)
    {
        if (_map == null) _map = new bool[initY, initX];
        FormMapWindow window = (FormMapWindow)EditorWindow.GetWindow(typeof(FormMapWindow));
        window.map = _map;
        window.Show();
    }
    bool[,] map;
    Rect rect = new Rect(100,100,500,500);
    [MenuItem("Window/二维bool数组编辑器")]
    static void Init()
    {
        FormMapWindow window = (FormMapWindow)EditorWindow.GetWindow(typeof(FormMapWindow));
        
        window.Show();
    }
    void OnGUI()
    {
        CreateButton();
    }
    void CreateButton()
    {
        if (map == null) map = new bool[10, 10];
        EditorGUILayout.BeginVertical();
        if (true)
        {
            var w = map.GetLength(0);
            var h = map.GetLength(1);
            for (int i = 0; i < w; i++)
            {
                for(int j=0;j<h;j++)
                {
                    if (GUI.Button(new Rect(j * 60 + 20, i * 40 + 40, 50, 30), map[i,j] ? "x" : " "))
                    {
                        map[i, j] = !map[i, j];
                        //Debug.Log(map.GetValue(i / map.mapLength, i % map.mapLength).ToString() + "," + (i / map.mapLength).ToString() + "," + (i % map.mapLength).ToString());
                    }
                }
                
            }
        }
        EditorGUILayout.EndVertical();
    }
    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }
}
