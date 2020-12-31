using CqCore;

namespace UnityEngine
{
    public static class CameraUtil
    {
        
        /// <summary>
        /// 将p点按区间移动在相机像素范围内
        /// </summary>
        public static Vector2 IntoPixelRange(this Camera camera, Vector2 p)
        {
            p.x = MathUtil.MoveToRange(p.x, 0, camera.pixelWidth);
            p.y = MathUtil.MoveToRange(p.y, 0, camera.pixelHeight);
            return p;
        }

        /// <summary>
        /// 摄像机视锥和包围盒是否相交或者包含
        /// </summary>
        public static bool Intersects(this Camera camera,Bounds bounds)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        /// <summary>
        /// 点在摄像机视锥内
        /// </summary>
        public static bool Contains(this Camera camera, Vector3 pos)
        {
            var p = camera.WorldToScreenPoint(pos);
            return p.x >= 0 && p.x <= Screen.width && p.y >= 0 && p.y <= Screen.height;
        }

        /// <summary>
        /// 点在摄像机视锥内
        /// </summary>
        public static bool ContainsByViewport(this Camera cam, Vector3 worldPos)
        {
            Transform camTransform = cam.transform;
            Vector2 viewPos = cam.WorldToViewportPoint(worldPos);
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);     //判断物体是否在相机前面

            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 由屏幕上的一个点,沿摄像机观看方向发出射线和一个平面求交点
        /// </summary>
        public static Vector3? ScreenToWorldPoint(this Camera camera,Vector3 mousePosition, Plane plane)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);
            float f;
            if (plane.Raycast(ray, out f))
            {
                return ray.GetPoint(f);
            }
            return null;
        }
        public static readonly Plane yPlane = new Plane(Vector3.up, Vector3.zero);
        /// <summary>
        /// 鼠标沿摄像机发出射线和y=0平面的交点
        /// </summary>
        public static Vector3? MouseRayCrossYPlane(this Camera camera)
        {
            return ScreenToWorldPoint(camera,Input.mousePosition,yPlane);
        }
    }
}
