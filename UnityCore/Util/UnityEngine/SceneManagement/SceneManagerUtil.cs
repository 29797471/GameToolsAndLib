using System.Collections.Generic;

namespace UnityEngine.SceneManagement
{
    public static class SceneManagerUtil
    {
        /// <summary>
        /// 获取所有场景(在运行时会包含DontDestroyOnLoad)
        /// </summary>
        public static List<Scene> GetAllScenes()
        {
            var scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }
            if (Application.isPlaying)
            {
                var go = new GameObject();
                GameObject.DontDestroyOnLoad(go);
                if (go.scene.rootCount > 1)
                {
                    var scene = go.scene;
                    GameObject.DestroyImmediate(go);
                    scenes.Add(scene);
                }
            }
            return scenes;
        }
    }
}
