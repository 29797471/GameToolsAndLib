//using System;
//using System.Text;
//using UnityEngine;

//namespace UnityCore
//{
//    /// <summary>
//    /// 资源策略
//    /// 1.使用一个相对路径去访问资源
//    /// a.文本文件返回string
//    /// b.assetbundle返回object
//    /// c.图片返回Texture2D
//    /// 2.保证资源不重复加载（a.如果已经加载完成则直接拿缓存 b.如果正在加载则添加加载完成的回调 c.开始加载）
//    /// 3.加载时先去本地找,如果没有或者本地文件比较旧时就从网络下载下到本地,下载后更新文件版本,再从本地加载到内存
//    /// </summary>
//    public static class ResMgr
//    {
//        public static Action LoadFileAsync(string relativePath, Action<byte[]> OnLoad)
//        {
//            return FileVersionMgr.instance.LoadFileAsync(relativePath, OnLoad);
//        }
//        public static Action LoadAsync(string relativePath, Action<string> OnLoad)
//        {
//            return LoadFileAsync(relativePath, bytes=> OnLoad(Encoding.UTF8.GetString(bytes)));
//        }
//        public static Action LoadAsync(string relativePath, Action<UnityEngine.Object[]> OnLoad)
//        {
//            return LoadFileAsync(relativePath, bytes =>
//            {
//                var asset = AssetBundle.LoadFromMemory(bytes);
//                OnLoad(asset.LoadAllAssets());
//            });
//        }
//    }
//}

