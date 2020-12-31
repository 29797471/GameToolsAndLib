using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 提供一些基本图形绘制
    /// </summary>
    public class HelpDrawMono : MonoBehaviourExtended
    {
        List<HelpLineData> list = new List<HelpLineData>();
        List_GC<List<Matrix4x4>> listMat=new List_GC<List<Matrix4x4>>();
        const int listMatMaxCount = 1023;

        /// <summary>
        /// 线条颜色
        /// </summary>
        [Color("线条颜色")]
        public Color color = Color.white;

        [TextBox("线宽")]
        public float lineWidth;

        [CheckBox("虚线")]
        public bool isDottedLine;

        [CqLabel("虚线")]
        public Material matDottedLine;
        [CqLabel("实线")]
        public Material matLine;
        [CqLabel("圆")]
        public Material matCircle;
        [CqLabel("圆环")]
        public Material matRing;

        Quaternion defaultQuat = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 按调用处理方式分两种:1.调用即时绘制(外部调用时需放在update内) 2.调用只计算,在统一绘制接口里面绘制
        /// </summary>
        public HelpDrawStyle HelpDrawStyle
        {
            get
            {
                return mHelpDrawStyle;
            }
            set
            {
                if (mHelpDrawStyle != value)
                {
                    mHelpDrawStyle = value;
                }
            }
        }
        [ComBox("绘制方式", ComBoxStyle.RadioBox)]
        public HelpDrawStyle mHelpDrawStyle;
        void OnDrawGizmos()
        {
            if (mHelpDrawStyle != HelpDrawStyle.Gizmos) return;
            for (int i = 0; i < list.Count; i++)
            {
                var it = list[i];
                Gizmos.color = it.color;
                Gizmos.DrawLine(it.a, it.b);
            }
        }
        void Awake()
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(obj);
        }
        Mesh mesh;
        public void Clear()
        {
            list.Clear();
            listMat.Clear();
        }
        void Update()
        {
            switch (mHelpDrawStyle)
            {
                case HelpDrawStyle.None:
                    break;
                case HelpDrawStyle.Debug:
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            var it = list[i];
                            Debug.DrawLine(it.a, it.b, it.color);
                        }
                    }
                    break;
                case HelpDrawStyle.Gizmos:
                    break;
                case HelpDrawStyle.Graphics:
                    {
                        foreach(var it in listMat)
                        {
                            Graphics.DrawMeshInstanced(mesh, 0, matLine, it, null, UnityEngine.Rendering.ShadowCastingMode.On);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region 绘制接口
        /// <summary>
        /// 绘制贝塞尔曲线
        /// </summary>
        public void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, int part = 100)
        {
            DrawCalc.DrawBezier(p0, p1, p2, part, DrawLine);
        }

        /// <summary>
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int sampling = 100)
        {
            DrawCalc.DrawBezier(p0, p1, p2, p3, sampling, DrawLine);
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        public void DrawLine(Vector3 a, Vector3 b)
        {
            if (a.EqualsByEpsilon(b)) return;
            var baseMat= transform.localToWorldMatrix;
            a = baseMat.MultiplyPoint(a);
            b = baseMat.MultiplyPoint(b);
            var data = new HelpLineData() { a = a, b = b, color = color };
            list.Add(data);
            var dir = b - a;
            var mat = Matrix4x4.TRS((a + b) / 2, Quaternion.LookRotation(dir, Vector3.up) * defaultQuat, new Vector3(lineWidth, dir.magnitude, 1));
            
            if (listMat.Count == 0) listMat.Add(new List<Matrix4x4>());
            var last=listMat.Last();
            if(last.Count== listMatMaxCount)
            {
                last = new List<Matrix4x4>();
                listMat.Add(last);
            }
            last.Add(mat);
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        public void DrawLine(Vector2 a, Vector2 b)
        {
            DrawLine(a.ToVector3(), b.ToVector3());
        }

        /// <summary>
        /// 绘制任意角度的矩形
        /// </summary>
        public void DrawRect(Vector2 a, Vector2 b, float width)
        {
            DrawCalc.DrawRect(a,b, width, DrawLine);
        }

        /// <summary>
        /// 绘制任意角度的矩形
        /// </summary>
        public void DrawRect(Rect rect)
        {
            DrawCalc.DrawRect(rect, DrawLine);
        }

        public void DrawPolygon(IList<Vector2> vectors)
        {
            DrawCalc.DrawPolygon(vectors, DrawLine);
        }

        public void DrawBounds(Bounds bounds, Matrix4x4? worldMat = null)
        {
            DrawCalc.DrawBounds(bounds, worldMat, DrawLine);
        }

        /// <summary>
        /// 绘制胶囊
        /// </summary>
        public void DrawCapsule(Vector2 a, Vector2 b, float rad)
        {
            DrawCalc.DrawCapsule(a, b, rad, DrawLine);
        }
        /// <summary>
        /// 绘制虚线(虚实相间,动态调整虚线宽度,两边半虚线宽(可拼接其它虚线))
        /// </summary>
        public void DrawDottedLine(Vector2 a, Vector2 b, float partWidth = 1f)
        {
            DrawCalc.DrawDottedLine(a, b, partWidth, DrawLine);
        }

        /// <summary>
        /// 绘制由A指向B的线段并在中间有一个方向箭头
        /// </summary>
        public void DrawArrowLine(Vector2 a, Vector2 b, float arrowSize = 1f,
            float arrowPos = 0.618f)
        {
            if (a == b) return;
            arrowSize = Math.Min(arrowSize, Vector2.Distance(a, b) / 20);
            var center = Vector2.LerpUnclamped(a, b, arrowPos);
            DrawLine(a, b);
            DrawCalc.DrawArrow(center, b - a, arrowSize, DrawLine);
        }

        /// <summary>
        /// 绘制由A指向B的线段并在中间有一个方向箭头
        /// </summary>
        public void DrawArrow(Vector2 center, Vector2 dir, float arrowSize)
        {
            DrawCalc.DrawArrow(center, dir, arrowSize, DrawLine);
        }

        /// <summary>
        /// 绘制圆
        /// </summary>
        public void DrawCirCle(float rad, Vector2 pos, int sampling = 21)
        {
            DrawCalc.DrawCirCle(rad, pos, sampling, DrawLine);
        }

        /// <summary>
        /// 绘制圆弧
        /// </summary>
        public void DrawCirCleLine(Vector2 start, Vector2 end, Vector2 center, int sampling = 21)
        {
        }
        #endregion
    }
}
