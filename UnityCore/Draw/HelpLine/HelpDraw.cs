using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 绘制辅助线
    /// </summary>
    public class HelpDraw
    {
        List<HelpLineData> list = new List<HelpLineData>();

        /// <summary>
        /// 线条颜色
        /// </summary>
        public Color color = Color.white;

        CancelHandle cancelHandle = new CancelHandle();
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
                    cancelHandle.CancelAll();
                    mHelpDrawStyle = value;
                    switch (mHelpDrawStyle)
                    {
                        case HelpDrawStyle.Debug:
                            GlobalMono.Inst.OnUpdate += DebugDraw;
                            cancelHandle.CancelAct += () => GlobalMono.Inst.OnUpdate -= DebugDraw;
                            break;
                        case HelpDrawStyle.Gizmos:
                            GlobalMono.Inst.DrawGizmos += GizmosDraw;
                            cancelHandle.CancelAct += () => GlobalMono.Inst.DrawGizmos -= GizmosDraw;
                            break;
                    }
                }
            }
        }
        HelpDrawStyle mHelpDrawStyle;

        ~HelpDraw()
        {
            Clear();
            cancelHandle.CancelAll();
        }
        public void Clear()
        {
            list.Clear();
        }
        Func<Vector2, Vector3> TranFun = x => x.ToVector3();

        /// <summary>
        /// 设置在Scene视图中显示2d坐标的转换方程
        /// </summary>
        public void SetVector2ToVector3(Func<Vector2, Vector3> TranFun)
        {
            this.TranFun = TranFun;
        }
        /// <summary>
        /// 绘制贝塞尔曲线
        /// </summary>
        public void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, int part = 100)
        {
            DrawCalc.DrawBezier(p0, p1, p2, part, DrawLine);
        }
        /// <summary>
        /// 绘制贝塞尔曲线
        /// </summary>
        public void DrawBezier(Vector2 p0, Vector2 p1, Vector2 p2, int part = 100)
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
        /// 绘制贝塞尔
        /// 插入sampling个点,分sampling+1段来绘制
        /// </summary>
        public void DrawBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int sampling = 100)
        {
            DrawCalc.DrawBezier(p0, p1, p2, p3, sampling, DrawLine);
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        public void DrawLine(Vector2 from, Vector2 to)
        {
            DrawLine(TranFun(from), TranFun(to));
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        public void DrawLine(Vector3 a, Vector3 b)
        {
            var data = new HelpLineData() { a = a, b = b, color = color };
            list.Add(data);
        }

        /// <summary>
        /// 绘制任意角度的矩形
        /// </summary>
        public void DrawRect(Vector2 a, Vector2 b, float width)
        {
            DrawCalc.DrawRect(a, b, width, DrawLine);
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

        void DebugDraw()
        {
            //CqCore.CqDebug.BeginSample("DebugDraw");
            for (int i = 0; i < list.Count; i++)
            {
                var it = list[i];
                Debug.DrawLine(it.a, it.b, it.color);
            }
            //CqCore.CqDebug.EndSample();
        }
        void GizmosDraw()
        {
            for (int i = 0; i < list.Count; i++)
            {
                var it = list[i];
                Gizmos.color = it.color;
                Gizmos.DrawLine(it.a, it.b);
            }
        }
    }
}
