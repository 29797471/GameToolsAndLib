using UnityEngine;

namespace UnityEditor
{
    public static class SceneViewUtil
    {
        /// <summary>
        /// 获取Scene视图鼠标点击发出的射线
        /// </summary>
        /// <returns></returns>
        public static Ray GetClickRay()
        {
            var size = SceneView.currentDrawingSceneView.position.size;
            var point = Event.current.mousePosition.Division(size);
            point.y = 1 - point.y;
            return SceneView.currentDrawingSceneView.camera.ViewportPointToRay(point);
        }
    }
}
