//using System.Collections.Generic;
//using UnityEngine;

//namespace UnityCore
//{
//    /// <summary>
//    /// 运行时显示
//    /// </summary>
//    public class HelpGraphics : HelpLineBase<HelpGraphics>
//    {
//        Mesh mesh;
//        Material mat;
//        List<HelpLineData> list = new List<HelpLineData>();
//        protected override void OnInit()
//        {
//            GlobalMono.Inst.OnUpdate += GraphicsDraw;

//            mesh = new Mesh();
//            mesh.vertices = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
//            mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
//            mesh.RecalculateNormals();
//            mat = new Material(Shader.Find("Unlit/Color"));
//        }

//        void GraphicsDraw()
//        {
//            for (int i = 0; i < list.Count; i++)
//            {
//                var it = list[i];
//                mat.color = it.color;
//                Graphics.DrawMesh(mesh, Matrix4x4.TRS((it.a + it.b) / 2,
//                    Quaternion.LookRotation(it.a - it.b, Vector3.up),
//                    new Vector3(1f, 1, Vector2.Distance(it.a, it.b))), mat, 0);
//            }
//        }
//        public override void Clear()
//        {
//            list.Clear();
//        }
//    }
//}
