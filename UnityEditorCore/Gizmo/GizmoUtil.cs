using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GizmoMgr:Singleton<GizmoMgr>
{
    List<MonoBehaviour> mSelects;
    public List<MonoBehaviour> Selects
    {
        get
        {
            if (mSelects == null)
            {
                mSelects = new List<MonoBehaviour>();
            }
            return mSelects;
        }
    }

    /// <summary>
    /// 如果gameObject没有被激活一定不会被绘制
    /// GizmoType： 指定如何绘制线条，何时绘制线条
    /// NoSelected  当该gameObject未被选中，也没有被父级选中
    /// Selected 当该gameObject被选中时
    /// Pickable 若gizmo在编辑器中可被选中
    /// </summary>
    [DrawGizmo(GizmoType.NonSelected)]
    static void OnNonSelected(MonoBehaviour b, GizmoType gt)
    {
        var selects = instance.Selects;
        //if (Application.isPlaying) return;
        if (selects.Contains(b))
        {
            selects.Remove(b);
        }
    }
    [DrawGizmo(GizmoType.Selected)]
    static void OnSelected(MonoBehaviour b, GizmoType gt)
    {
        var selects = instance.Selects;
        if (selects.Contains(null))
        {
            selects.Remove(null);
        }
        if (!selects.Contains(b))
        {
            selects.Add(b);
        }
        
    }
}

