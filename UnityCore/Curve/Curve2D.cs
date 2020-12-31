using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 2D曲线编辑器<para/>
/// 改用CqCurveMono
/// </summary>
[InpectorDrawStyle(150)]
[System.Obsolete]
public class Curve2D : MonoBehaviour
{
    [CurveEdit("isClose","a","b")]
    [ListBox("曲线")]
    public List<Vector2> list;

    [CheckBox("闭合曲线"),OnValueChanged("ReDraw")]
    public bool isClose=true;

    [CheckBox("锁定在场景中绘制")]
    public bool lockDraw;

    [TextBox("三次贝塞尔进入系数")]
    public float a=0.25f;
    [TextBox("三次贝塞尔退出系数")]
    public float b=0.25f;

    HelpDraw helpDraw;
    void Awake()
    {
        var mat = transform.localToWorldMatrix;
        helpDraw = new HelpDraw
        {
            color = Color.red,
            HelpDrawStyle = HelpDrawStyle.Gizmos
        };
        ReDraw();
    }

    /// <summary>
    /// 长度
    /// </summary>
    public float Length
    {
        get
        {
            var mat = transform.localToWorldMatrix;
            var list3D = new List<Vector3>();
            foreach (var it in list)
            {
                list3D.Add(mat.MultiplyPoint(it));
            }
            float len = 0f;
            if (isClose)
            {
                var nSize = list.Count;

                for (int i = 0; i < nSize; i++)
                {
                    var prev = list3D.GetItemByRound(i - 1);
                    var p1 = list3D[i];
                    var p2 = list3D.GetItemByRound(i + 1);
                    var next = list3D.GetItemByRound(i + 2);
                    var startTangent = p1 + (p2 - prev) * a;
                    var endTangent = p2 - (next - p1) * b;
                    len += BezierUtil.Length(p1, startTangent, endTangent, p2);
                }
            }
            else
            {
                var nSize = list.Count - 1;

                for (int i = 0; i < nSize; i++)
                {
                    var prev = list3D.GetItemByRange(i - 1);
                    var p1 = list3D[i];
                    var p2 = list3D.GetItemByRange(i + 1);
                    var next = list3D.GetItemByRange(i + 2);
                    var startTangent = p1 + (p2 - prev) * a;
                    var endTangent = p2 - (next - p1) * b;
                    len += BezierUtil.Length(p1, startTangent, endTangent, p2);
                }
            }
            return len;
        }
    }
    public void ReDraw()
    {
        helpDraw.Clear();
        var mat = transform.localToWorldMatrix;
        var list3D = new List<Vector3>();
        foreach (var it in list)
        {
            list3D.Add(mat.MultiplyPoint(it));
        }
        if (isClose)
        {
            var nSize = list.Count;

            for (int i = 0; i < nSize; i++)
            {
                var prev = list3D.GetItemByRound(i - 1);
                var p1 = list3D[i];
                var p2 = list3D.GetItemByRound(i + 1);
                var next = list3D.GetItemByRound(i + 2);
                var startTangent = p1 + (p2 - prev) * a;
                var endTangent = p2 - (next - p1) * b;
                helpDraw.DrawBezier(p1,startTangent, endTangent,p2);
            }
        }
        else
        {
            var nSize = list.Count - 1;

            for (int i = 0; i < nSize; i++)
            {
                var prev = list3D.GetItemByRange(i - 1);
                var p1 = list3D[i];
                var p2 = list3D.GetItemByRange(i + 1);
                var next = list3D.GetItemByRange(i + 2);
                var startTangent = p1 + (p2 - prev) * a;
                var endTangent = p2 - (next - p1) * b;
                helpDraw.DrawBezier(p1,startTangent, endTangent,p2);
            }
        }
    }
}
