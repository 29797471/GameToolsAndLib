using CqCore;
using System.Collections.Generic;

namespace UnityCore
{
    /// <summary>
    /// 内存快照消息体
    /// </summary>
    public class ProfilerMsg
    {
        public static string[] types = new string[]
        {
            "UnityEngine.Texture2D",
            "UnityEngine.Shader",
            "UnityEngine.RenderTexture",
            "UnityEngine.ParticleSystem",
            "UnityEngine.MeshCollider",
            "UnityEngine.Mesh",
            "UnityEngine.Material",
            "UnityEngine.Font",
            "UnityEngine.AnimationClip",
            "UnityEngine.AudioClip",
        };
        
        public TreeNode<MemoryDataNode> head;

        #region Memory
        public int systemMemorySize;
        public int graphicsMemorySize;

        public long TotalUnusedReservedMemory;
        public long TotalReservedMemory;
        public long TotalAllocatedMemory;
        public long MonoHeapSize;
        public long MonoUsedSize;
        public long usedHeapSize;
        #endregion

        #region Time
        public float realtimeSinceStartup;
        public int renderedFrameCount;
        public int frameCount;
        public float timeScale;
        public float maximumParticleDeltaTime;
        public float smoothDeltaTime;
        public float maximumDeltaTime;
        public int captureFramerate;
        public float fixedDeltaTime;
        public float unscaledDeltaTime;
        public float fixedUnscaledTime;
        public float unscaledTime;
        public float fixedTime;
        public float deltaTime;
        public float timeSinceLevelLoad;
        public float time;
        public float fixedUnscaledDeltaTime;
        public bool inFixedTimeStep;
        #endregion
    }

    /// <summary>
    /// 每种类型统计数据
    /// </summary>
    public class ProfilerType
    {
        public string typeName;
        public long memorySize;
        public List<ProfilerItem> items;
    }
    /// <summary>
    /// 每资源对象统计数据
    /// </summary>
    public class ProfilerItem
    {
        public string itemName;
        public int refCount;
        public long memorySize;
    }
}
