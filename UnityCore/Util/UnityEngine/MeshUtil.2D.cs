using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public static partial class MeshUtil
    {

        /// <summary>
        /// 生成一个xz平面上的正方形图元
        /// </summary>
        public static Mesh MakeXZQuad()
        {
            var xzQuadMesh = new Mesh();
            xzQuadMesh.vertices = new Vector3[]
            {
                new Vector3(-0.5f,0, -0.5f),
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(-0.5f, 0, 0.5f),
            };
            xzQuadMesh.uv = new Vector2[]
            {
                new Vector2(0,0),
                new Vector2(1,1),
                new Vector2(1,0),
                new Vector2(0,1),
            };
            xzQuadMesh.triangles = new int[] { 0, 1, 2, 1, 0, 3 };
            xzQuadMesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
            xzQuadMesh.RecalculateTangents();
            return xzQuadMesh;
        }

        /// <summary>
        /// 将一条闭合顶点连线拓展成固定宽度的网格
        /// </summary>
        public static void Create2DByClose(this Mesh mesh, Vector2[] verts, float lineWidth, bool curve = false)
        {
            mesh.Clear();

            List<CurveLinePoint> linePoints = new List<CurveLinePoint>();

            for (int i = 0; i < verts.Length; i++)
            {
                linePoints.Add(new CurveLinePoint(verts[i].x, verts[i].y, curve));
            }

            mesh.vertices = MakeVertices(linePoints, lineWidth, true);

            mesh.MakeUV();

            mesh.triangles = MakeTriangles(mesh.vertices, true, false);

            mesh.RecalculateNormals();
        }

        /// <summary>
        /// 将一条曲线 生成2d网格
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="curve"></param>
        /// <param name="sampling"></param>
        /// <param name="scale"></param>
        /// <param name="lineWidth"></param>
        public static void CreateByLinePoints(this Mesh mesh, AnimationCurve curve, int sampling, Vector3 scale, float lineWidth)
        {
            mesh.Clear();

            var curvesPoints = curve.Sampling(sampling);
            for (int i = 0; i < curvesPoints.Count; i++)
            {
                curvesPoints[i].pos = Vector3.Scale(curvesPoints[i].pos, scale);
            }
            mesh.vertices = MakeVertices(curvesPoints, lineWidth, false);

            mesh.MakeUV();

            mesh.triangles = MakeTriangles(mesh.vertices, true, false);

            mesh.RecalculateNormals();
        }

        /// <summary>
        /// 生成顶点
        /// </summary>
        static Vector3[] MakeVertices(List<CurveLinePoint> linePoints, float lineWidth, bool close = false)
        {
            if (close)
            {
                linePoints[0].prev = linePoints[linePoints.Count - 1];
                linePoints[linePoints.Count - 1].next = linePoints[0];
            }
            else
            {
                linePoints[0].isCurveStartPoint = true;
                linePoints[linePoints.Count - 1].isCurveStartPoint = true;
                linePoints[0].prev = linePoints[0];
                linePoints[linePoints.Count - 1].next = linePoints[linePoints.Count - 1];
            }
            for (int i = 0; i < linePoints.Count - 1; i++)
            {
                linePoints[i].next = linePoints[i + 1];
            }
            for (int i = 1; i < linePoints.Count; i++)
            {
                linePoints[i].prev = linePoints[i - 1];
            }

            //生成顶点
            var verts = new Vector3[linePoints.Count * 2 + (close ? 2 : 0)];
            for (int i = 0; i < linePoints.Count; i++)
            {
                linePoints[i].MakeVerts(verts, i * 2, lineWidth);
            }

            if (close)
            {
                verts[linePoints.Count * 2] = verts[0];
                verts[linePoints.Count * 2 + 1] = verts[1];
            }

            return Array.FindAll(verts, vert => !float.IsNaN(vert.x) && !float.IsNaN(vert.y));
        }

        /// <summary>
        /// 生成uv
        /// </summary>
        static void MakeUV(this Mesh mesh)
        {
            var verts = mesh.vertices;
            //生成uv
            Vector2[] uv = new Vector2[verts.Length];

            //按长度等分的纹理
            var lineLength = 0f;

            for (int k = 0; k < verts.Length; k += 2)
            {
                uv[k] = new Vector2(lineLength, 1);
                uv[k + 1] = new Vector2(lineLength, 0);
                if (k + 3 < verts.Length)
                {
                    lineLength += Vector2.Distance(verts[k] + verts[k + 1], verts[k + 2] + verts[k + 3]);
                }
            }

            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(uv[i].x / lineLength, uv[i].y);
            }

            mesh.uv = uv;
        }

        /// <summary>
        /// 顺时针
        /// 
        /// 0 2
        /// 1 3
        /// </summary>
        static int[] clockwise_indexs = { 0, 2, 1, 1, 2, 3 };//{ 0, 3, 1, 0, 2, 3 };

        /// <summary>
        /// 逆时针
        /// </summary>
        static int[] anti_clockwise_indexs = { 0, 1, 2, 1, 3, 2 };//{ 0, 1, 3, 0, 3, 2 };

        /// <summary>
        /// 生成绘制三角形
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="clockwise">顺时针</param>
        /// <param name="close">闭合,首尾相连</param>
        static int[] MakeTriangles(Vector3[] verts, bool clockwise = false, bool close = false)
        {
            int[] indexs = null;
            if (clockwise)
            {
                indexs = clockwise_indexs;
            }
            else
            {
                indexs = anti_clockwise_indexs;
            }

            //生成三角形索引
            int[] _triangles = new int[(verts.Length / 2 - (close ? 0 : 1)) * indexs.Length];


            for (int i = 0, k = 0; i < _triangles.Length; i += indexs.Length, k += 2)
            {
                for (int j = 0; j < indexs.Length; j++)
                {
                    _triangles[i + j] = k + indexs[j];
                }
            }
            if (close)
            {
                var len = verts.Length;
                for (int i = _triangles.Length - indexs.Length; i < _triangles.Length; i++)
                {
                    if (_triangles[i] >= len)
                    {
                        _triangles[i] -= len;
                    }
                }
            }
            //    //if (clockwise)
            //    //{
            //    //    _triangles[_triangles.Length - 5] = 0;
            //    //    _triangles[_triangles.Length - 2] = 0;
            //    //    _triangles[_triangles.Length - 1] = 1;
            //    //}
            //    //else
            //    //{
            //    //    _triangles[_triangles.Length - 4] = 0;
            //    //    _triangles[_triangles.Length - 2] = 1;
            //    //    _triangles[_triangles.Length - 1] = 0;
            //    //}
            //}

            //Vector3[] temp = new Vector3[indexs.Length];
            //for (int i = 0, k = 0; i < _triangles.Length; i += indexs.Length, k += 2)
            //{
            //    for (int j = 0; j < indexs.Length; j++)
            //    {
            //        temp[j] = verts[_triangles[i+j]];
            //    }
            //    if (!IsClockwise(temp[0], temp[1], temp[2]))
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            _triangles[i + j] = 0;
            //        }
            //        //_triangles[i + 4] = _triangles[i + 4] - 2;
            //    }
            //    else if(!IsClockwise(temp[3], temp[4], temp[5]))
            //    {
            //        for (int j = 3; j < 6; j++)
            //        {
            //            _triangles[i + j] = 0;
            //        }
            //    }
            //}
            return _triangles;
        }
        /// <summary>
        /// a-b-c组成的空间三角形是顺时针
        /// </summary>
        public static bool IsClockwise(Vector3 a, Vector3 b, Vector3 c)
        {
            var dir = Vector3.Cross(a - b, c - b);
            return dir.z > 0;
        }
    }
}
