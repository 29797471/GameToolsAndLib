using UnityCore;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviourExtended))]
public class MonoBehaviourExtendedInspector : Editor
{
    public new MonoBehaviourExtended target
    {
        get
        {
            return (MonoBehaviourExtended)base.target;
        }
    }
    private void OnEnable()
    {
    }
    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        
        GUILayout.Label("测试");
        
        Handles.EndGUI();
        Debug.Log(111);
    }
}
