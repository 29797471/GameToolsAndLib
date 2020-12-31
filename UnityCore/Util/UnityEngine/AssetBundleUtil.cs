using CqCore;
using System;
using System.Collections;

namespace UnityEngine
{
    public static class AssetBundleUtil
    {
        public static void LoadFromMemoryAsync(byte[] bytes, Action<AssetBundle> OnComplete, ICancelHandle handle =null)
        {
            GlobalCoroutine.Start(LoadFromMemoryCoroutine(bytes, OnComplete), handle);
        }
        static IEnumerator LoadFromMemoryCoroutine(byte[] bytes, Action<AssetBundle> OnComplete)
        {
            var abcr = AssetBundle.LoadFromMemoryAsync(bytes);
            yield return abcr;
            OnComplete(abcr.assetBundle);
        }
    }
}
