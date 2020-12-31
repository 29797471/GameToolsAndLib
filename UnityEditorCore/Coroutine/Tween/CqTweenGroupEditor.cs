using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CqTweenGroup))]
public class CqTweenGroupEditor : Editor
{
    public new CqTweenGroup target
    {
        get
        {
            return (CqTweenGroup)base.target;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //obj=(GameObject)EditorGUILayout.ObjectField("选择一个带缓动的目标",obj, typeof(GameObject), true);
        //if(obj!=null)
        //{
        //    if (GUILayout.Button("添加缓动"))
        //    {
        //        var list = obj.GetComponents<CqTweenGroupItem>();
        //        if (target.tweenList == null) target.tweenList = new List<CqTweenGroupItem>();
        //        foreach (var com in list)
        //        {
        //            if (!target.tweenList.Contains(com)) target.tweenList.Add(com );
        //        }
        //    }
        //}
    }
}