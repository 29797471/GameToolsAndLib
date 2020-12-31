using CqCore;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 全局类,在此发送全局Update和协程
/// </summary>
public class GlobalMono : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGlobalCoroutine()
    {
        //GlobalCoroutine.StopAllCoroutines();
    }
    const string GLOBAL_MONO = "GlobalMono";
    private static bool _applicationIsQuitting = false;
    void Awake()
    {
        //Debug.Log("GlobalMono Awake");
        mInst = this;

        GameObject.DontDestroyOnLoad(gameObject);

        DefineGetCurrentToWaitFor();
    }

    /// <summary>
    /// 定义对协程返回值的等待方式
    /// </summary>
    public static void DefineGetCurrentToWaitFor()
    {
        var waitForSecondsInfo = AssemblyUtil.GetMemberInfo<WaitForSeconds>("m_Seconds");
        GlobalCoroutine.Define_GetCurrentToWaitFor(current =>
        {
            if (current is WaitForSeconds)
            {
                return GlobalCoroutine.Sleep((float)waitForSecondsInfo.GetValue(current));
            }
            else if (current is AsyncOperation)
            {
                return _StartAsyncOperation((AsyncOperation)current);
            }
            else if (current is WWW)
            {
                return _StartWWW((WWW)current);
            }
            else if (current is int)
            {
                return GlobalCoroutine.Sleep((int)current);
            }
            else if (current is float)
            {
                return GlobalCoroutine.Sleep((float)current);
            }
            else
            {
                throw new Exception("未定义的协程返回类型" + current);
            }
        });
    }

    static IEnumerator _StartWWW(WWW obj)
    {
        while (!obj.isDone)
        {
            yield return null;
        }
    }
    static IEnumerator _StartAsyncOperation(AsyncOperation obj)
    {
        while (!obj.isDone)
        {
            yield return null;
        }
    }

    static GlobalMono mInst;
    public static GlobalMono Inst
    {
        get
        {
            if (_applicationIsQuitting)
            {
                return null;
            }
            if (mInst == null)
            {
                var obj = GameObject.Find(GLOBAL_MONO);
                if (obj == null)
                {
                    obj = new GameObject(GLOBAL_MONO);
                }
                mInst = obj.GetComponent<GlobalMono>();
                if (mInst == null)
                {
                    mInst = obj.AddComponent<GlobalMono>();
                }
            }
            return mInst;
        }
    }
    void OnApplicationQuit()
    {
        DestroyImmediate(gameObject);
    }
    void Update()
    {
        //CqDebug.BeginSample("GlobalCoroutineUpdate");
        //GlobalCoroutine.Update(GlobalCoroutine.GetTickTime(Time.deltaTime));

        GlobalCoroutine.Update(GlobalCoroutine.GetSpanTick(Time.time));
        //CqDebug.EndSample();
        //CqDebug.BeginSample("OnUpdate");
        if (OnUpdate != null) OnUpdate();
        //CqDebug.EndSample();
    }
    void FixedUpdate()
    {
        if (OnFixedUpdate != null) OnFixedUpdate();
    }
    /// <summary>
    /// 更新
    /// </summary>
    public event Action OnUpdate;

    /// <summary>
    /// 物理更新
    /// </summary>
    public event Action OnFixedUpdate;

    /// <summary>
    /// 前后台切换通知
    /// </summary>
    public event Action<bool> ApplicationPause;


    void OnDrawGizmos()
    {
        if (DrawGizmos != null) DrawGizmos();
    }

    /// <summary>
    /// 绘制
    /// </summary>
    public event Action DrawGizmos;

    void OnApplicationPause(bool pause)
    {
        if (ApplicationPause != null) ApplicationPause(pause);
    }

    void OnDestroy()
    {
        GlobalCoroutine.StopAllCoroutines();
        ApplicationPause = null;
        OnUpdate = null;
        OnFixedUpdate = null;
        mInst = null;
        _applicationIsQuitting = true;
    }
}

