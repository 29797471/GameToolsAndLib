using CqCore;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace UnityCore
{
    /// <summary>
    /// 性能测试API
    /// </summary>
    public static class CqProfiler
    {
        /// <summary>
        /// 抓取内存数据
        /// </summary>
        public static ProfilerMsg MakeProfilerMsg()
        {
            var data = new ProfilerMsg();

            data.systemMemorySize = SystemInfo.systemMemorySize;
            data.graphicsMemorySize = SystemInfo.graphicsMemorySize;

            #region Memory
            data.TotalUnusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong();
            data.TotalReservedMemory = Profiler.GetTotalReservedMemoryLong();
            data.TotalAllocatedMemory = Profiler.GetTotalAllocatedMemoryLong();
            data.MonoHeapSize = Profiler.GetMonoHeapSizeLong();
            data.MonoUsedSize = Profiler.GetMonoUsedSizeLong();
            data.usedHeapSize = Profiler.usedHeapSizeLong;
            #endregion

            #region Time
            data.realtimeSinceStartup = Time.realtimeSinceStartup;
            data.renderedFrameCount = Time.renderedFrameCount;
            data.frameCount = Time.frameCount;
            data.timeScale = Time.timeScale;
            data.maximumParticleDeltaTime = Time.maximumParticleDeltaTime;
            data.smoothDeltaTime = Time.smoothDeltaTime;
            data.maximumDeltaTime = Time.maximumDeltaTime;
            data.captureFramerate = Time.captureFramerate;
            data.fixedDeltaTime = Time.fixedDeltaTime;
            data.unscaledDeltaTime = Time.unscaledDeltaTime;
            data.fixedUnscaledTime = Time.fixedUnscaledTime;
            data.unscaledTime = Time.unscaledTime;
            data.fixedTime = Time.fixedTime;
            data.deltaTime = Time.deltaTime;
            data.timeSinceLevelLoad = Time.timeSinceLevelLoad;
            data.time = Time.time;
            data.fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            data.inFixedTimeStep = Time.inFixedTimeStep;
            #endregion
            
            var head = new TreeNode<MemoryDataNode>();
            data.head = head;
            head.Data = new MemoryDataNode()
            {
                Name = "Memory",
            }; 
            head.Data.instanceID = head.Data.Name.GetHashCode();
            var totalSize =0L;
            
            foreach (var it in ProfilerMsg.types)
            {
                var child= new TreeNode<MemoryDataNode>();
                //var objects = FindObjectsOfType(AssemblyUtil.GetType("UnityEngine." + it));
                var type = AssemblyUtil.GetType(it);
                 var objects = Resources.FindObjectsOfTypeAll(type);
                child.Data = new MemoryDataNode()
                {
                    mName = type.Name,
                };
                child.Data.instanceID = child.Data.Name.GetHashCode();
                foreach (var obj in objects)
                {
                    var childchild = new TreeNode<MemoryDataNode>();
                    var size = Profiler.GetRuntimeMemorySizeLong(obj);
                    childchild.Data = new MemoryDataNode()
                    {
                        mName = obj.name,
                        instanceID = obj.GetInstanceID(),
                        size = size,
                    };
                    child.Data.size += size;
                    child.AddChildren(childchild);
                }
                totalSize += child.Data.size;
                head.AddChildren(child);
            }
            head.Data.size = totalSize;
            //{
            //    var child = new TreeNode<MemoryDataNode>();
            //    child.Data = new MemoryDataNode()
            //    {
            //        mName = "Other",
            //        size = otherSize,
            //    };
            //    child.Data.instanceID = child.Data.Name.GetHashCode();
            //    head.AddChildren(child);
            //}
            return data;
        }
        
        /// <summary>
        /// 将当前Hierarchy的所有对象转可以序列化的数据对象
        /// </summary>
        /// <returns></returns>
        public static TreeNode<SerGameObject> SaveHierarchyData()
        {
            var sObj = new SerGameObject();
            sObj.Name = "Hierarchy";
            sObj.activeSelf = true;
            var xx = new TreeNode<SerGameObject>();
            xx.Data = sObj;

            var scenes = SceneManagerUtil.GetAllScenes();

            foreach (var scene in scenes)
            {
                var child = ConvertCurrentSceneObj(scene);
                xx.AddChildren(child);
            }
            return xx;
        }
        static TreeNode<SerGameObject> ConvertCurrentSceneObj(Scene scene)
        {
            var objs = scene.GetRootGameObjects();
            var sObj = new SerGameObject();
            sObj.Name = scene.name;
            sObj.activeSelf = true;
            var xx = new TreeNode<SerGameObject>();
            xx.Data = sObj;
            foreach (var obj in objs)
            {
                if (MathUtil.StateCheck(obj.hideFlags, HideFlags.HideInHierarchy)) continue;
                
                var child = (SerGameObject)obj;
                xx.AddChildren(child.Node);
            }
            return xx;
        }
    }
}
