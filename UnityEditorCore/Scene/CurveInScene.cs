using CqCore;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCore
{
    //2D曲线编辑器,已经过时
    //internal class CurveInScene:Singleton<CurveInScene>
    //{

    //    const float lineWidth = 1f;
    //    /// <summary>
    //    /// 顶点所在平面
    //    /// </summary>
    //    Plane plane;
    //    //鼠标操作的点索引
    //    int nearIndex = -1;

    //    //上一次鼠标在平面上的位置
    //    Vector3 mousePos3D;

    //    /// <summary>
    //    /// 上一次多边形所在的矩阵
    //    /// </summary>
    //    Matrix4x4 lastMat;

    //    bool mHasSelect = false;
    //    bool HasSelect
    //    {
    //        set
    //        {
    //            if (mHasSelect != value)
    //            {
    //                mHasSelect = value;
    //                HandleUtility.Repaint();
    //                SceneView.RepaintAll();
    //            }
    //        }
    //        get
    //        {
    //            return mHasSelect;
    //        }
    //    }

    //    Causality<MonoBehaviour, List<CurveEditAttribute>> causality;

    //    [InitializeOnLoadMethod]
    //    static void Init()
    //    {
    //        instance.causality = new Causality<MonoBehaviour, List<CurveEditAttribute>>
    //            (b =>AssemblyUtil.GetMemberAttributesInObject<CurveEditAttribute>(b));
    //        SceneView.onSceneGUIDelegate += instance.OnSceneGUICall;
    //    }

        
    //    private void OnSceneGUICall(SceneView sceneView)
    //    {
    //        if (Application.isPlaying) return;
    //        var _HasDrawEdit = false;
    //        if (Selection.activeGameObject!=null)
    //        {
    //            var mbs=Selection.activeGameObject.GetComponentsInChildren<MonoBehaviour>();

    //            foreach(var mb in mbs)
    //            {
    //                if(mb != null && mb.enabled && mb.gameObject.activeInHierarchy)
    //                {
    //                    var list = causality.Call(mb);
    //                    foreach (var att in list)
    //                    {
    //                        if (_HasDrawEdit)
    //                        {
    //                            ReSetColor(DrawInScene, att);
    //                        }
    //                        else
    //                        {
    //                            _HasDrawEdit = true;
    //                            ReSetColor(EditInScene, att);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        HasSelect = _HasDrawEdit;
    //    }
    //    /// <summary>
    //    /// 当顶点不够构成一个平面时,初始化3个顶点
    //    /// </summary>
    //    private void Check(IList<Vector2> list, float len)
    //    {
    //        if (list == null) list = new List<Vector2>();
    //        if (list.Count < 3)
    //        {
    //            list.Clear();
    //            var delta = Mathf.PI * 2 / 3;
    //            for (float i = 0; i < Mathf.PI * 2; i += delta)
    //            {
    //                list.Add(new Vector2(Mathf.Sin(i), Mathf.Cos(i)) * len);
    //            }
    //        }
    //    }

    //    void DrawInScene(CurveEditAttribute att)
    //    {
    //        List<Vector2> list = att.Target as List<Vector2>;
    //        MonoBehaviour mb = att.Mono;
    //        Camera cam = Camera.current;
            
    //        var transform = mb.transform;
    //        var worldMat = transform.localToWorldMatrix;
    //        var localMat = transform.worldToLocalMatrix;
    //        var list3D = list.ConvertAll(x => worldMat.MultiplyPoint(x));
    //        var btnSize = Vector3.Distance(cam.transform.position, transform.position) / 100f;

    //        Check(list, btnSize * 15);

    //        DrawLines(list3D,att.IsClose,att.A,att.B);
    //    }



    //    /// <summary>
    //    /// 绘制曲线
    //    /// </summary>
    //    void DrawLines(List<Vector3> list,bool isClose,float a,float b)
    //    {
    //        if(isClose)
    //        {
    //            var nSize = list.Count ;

    //            for (int i = 0; i < nSize; i++)
    //            {
    //                var prev = list.GetItemByRound(i - 1);
    //                var p1 = list[i];
    //                var p2 = list.GetItemByRound(i + 1);
    //                var next = list.GetItemByRound(i + 2);
    //                var startTangent = p1 + (p2 - prev) * a;
    //                var endTangent = p2 - (next - p1) * b;
    //                Handles.DrawBezier(p1, p2, startTangent, endTangent, Color.red, null, lineWidth);
    //            }
    //        }
    //        else
    //        {
    //            var nSize = list.Count -  1;

    //            for (int i = 0; i < nSize; i++)
    //            {
    //                var prev = list.GetItemByRange(i - 1);
    //                var p1 = list[i];
    //                var p2 = list.GetItemByRange(i + 1);
    //                var next = list.GetItemByRange(i + 2);
    //                var startTangent = p1 + (p2 - prev) * a;
    //                var endTangent = p2 - (next - p1) * b;
    //                Handles.DrawBezier(p1, p2, startTangent, endTangent, Color.red, null, lineWidth);
    //            }
    //        }
    //    }
        
    //    void ReSetColor<T>(Action<T> Draw, T t)
    //    {
    //        var Handlescolor = Handles.color;
    //        var GUIcolor = GUI.color;
    //        Draw(t);
    //        Handles.color = Handlescolor;
    //        GUI.color = GUIcolor;
    //    }
    //    void ReSetColor(Action Draw)
    //    {
    //        var Handlescolor = Handles.color;
    //        var GUIcolor = GUI.color;
    //        Draw();
    //        Handles.color = Handlescolor;
    //        GUI.color = GUIcolor;
    //    }
    //    void EditInScene(CurveEditAttribute att)
    //    {
    //        DrawInScene(att);
    //        List<Vector2> list = att.Target as List<Vector2>;
    //        Camera cam = Camera.current;
    //        var evt = Event.current;

    //        var transform = att.Mono.transform;
    //        var worldMat = transform.localToWorldMatrix;
    //        var localMat = transform.worldToLocalMatrix;
    //        var list3D = list.ConvertAll(x => worldMat.MultiplyPoint(x));
    //        var btnSize = Vector3.Distance(cam.transform.position, transform.position) / 100f;

    //        #region 更新多边形所在平面
    //        if (lastMat != worldMat)
    //        {
    //            lastMat = worldMat;
    //            var a = lastMat.MultiplyPoint(Vector3.up);
    //            var b = lastMat.MultiplyPoint(Vector2.zero);
    //            var c = lastMat.MultiplyPoint(Vector2.right);
    //            plane = new Plane(a, b, c);
    //        }
    //        #endregion

    //        {
    //            Handles.color = Color.white;
    //            #region 移动时计算和哪个顶点比较近,获得索引
    //            if (evt.type == EventType.MouseMove)
    //            {
    //                var _mousePos = evt.GetMousePointInPlane(cam, plane);
    //                if (_mousePos != null)
    //                {
    //                    mousePos3D = (Vector3)_mousePos;
    //                    var mousePos2D = (Vector2)localMat.MultiplyPoint(mousePos3D);

    //                    nearIndex = -1;
    //                    for (int i = 0; i < list3D.Count; i++)
    //                    {
    //                        if (Vector3.Distance(list3D[i], mousePos3D) < btnSize * 1.5f)
    //                        {
    //                            nearIndex = i;
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            #endregion
                
    //            #region 绘制顶点索引
    //            GUI.color = Color.black;
    //            for (int i = 0; i < list3D.Count; i++)
    //            {
    //                Handles.Label(list3D[i], i.ToString());
    //            }
    //            #endregion

    //            #region 鼠标靠近顶点时绘制顶点移动句柄
    //            if (nearIndex != -1)
    //            {
    //                //Debug.Log(list3D[nearIndex]);
    //                var p = Handles.DoPositionHandle(list3D[nearIndex], worldMat.rotation);
    //                if (list3D[nearIndex] != p)
    //                {
    //                    list[nearIndex] = localMat.MultiplyPoint(p);
    //                    //attribute.IsDirty = true;
    //                }
    //            }
    //            #endregion
    //        }
    //    }

    //}
}
