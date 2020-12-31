using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 一条可闭合的曲线路径
/// </summary>
public class CqPath : MonoBehaviourExtended
{
    public CqCurve curve;
    public Vector3 this[float k]
    {
        get
        {
            return transform.localToWorldMatrix.MultiplyPoint(curve[k]);
        }
    }

    public System.Action OnDrawGizmos_Editor;
    public System.Action OnDrawGizmosSelected_Editor;

    private void OnDrawGizmos()
    {
        if (OnDrawGizmos_Editor != null) OnDrawGizmos_Editor();
    }
    private void OnDrawGizmosSelected()
    {
        if (OnDrawGizmosSelected_Editor != null) OnDrawGizmosSelected_Editor();
    }
}
