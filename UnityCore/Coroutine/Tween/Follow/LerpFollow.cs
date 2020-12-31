using UnityEngine;
using UnityCore;

/// <summary>
/// 插值跟随
/// </summary>
public class LerpFollow : MonoBehaviourExtended
{
    [ComponentProperty("操作属性"), ComponentFPType(typeof(Vector3))]
    public ComponentProperty cp;

    public GameObject obj;

    [ComponentProperty("跟随目标属性"), ComponentFPType(typeof(Vector3)), ComponentFP("obj")]
    public ComponentProperty vp;

    [TextBox("插值系数")]
    public float t;
    void Update()
    {
        cp.Value = Vector3.LerpUnclamped((Vector3)cp.Value, (Vector3)vp.Value, t);
    }
}
