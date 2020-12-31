using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DebugInfo : MonoBehaviour 
{
    class Info
    {
        public float startTime;            //信息开始显示的时间
        public float showTime;             //信息需要显示的时间
        public string message;             //信息
    };
    public float updateInterval = 0.5F;
    private float lastInterval;
    private int frames = 0;
    private float fps;

    private List<Info> showList = new List<Info>();
    
    
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        //Debug.LogWarning("+++++++++++++My IpAddress Data:"+Network.player.ipAddress);
    }
    void OnGUI()
    {
        //GUILayout.BeginHorizontal();
        //GUILayout.EndHorizontal();
        GUILayout.Label("FPS:" + fps.ToString("f2"));
        GUILayout.Label("GameTime:" + Time.realtimeSinceStartup.ToString("f2"));
        //GUILayout.Label(" Application.dataPath:" + Application.dataPath);
        //GUILayout.Label(" Application.persistentDataPath:" + Application.persistentDataPath);
        //GUILayout.Label(" Application.temporaryCachePath:" + Application.temporaryCachePath);
        //GUILayout.Label(" Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        
        //GUILayout.Label("Score:" + GameCore.instance )
        for (int i = 0; i < showList.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("info:" + showList[i].message);
            GUILayout.EndHorizontal();
        }
    }
    // Update is called once per frame
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > (lastInterval + updateInterval))
        {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
        //检查需要删除的调试信息列表
        CheckOutTime();
    }
    void CheckOutTime()
    {
        bool bEase = false;
        for (int i = 0; i < showList.Count; ++i)
        {
            Info info = showList[i];
            if ((Time.time - info.startTime) > info.showTime)
            {
                bEase = true;
                showList.RemoveAt(i);
                break;
            }
        }
        if (bEase)
        {
            CheckOutTime();
        }
    }
    public static void Log(string message)
    {
        Debug.Log(message);
    }
    public void ShowInfo(string _message, float _showTime)
    {
        Info info = new Info
        {
            message = _message,
            showTime = _showTime,
            startTime = Time.time
        };
        showList.Add(info);
    }
}
