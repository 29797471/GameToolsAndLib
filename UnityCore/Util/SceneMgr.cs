using UnityEngine;
using System.Collections;
using System;
using CqCore;

namespace UnityCore
{
    public class SceneMgr
    {
        #region Instance Define
        static SceneMgr inst = null;
        static public SceneMgr Instance
        {
            get
            {
                if (inst == null)
                {
                    inst = new SceneMgr();
                }
                return inst;
            }
        }
        #endregion
        //进度更新
        public Action<float> OnProgress;



        public float progress
        {
            get
            {
                return (asyn != null) ? asyn.progress : 0;
            }
        }
        Camera _camera;
        public Camera camera//mainCamera
        {
            get
            {
                if (_camera == null)
                {
                    if (Camera.main != null)
                    {
                        _camera = Camera.main;
                    }
                    else
                    {
                        var cam = GameObject.FindGameObjectWithTag("MainCamera");
                        if (cam != null)
                        {
                            _camera = cam.GetComponent<Camera>();
                        }
                    }
                }
                return _camera;
            }
        }


        private AsyncOperation asyn = null;   //异步加载地图时返回的加载数据
        private string _current;//当前场景

        public string current
        {
            get
            {
                return _current;
            }
        }
        private string _prev;//上一场景
        private string _next;//下一场景

        private IEnumerator BaseLoadScene(string next)
        {
            if (asyn == null || asyn.allowSceneActivation == true)
            {
                //1.完成后开始加载下一个场景
                asyn = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(next.ToString());
                //2.这里设置为当下一个场景加载完毕后不会进行跳转(unity4.x新增的API)
                asyn.allowSceneActivation = false;
                //3.定义循环等待异步操作完成
                #region 修改 2015.06.08 hans
                //说明：当asyn.allowSceneActivation = false时，asyn.isDone!=true;还有又要GameObject创建的时候占用的主线程，所以当资源加载的时候分，Update，协程 都是会卡顿的
                //while (!asyn.isDone && asyn.progress < 0.9f)
                //{
                //    if (OnProgress != null) OnProgress(asyn.progress);
                //    yield return new WaitForSeconds(0.1f);
                //}
                bool isOut = false;
                float progressTmp = 0;
                
                while (!asyn.isDone && !isOut)
                {
                    if (OnProgress != null)//判断是否有进度条
                    {
                        yield return GlobalCoroutine.Sleep(0.01f);
                        progressTmp += Time.deltaTime;//每次 进度加多少
                        if (progressTmp < asyn.progress / 0.9f)
                        {
                        }
                        else
                        {
                            progressTmp = asyn.progress / 0.9f;
                        }
                        OnProgress(progressTmp);
                        if (progressTmp >= 1f)
                        {
                            isOut = true;
                        }
                    }
                    else
                    {
                        //无进度条直接跳转
                        isOut = true;
                    }
                }
                #endregion

                //4.等待完成，设置allowSceneActivation为true，开始跳转
                asyn.allowSceneActivation = true;

                _prev = _current;
                _current = next;
                if (_current != "loading") DebugInfo.Log("Scene(" + _current + ")Loaded");
            }

        }
        public void LoadScene(string next, bool loading = false)
        {
            if (loading)
            {
                _next = next;
                GlobalCoroutine.Start(BaseLoadScene("loading"));
            }
            else
            {
                GlobalCoroutine.Start(BaseLoadScene(next));
            }
        }
        //loading界面加载完成，开始加载场景并显示进度
        public void OnLoadingNextScene()
        {
            GlobalCoroutine.Start(BaseLoadScene(_next));
        }

    }
}

