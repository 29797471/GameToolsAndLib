//using UnityEngine;
//using UnityEditor;
//using System;
//using UnityCore;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using UnityEditorInternal;

///// <summary>
///// 继承PropertyDrawer, 必须放入Editor文件夹下
///// </summary>
//[CustomPropertyDrawer(typeof(PolygonAttribute))]
//public class PolygonAttributeDrawer : PropertySceneDrawer
//{
//    public new PolygonAttribute attribute
//    {
//        get
//        {
//            return (PolygonAttribute)base.attribute;
//        }
//    }
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

//    /// <summary>
//    /// 0.正常 1.移动顶点 2.添加
//    /// </summary>
//    static int oprState;

//    static string[] names = new string[] { "正常(n)","添加(a)", "删除(d)" };
//    static string[] keys = new string[] { "n","a", "d" };
//    static Vector2[] screenPoints = new Vector2[]
//    {
//        new Vector2(-100, 140),
//        new Vector2(-30, 300),
//    };

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

    
//    static void OnDrawGizmo()
//    {

//    }
//    protected override void OnSceneGUILayout(SerializedProperty property)
//    {
//        Debug.LogError(1111);
//        Camera cam = Camera.current;
//        var a = cam.IntoPixelRange(screenPoints[0]);
//        var b = cam.IntoPixelRange(screenPoints[1]);
//        GUILayout.BeginArea(new Rect(a, b - a));
//        GUILayout.BeginVertical();
//        //绘制4个状态切换按钮
//        for (int i = 0; i < names.Length; i++)
//        {
//            var bl = GUILayout.Toggle(oprState == i, names[i]);
//            if (bl)
//            {
//                oprState = i;
//            }
//        }
//        GUILayout.EndVertical();
//        GUILayout.EndArea();
//    }


//    protected override void OnSceneGUI(SerializedProperty property)
//    {
//        try
//        {
//            var o = property.serializedObject.targetObject;
//        }
//        catch (Exception)
//        {
//            return;
//        } 
//        Camera cam = Camera.current;
//        var evt=Event.current;
//        for (int i = 0; i < keys.Length; i++)
//        {
//            if (evt.Equals(Event.KeyboardEvent(keys[i])))
//            {
//                oprState = i;
//                break;
//            }
//        }
//        var transform = (property.serializedObject.targetObject as MonoBehaviour).transform;
//        var list = (fieldInfo.GetValue(property.serializedObject.targetObject) as List<Vector2>);
//        var worldMat = transform.localToWorldMatrix;
//        var localMat = transform.worldToLocalMatrix;
//        var list3D = list.ConvertAll(x => worldMat.MultiplyPoint(x));
//        var btnSize = Vector3.Distance(cam.transform.position, transform.position) / 100f;

//        Check(list,btnSize*15);

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


//        DrawLines(list3D);

//        {
            
//            Handles.color = Color.white;
//            #region 移动时计算和哪个顶点比较近,获得索引
//            if (evt.type== EventType.MouseMove)
//            {
//                var _mousePos = evt.GetMousePointInPlane(cam, plane);
//                if(_mousePos!=null)
//                {
//                    mousePos3D = (Vector3)_mousePos;
//                    var mousePos2D = (Vector2)localMat.MultiplyPoint(mousePos3D);

//                    nearIndex = -1;
//                    for (int i = 0; i < list3D.Count; i++)
//                    {
//                        if (oprState == 0)
//                        {
//                            if (Vector3.Distance(list3D[i], mousePos3D) < btnSize * 1.5f)
//                            {
//                                nearIndex = i;
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//            #endregion
//            if (oprState == 1)
//            {
//                int start, end;
//                Vector2 intersection;
//                var mousePos2D = (Vector2)localMat.MultiplyPoint(mousePos3D);
//                mousePos2D.MinDistance(list.ToArray(), out start, out end, out intersection);
//                Handles.color = Color.green;

//                Handles.DrawLine(mousePos3D, list3D[start]);
//                Handles.DrawLine(mousePos3D, list3D[end]);

//                //if(evt.isMouse && evt.clickCount==2)
//                if (Handles.Button(mousePos3D, Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
//                {
//                    list.Insert(end, mousePos2D);
//                    //attribute.IsDirty = true;
//                }

//                //显示到多边形最近的点
//                //if (Handles.Button(transform.localToWorldMatrix.MultiplyPoint(intersection), Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
//                //{
//                //}
//            }
//            #region 绘制删除顶点的按钮
//            if (oprState == 2)
//            {
//                for (int i = 0; i < list3D.Count; i++)
//                {
//                    if (Handles.Button(list3D[i], Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
//                    {
//                        list.RemoveAt(i);
//                        //attribute.IsDirty = true;
//                        break;
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
    
//    void DrawLines(IList<Vector3> list)
//    {
//        Handles.color = Color.red;
//        System.Action<int, int> Draw = (a, b) =>
//        {
//            Handles.DrawLine(list[a],list[b]);
//        };
//        Draw(list.Count - 1, 0);
//        for (int i = 0; i < list.Count - 1; i++)
//        {
//            Draw(i, i + 1);
//        }
//    }
//}