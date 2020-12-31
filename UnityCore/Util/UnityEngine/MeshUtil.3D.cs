using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static partial class MeshUtil
    {
        public class PP
        {
            public int id;
            public List<int> nexts = new List<int>();
            public void AddOrRemove(int i)
            {
                if (nexts.Contains(i))
                {
                    nexts.Remove(i);
                }
                else
                {
                    nexts.Add(i);
                }
            }
        }

        /// <summary>
        /// 获取边缘顶点向外的法线方向
        /// </summary>
        public static Vector3[] GetEdgeNormalDir(this Mesh mesh)
        {
            var triangles = mesh.triangles;
            var vertices = mesh.vertices;
            var pAry = new PP[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                pAry[i] = new PP() { id = i };
            }
            System.Action<int, int> AddLine = (a, b) =>
            {
                pAry[triangles[a]].AddOrRemove(triangles[b]);
                pAry[triangles[b]].AddOrRemove(triangles[a]);
            };
            for (int i = 0; i < triangles.Length; i += 3)
            {
                AddLine(i, i + 1);
                AddLine(i + 1, i + 2);
                AddLine(i + 2, i);
            }

            var dirAry = new Vector3[vertices.Length];

            int index = 0;
            int prev = pAry[0].nexts[0];
            do
            {
                pAry[index].nexts.Remove(prev);
                var dir = vertices[prev] - vertices[pAry[index].nexts[0]];
                dir.Normalize();
                dirAry[index] = new Vector3(dir.y, -dir.x, 0);
                prev = index;
                index = pAry[index].nexts[0];
            } while (index != 0);

            return dirAry;
        }

        /// <summary>
        /// 获模型的边缘顶点,按顺序排列
        /// </summary>
        public static Vector3[] GetEdge(this Mesh mesh, out int[] indexs)
        {
            
            var triangles = mesh.triangles;
            var pAry = new PP[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                pAry[i] = new PP() { id = i };
            }
            System.Action<int, int> AddLine = (a, b) =>
            {
                pAry[triangles[a]].AddOrRemove(triangles[b]);
                pAry[triangles[b]].AddOrRemove(triangles[a]);
            };
            for (int i = 0; i < triangles.Length; i += 3)
            {
                AddLine(i, i + 1);
                AddLine(i + 1, i + 2);
                AddLine(i + 2, i);
            }
            int index = 0;
            int prev = pAry[0].nexts[0];
            do
            {
                pAry[index].nexts.Remove(prev);
                prev = index;
                index = pAry[index].nexts[0];
            } while (index != 0);

            var vertices = mesh.vertices;
            indexs = new int[vertices.Length];
            var vesAry = new Vector3[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                indexs[i] = index;
                vesAry[i] = vertices[index];
                index = pAry[index].nexts[0];
            }
            return vesAry;
        }
        

        /// <summary>
        /// 将一个任一多边形生成网格,顶点索引顺时针的面为正面.
        /// </summary>
        public static Mesh Create3DByPoly(IList<Vector3> verts,System.Func<Vector3,Vector2> ToVec2,bool make_uv=false)
        {
            var mesh = new Mesh();
            mesh.vertices = verts.ToArray();
            var tris = new UnityCore.Triangulator(verts.ToList().ConvertAll(x=>ToVec2(x)).ToArray());
            
            mesh.triangles = tris.Triangulate();

            var normals = new Vector3[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; ++i)
            {
                normals[i] = new Vector3(0, 1, 0);
            }

            mesh.normals = normals;

            if(make_uv && mesh.vertices.Length%2==0)
            {
                var uv = new Vector2[mesh.vertices.Length];
                var sideCount = mesh.vertices.Length / 2;
                for (int i=0;i<sideCount;i++)
                {
                    var t = i * 1f / (sideCount-1);
                    uv[i] = new Vector2(t, 0);
                    uv[mesh.vertices.Length-i-1] = new Vector2(t, 1);
                }
                mesh.uv = uv;
            }
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            return mesh;
        }

        /// <summary>
        /// 将一个管道多边形生成网格,顶点索引顺时针的面为正面.
        /// </summary>
        public static Mesh Create3DByPipeline(IList<Vector3> verts)
        {
            var mesh = new Mesh();
            var indexes = new List<int>();
            for (var i = 0; i < verts.Count; i++)
            {
                indexes.Add(i);
            }
            mesh.vertices = verts.ToArray();
            var faces = verts.Count - 2;
            var tris = new List<int>();
            for (int i = 0, j = verts.Count - 1; i < verts.Count / 2-1; i++,j--)
            {
                tris.AddRange(new int[] { i, i + 1, j - 1 });
                tris.AddRange(new int[] { j, i , j - 1 });
            }

            mesh.triangles = tris.ToArray();

            var normals = new Vector3[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; ++i)
            {
                normals[i] = new Vector3(0, 1, 0);
            }

            mesh.normals = normals;

            if ( mesh.vertices.Length % 2 == 0)
            {
                var uv = new Vector2[mesh.vertices.Length];
                var sideCount = mesh.vertices.Length / 2;
                for (int i = 0; i < sideCount; i++)
                {
                    var t = i * 1f / (sideCount - 1);
                    uv[i] = new Vector2(t, 0);
                    uv[mesh.vertices.Length - i - 1] = new Vector2(t, 1);
                }
                mesh.uv = uv;
            }
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            return mesh;
        }
        /// <summary>
        /// 将一条在同一平面上的闭合曲线或者闭合折线 上的点转化成网格
        /// 不计算uv
        /// </summary>
        /// <param name="mesh">面片</param>
        /// <param name="verts">顶点</param>
        /// <param name="lineWidth">延平面拓展的线条宽</param>
        /// <param name="depth">垂直于平面拓展的线条厚度</param>
        public static void Create3DByClose(this Mesh mesh, IList<Vector3> verts, float lineWidth, float depth)
        {
            mesh.Clear();
            List<CurveLinePoint> linePoints = new List<CurveLinePoint>();
            for (int i = 0; i < verts.Count; i++)
            {
                linePoints.Add(new CurveLinePoint(verts[i].x, verts[i].y));
            }
            mesh.vertices = MakeDepth(MakeVertices(linePoints, lineWidth, false), depth);

            mesh.Make3DTriangles();

            mesh.RecalculateNormals();
        }
        /// <summary>
        /// 将一条在同一平面上的闭合曲线或者闭合折线 上的点转化成网格
        /// 不计算uv
        /// </summary>
        public static void Create3DByClose(this Mesh mesh, IList<Vector3> verts,  float depth)
        {
            mesh.Clear();

            mesh.vertices = MakeDepth(verts, depth);

            mesh.triangles = MakeTriangles(mesh.vertices,true,true);

            mesh.RecalculateNormals();
        }
        /// <summary>
        /// 生成顶点
        /// </summary>
        static void Make3DVertices(this Mesh mesh, List<CurveLinePoint> linePoints, float lineWidth, float depth)
        {
            mesh.vertices = MakeDepth(MakeVertices(linePoints, lineWidth, false),depth);
        }
        /// <summary>
        /// 将所有顶点往上下两个方向拓展
        /// </summary>
        static Vector3[] MakeDepth(IList<Vector3> _verts, float depth)
        {
            var verts = new Vector3[_verts.Count * 2];
            depth = depth / 2;
            for (int i = 0; i < _verts.Count; i++)
            {
                verts[i * 2] = _verts[i] + Vector3.forward * depth;
                verts[i * 2 + 1] = _verts[i] + Vector3.back * depth;
            }
            return verts;
        }
        /// <summary>
        /// 顺时针
        /// 
        /// 0 2
        /// 1 3
        /// </summary>
        static int[] clockwise_3dindexs = { 0, 2, 1, 1, 2, 3 };//{ 0, 3, 1, 0, 2, 3 };

        /// <summary>
        /// 逆时针
        /// </summary>
        public static int[] anti_clockwise_3dindexs =
            {
                0,6,4,
                0,2,6,
                0,4,1,
                1,4,5,
                2,3,6,
                6,3,7
            };

        /// <summary>
        /// 生成绘制三角形
        /// </summary>
        static void Make3DTriangles(this Mesh mesh)
        {
            var verts = mesh.vertices;
            var len = verts.Length;

            int[] indexs = anti_clockwise_3dindexs;
            //生成三角形索引
            int[] _triangles = new int[(len / 4) * indexs.Length];

            for (int i = 0, k = 0; i < _triangles.Length; i += indexs.Length, k += 4)
            {
                for (int j = 0; j < indexs.Length; j++)
                {
                    _triangles[i + j] = k + indexs[j];
                }
            }

            for (int i = _triangles.Length - indexs.Length; i < _triangles.Length; i++)
            {
                if (_triangles[i] >= len)
                {
                    _triangles[i] -= len;
                }
            }

            mesh.triangles = _triangles;
        }

        /// <summary>
        /// 拆分由边缘生成的模型成各个子模型
        /// </summary>
        public static GameObject[] SplitModels(Mesh targetMesh,Mesh makeMesh,Vector3[] edgeVerts, Transform parent)
        {
            parent.RemoveAllChildren();
            GameObject[] objs = new GameObject[edgeVerts.Length];
            for (int j = 0; j < edgeVerts.Length; j++)
            {
                var obj = new GameObject();
                obj.transform.SetParent(parent);
                obj.name = j.ToString();
                var mf = obj.AddComponent<MeshFilter>();
                var mr = obj.AddComponent<MeshRenderer>();
                var mesh = new Mesh();
                var s = new Vector3[8];

                var center = Vector3.zero;
                for (int i = 0; i < s.Length; i++)
                {
                    var index = i + j * 4;
                    if (index >= makeMesh.vertices.Length) index -= makeMesh.vertices.Length;
                    s[i] = makeMesh.vertices[index];
                    center += s[i];
                }
                center = center / 8;
                Quaternion q;
                if (j + 1 != edgeVerts.Length)
                {
                    q = Quaternion.LookRotation(edgeVerts[j] - edgeVerts[j + 1], Vector3.forward);
                }
                else
                {
                    q = Quaternion.LookRotation(edgeVerts[j] - edgeVerts[0], Vector3.forward);
                }

                var mattx = Matrix4x4.TRS(center, q, Vector3.one);
                var matt = mattx.inverse;
                var aeawe = Matrix4x4.Translate(center);
                for (int i = 0; i < s.Length; i++)
                {
                    s[i] = matt.MultiplyPoint(s[i]);
                }
                mesh.vertices = s;

                var ss = new int[18];
                for (int i = 0; i < ss.Length; i++)
                {
                    ss[i] = makeMesh.triangles[i];
                }
                mesh.triangles = ss;
                mesh.RecalculateNormals();
                obj.transform.SetWorldMatrix(mattx);
                mf.mesh = mesh;
                objs[j] = obj;
            }
            return objs;
        }
    }
}
