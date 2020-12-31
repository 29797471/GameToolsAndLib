using CqCore;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityCore
{
    public static partial class UnityUtil
    {
        public static Vector4 QuaternionToVector4(Quaternion rot)
        {
            return new Vector4(rot.x, rot.y, rot.z, rot.w);
        }
        public static Quaternion Vector4ToQuaternion(Vector4 v)
        {
            return new Quaternion(v.x, v.y, v.z, v.w);
        }
        

        /// <summary>
        /// 判断是否与目标重叠
        /// </summary>
        public static bool InPoint(Vector3 center, Vector3 pos)
        {
            if (Vector3.Distance(center, pos) < 0.3f)
                return true;
            else return false;
        }

        /// <summary>
        ///判断目标是否在圆形区域内 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static bool InCircle(Vector3 center, Vector3 pos, float radius)
        {
            //Debug.LogError("dis" + Vector3.Distance(center, pos) + "   "+ radius);
            if (Vector3.Distance(center, pos) <= radius)
            {
                return true;
            }
            else return false;

        }

        /// <summary>
        ///判断目标是否在扇形区域内 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static bool InSector(Transform center, Vector3 pos, float radius, float angle)
        {
            Vector2 PosV2 = new Vector2(pos.x, pos.z);
            Vector2 centerV2 = new Vector2(center.position.x, center.position.z);
            Vector2 dirV2 = new Vector2(center.forward.x, center.forward.z);
            Vector2 curdir = PosV2 - centerV2;
            if (Vector2.Distance(PosV2, centerV2) <= radius && Vector2.Angle(dirV2, curdir) <= angle)
                return true;
            else
                return false;
        }



        static public float Multiply(float px, float py, float p1x, float p1y, float p2x, float p2y)
        {
            return (px - p2x) * (p1y - p2y) - (p1x - p2x) * (py - p2y);
        }

        static public bool IsinRect(Vector3 judgePoint, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            float x = judgePoint.x;
            float y = judgePoint.z;

            float x1 = v1.x;
            float y1 = v1.z;

            float x2 = v2.x;
            float y2 = v2.z;

            float x3 = v3.x;
            float y3 = v3.z;

            float x4 = v4.x;
            float y4 = v4.z;
            if (Multiply(x, y, x1, y1, x2, y2) * Multiply(x, y, x4, y4, x3, y3) <= 0 && Multiply(x, y, x1, y1, x4, y4) * Multiply(x, y, x2, y2, x3, y3) <= 0)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 判断目标是否在矩形区域内
        /// </summary>
        /// <param name="center"></param>
        /// <param name="pos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool InRect(Transform center, Vector3 pos, float width, float height)
        {

            Vector3 left = center.position - width * center.right;

            Vector3 right = center.position + width * center.right;

            Vector3 leftEnd = left + center.forward * height;

            Vector3 rightEnd = right + center.forward * height;

            //Debug.DrawLine(center.position, left, Color.red);
            //Debug.DrawLine(center.position, right, Color.red);
            //Debug.DrawLine(left, leftEnd, Color.red);
            //Debug.DrawLine(right, rightEnd, Color.red);
            //Debug.DrawLine(leftEnd, rightEnd, Color.red);

            Debug.LogError("vert" + "" + left + "" + right + "" + leftEnd + "" + rightEnd);
            return IsinRect(pos, left, leftEnd, rightEnd, right);
        }



        public static Vector3 InverseLerpVe3(Vector3 from, Vector3 to, Vector3 value)
        {
            Vector3 a;
            a.x = Mathf.InverseLerp(from.x, to.x, value.x);
            a.y = Mathf.InverseLerp(from.y, to.y, value.y);
            a.z = Mathf.InverseLerp(from.z, to.z, value.z);
            return a;
            //return ((to - from) / (value - from));
        }
        
        /// <summary>
        /// 修正规划路线
        /// 1.去掉不可行走的路径点
        /// 2.补足成实际路线
        /// </summary>
        public static Vector3[] AutoModifiPath(Vector3 sourcePos, Vector3 targetPos)
        {

            NavMeshPath path = new NavMeshPath();
            bool res = NavMesh.CalculatePath(sourcePos, targetPos, -1, path);
            if (!res)
            {
                NavMeshHit hit1 = new NavMeshHit();
                NavMeshHit hit2 = new NavMeshHit();
                NavMesh.SamplePosition(sourcePos, out hit1, 2f, 1);
                NavMesh.SamplePosition(targetPos, out hit2, 2f, 1);
                res = NavMesh.CalculatePath(hit1.position, hit2.position, -1, path);
            }
            if (path.corners.Length <= 1)
            {
                Debug.Log("error sourcePos:" + sourcePos + " targetPos:" + targetPos + " path.corners.Length:" + path.corners.Length);
            }

            return path.corners;
        }
        /// <summary>
        /// 获取自带寻路的路径
        /// </summary>
        public static Vector3[] AutoModifiPath(NavMeshAgent agent, Vector3 targetPos)
        {
            NavMeshPath path = new NavMeshPath();
            bool res = agent.CalculatePath(targetPos, path);
            if (!res)
            {
                //NavMeshHit hit1 = new NavMeshHit();
                NavMeshHit hit2 = new NavMeshHit();
                NavMesh.SamplePosition(targetPos, out hit2, 2f, 1);
                res = agent.CalculatePath(hit2.position, path);
            }
            if (path.corners.Length <= 1)
            {
                Debug.Log(" targetPos:" + targetPos + " path.corners.Length:" + path.corners.Length);
            }

            return path.corners;
        }
        //点在寻路网格上判定
        public static bool InNavMeshPath(Vector3 point)
        {
            NavMeshHit hit1 = new NavMeshHit();
            return NavMesh.SamplePosition(point, out hit1, 0.05f, 1);

            //return NavMesh.FindClosestEdge(point,out hit1, -1);
            //return NavMesh.CalculatePath(Vector3.zero, point,-1,path);
        }

        private static Vector3 Interp(Vector3[] pts, float t)
        {
            int numSections = pts.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;

            Vector3 a = pts[currPt];
            Vector3 b = pts[currPt + 1];
            Vector3 c = pts[currPt + 2];
            Vector3 d = pts[currPt + 3];

            return .5f * (
                (-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }
        //由几个顶点得到顶点之间许多点组成的近似曲线的连线
        public static Vector3[] CurvePath(Vector3[] path)
        {

            Vector3[] suppliedPath;
            Vector3[] vector3s;

            //create and store path points:
            suppliedPath = path;

            //populate calculate path;
            int offset = 2;
            vector3s = new Vector3[suppliedPath.Length + offset];
            Array.Copy(suppliedPath, 0, vector3s, 1, suppliedPath.Length);

            //populate start and end control points:
            //vector3s[0] = vector3s[1] - vector3s[2];
            vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
            vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);

            //is this a closed, continuous loop? yes? well then so let's make a continuous Catmull-Rom spline!
            if (vector3s[1] == vector3s[vector3s.Length - 2])
            {
                Vector3[] tmpLoopSpline = new Vector3[vector3s.Length];
                Array.Copy(vector3s, tmpLoopSpline, vector3s.Length);
                tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
                tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
                vector3s = new Vector3[tmpLoopSpline.Length];
                Array.Copy(tmpLoopSpline, vector3s, tmpLoopSpline.Length);
            }

            //Line Draw:
            Vector3[] curve = new Vector3[path.Length * 20 + 1];
            curve[0] = Interp(vector3s, 0);
            int SmoothAmount = path.Length * 20;
            for (int i = 1; i <= SmoothAmount; i++)
            {
                float pm = (float)i / SmoothAmount;
                curve[i] = Interp(vector3s, pm);
            }
            return curve;

        }

        /// <summary>
        /// 获取圆上一个角度上的坐标点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 GetCirclePos(Vector3 center, float radius, float angle)
        {
            float pi_a = angle * Mathf.Deg2Rad;
            //Debug.LogError("cos" + (float)Math.Cos((double)pi_a) + "   " + pi_a);
            return new Vector3(radius * (float)Math.Cos((double)pi_a) + center.x, 0, radius * (float)Math.Sin((double)pi_a) + center.z);
        }

        /// <summary>
        /// 通过球面uv坐标转化成球面3d单位化位置坐标
        /// </summary>
        public static Vector3 UVToPosition(Vector2 uv)
        {
            /*
 *  三维球体的半径为r,水平转动角度为h（[0，2PI]），上下转动角度为p（[-PI/2，PI/2]），所以球面上一

点的三维坐标sphere(x, y, z)=(r* cosp* cosh, r* cosp* sinh, r* sinp)。
反向变换有p=arcsin(z/r) ，h=arctan(y/x)。

当把p对应到纹理的V方向，把H对应到纹理的U方向，UV的范围都是[0,1]。在知道球面坐标x、y，z和半

径r以后，球面点对应的纹理坐标就是V=arcsin(z/r)/PI+0.5，U=arctan(y/x)/2/PI。
 */
            var h = (uv.x + 0.75f) * 2 * Mathf.PI;//水平转动角度
            var p = (uv.y - 0.5f) * Mathf.PI;//上下转动角度
            return new Vector3(Mathf.Cos(p) * Mathf.Cos(h), -Mathf.Sin(p), Mathf.Cos(p) * Mathf.Sin(h));
        }
        /// <summary>
        /// 通过球面经纬度坐标转化成球面3d单位化位置坐标
        /// </summary>
        public static Vector3 LatitudeAndLongitudeToPosition(Vector2 uv)
        {
            var h = (uv.x+90) * Mathf.Deg2Rad;//水平转动角度
            var p = (-uv.y) * Mathf.Deg2Rad;//上下转动角度
            return new Vector3(Mathf.Cos(p) * Mathf.Cos(h), -Mathf.Sin(p), Mathf.Cos(p) * Mathf.Sin(h));
        }

        
        //InverseLerp
        public static Func<object, object, float, object> GetLerpUnclamped(Type type)
        {
            switch (AssemblyUtil.GetName(type))
            {
                case "System.Single":
                case "float":
                    {
                        return (start, end, t) => Mathf.LerpUnclamped((float)start, (float)end, t);
                    }
                
                case "UnityEngine.Quaternion":
                case "Quaternion":
                    {
                        return (start, end, t) => Quaternion.LerpUnclamped((Quaternion)start, (Quaternion)end, t);
                    }
                case "UnityEngine.Color":
                case "Color":
                    {
                        return (start, end, t) => Color.LerpUnclamped((Color)start, (Color)end, t);
                    }
                case "UnityEngine.Vector2":
                case "Vector2":
                    {
                        return (start, end, t) => Vector2.LerpUnclamped((Vector2)start, (Vector2)end, t);
                    }
                case "UnityEngine.Vector3":
                case "Vector3":
                    {
                        return (start, end, t) => Vector3.LerpUnclamped((Vector3)start, (Vector3)end, t);
                    }
                case "UnityEngine.Vector4":
                case "Vector4":
                    {
                        return (start, end, t) => Vector4.LerpUnclamped((Vector4)start, (Vector4)end, t);
                    }
                default:
                    {
                        return null;
                    }
            }
            //return null;
        }

    }
}
