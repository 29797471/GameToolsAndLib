using CqCore;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCore
{
    internal class PolygonInScene:Singleton<PolygonInScene>
    {
        
        /// <summary>
        /// 顶点所在平面
        /// </summary>
        Plane plane;
        //鼠标操作的点索引
        int nearIndex = -1;

        //上一次鼠标在平面上的位置
        Vector3 mousePos3D;

        /// <summary>
        /// 上一次多边形所在的矩阵
        /// </summary>
        Matrix4x4 lastMat;

        /// <summary>
        /// 0.正常 1.移动顶点 2.添加
        /// </summary>
        int oprState;

        string[] names = new string[] { "正常(n)", "添加(a)", "删除(d)" };
        string[] keys = new string[] { "n", "a", "d" };
        Vector2[] screenPoints = new Vector2[]
        {
            new Vector2(-100, 140),
            new Vector2(-30, 300),
        };

        bool mHasSelect = false;
        bool HasSelect
        {
            set
            {
                if (mHasSelect != value)
                {
                    mHasSelect = value;
                    HandleUtility.Repaint();
                    SceneView.RepaintAll();
                }
            }
            get
            {
                return mHasSelect;
            }
        }

        Causality<MonoBehaviour, List<PolygonEditAttribute>> causality;

        [InitializeOnLoadMethod]
        static void Init()
        {
            instance.causality = new Causality<MonoBehaviour, List<PolygonEditAttribute>>
                (b =>AssemblyUtil.GetMemberAttributesInObject<PolygonEditAttribute>(b));
            SceneView.onSceneGUIDelegate += instance.OnSceneGUICall;
        }

        
        private void OnSceneGUICall(SceneView sceneView)
        {
            if (Application.isPlaying) return;
            var _HasDrawEdit = false;
            if (Selection.activeGameObject!=null)
            {
                var mbs=Selection.activeGameObject.GetComponentsInChildren<MonoBehaviour>();

                foreach(var mb in mbs)
                {
                    if(mb!=null && mb.enabled && mb.gameObject.activeInHierarchy)
                    {
                        var list = causality.Call(mb);
                        foreach (var att in list)
                        {
                            if (_HasDrawEdit)
                            {
                                ReSetColor(DrawPolygonInScene, att);
                            }
                            else
                            {
                                _HasDrawEdit = true;
                                OnKeyboardEvent();
                                ReSetColor(DrawGUIInScene);
                                ReSetColor(EditPolygonInScene, att);
                            }
                        }
                    }
                }
            }
            HasSelect = _HasDrawEdit;
        }
        /// <summary>
        /// 当顶点不够构成一个平面时,初始化3个顶点
        /// </summary>
        private void Check(IList<Vector2> list, float len)
        {
            if (list == null) list = new List<Vector2>();
            if (list.Count < 3)
            {
                list.Clear();
                var delta = Mathf.PI * 2 / 3;
                for (float i = 0; i < Mathf.PI * 2; i += delta)
                {
                    list.Add(new Vector2(Mathf.Sin(i), Mathf.Cos(i)) * len);
                }
            }
        }

        void DrawGUIInScene()
        {
            try
            {
                Handles.BeginGUI();
                Camera cam = Camera.current;
                var a = cam.IntoPixelRange(screenPoints[0]);
                var b = cam.IntoPixelRange(screenPoints[1]);
                GUILayout.BeginArea(new Rect(a, b - a));
                GUILayout.BeginVertical();
                //绘制4个状态切换按钮
                for (int i = 0; i < names.Length; i++)
                {
                    var bl = GUILayout.Toggle(oprState == i, names[i]);
                    if (bl)
                    {
                        oprState = i;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
                Handles.EndGUI();
            }
            catch (Exception)
            {
            }
            

        }
        void DrawPolygonInScene(PolygonEditAttribute att)
        {
            List<Vector2> list = att.Target as List<Vector2>;
            MonoBehaviour mb = att.Mono;
            Camera cam = Camera.current;
            var evt = Event.current;
            for (int i = 0; i < keys.Length; i++)
            {
                if (evt.Equals(Event.KeyboardEvent(keys[i])))
                {
                    oprState = i;
                    break;
                }
            }
            var transform = mb.transform;
            var worldMat = transform.localToWorldMatrix;
            var localMat = transform.worldToLocalMatrix;
            var list3D = list.ConvertAll(x => worldMat.MultiplyPoint(x));
            var btnSize = Vector3.Distance(cam.transform.position, transform.position) / 100f;

            Check(list, btnSize * 15);

            DrawLines(list3D);
        }
        void OnKeyboardEvent()
        {
            var evt = Event.current;
            for (int i = 0; i < keys.Length; i++)
            {
                if (evt.Equals(Event.KeyboardEvent(keys[i])))
                {
                    oprState = i;
                    break;
                }
            }
        }

        void EditPolygonInScene(PolygonEditAttribute att)
        {
            List<Vector2> list = att.Target as List<Vector2>;
            Camera cam = Camera.current;
            var evt = Event.current;

            var transform = att.Mono.transform;
            var worldMat = transform.localToWorldMatrix;
            var localMat = transform.worldToLocalMatrix;
            var list3D = list.ConvertAll(x => worldMat.MultiplyPoint(x));
            var btnSize = Vector3.Distance(cam.transform.position, transform.position) / 100f;

            Check(list, btnSize * 15);

            #region 更新多边形所在平面
            if (lastMat != worldMat)
            {
                lastMat = worldMat;
                var a = lastMat.MultiplyPoint(Vector3.up);
                var b = lastMat.MultiplyPoint(Vector2.zero);
                var c = lastMat.MultiplyPoint(Vector2.right);
                plane = new Plane(a, b, c);
            }
            #endregion

            DrawLines(list3D);

            {
                Handles.color = Color.white;
                #region 移动时计算和哪个顶点比较近,获得索引
                if (evt.type == EventType.MouseMove)
                {
                    var _mousePos = evt.GetMousePointInPlane(cam, plane);
                    if (_mousePos != null)
                    {
                        mousePos3D = (Vector3)_mousePos;
                        var mousePos2D = (Vector2)localMat.MultiplyPoint(mousePos3D);

                        nearIndex = -1;
                        for (int i = 0; i < list3D.Count; i++)
                        {
                            if (oprState == 0)
                            {
                                if (Vector3.Distance(list3D[i], mousePos3D) < btnSize * 1.5f)
                                {
                                    nearIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion
                if (oprState == 1)
                {
                    int start, end;
                    Vector2 intersection;
                    var mousePos2D = (Vector2)localMat.MultiplyPoint(mousePos3D);
                    mousePos2D.MinDistance(list.ToArray(), out start, out end, out intersection);
                    Handles.color = Color.green;

                    Handles.DrawLine(mousePos3D, list3D[start]);
                    Handles.DrawLine(mousePos3D, list3D[end]);

                    //if(evt.isMouse && evt.clickCount==2)
                    if (Handles.Button(mousePos3D, Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
                    {
                        list.Insert(end, mousePos2D);
                        //attribute.IsDirty = true;
                    }

                    //显示到多边形最近的点
                    //if (Handles.Button(transform.localToWorldMatrix.MultiplyPoint(intersection), Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
                    //{
                    //}
                }
                #region 绘制删除顶点的按钮
                if (oprState == 2)
                {
                    for (int i = 0; i < list3D.Count; i++)
                    {
                        if (Handles.Button(list3D[i], Quaternion.identity, btnSize, btnSize, Handles.DotHandleCap))
                        {
                            list.RemoveAt(i);
                            //attribute.IsDirty = true;
                            break;
                        }
                    }
                }
                #endregion
                #region 绘制顶点索引
                GUI.color = Color.black;
                for (int i = 0; i < list3D.Count; i++)
                {
                    Handles.Label(list3D[i], i.ToString());
                }
                #endregion

                if(oprState==0)
                {
                    #region 鼠标靠近顶点时绘制顶点移动句柄
                    if (nearIndex != -1)
                    {
                        //Debug.Log(list3D[nearIndex]);
                        var p = Handles.DoPositionHandle(list3D[nearIndex], worldMat.rotation);
                        if (list3D[nearIndex] != p)
                        {
                            list[nearIndex] = localMat.MultiplyPoint(p);
                            //attribute.IsDirty = true;
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 绘制3d闭合多边形
        /// </summary>
        void DrawLines(IList<Vector3> list)
        {
            Handles.color = Color.red;
            var nSize = list.Count;
            for (int i = 0, j = nSize - 1; i < nSize; j = i++)
            {
                Handles.DrawLine(list[i], list[j]);
            }
        }
        
        
        
        void ReSetColor<T>(Action<T> Draw, T t)
        {
            var Handlescolor = Handles.color;
            var GUIcolor = GUI.color;
            Draw(t);
            Handles.color = Handlescolor;
            GUI.color = GUIcolor;
        }
        void ReSetColor(Action Draw)
        {
            var Handlescolor = Handles.color;
            var GUIcolor = GUI.color;
            Draw();
            Handles.color = Handlescolor;
            GUI.color = GUIcolor;
        }
        
    }
}
