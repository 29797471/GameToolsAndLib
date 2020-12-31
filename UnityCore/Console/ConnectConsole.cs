using CqCore;
using P2P;
using System;
using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// 连接到控制台服务器,承载两个功能
/// 1.接收命令执行代码
/// 2.发送日志信息
/// </summary>
public class ConnectConsole: MonoBehaviourExtended
{
    [ComBox("状态", ComBoxStyle.RadioBox), IsEnabled(false)]
    public NetState state;

    [TextBox("P2P服务器端口")]
    public int port=7777;

    [TextBox("P2P服务器IP")]
    public string ip= ConsoleConfig.defaultConsoleIP;

    [CheckBox("保持连接"),ToolTip("当断开后自动重连"),OnValueChanged("KeepConnect")]
    public bool mKeepConnect;
    public bool KeepConnect
    {
        get
        {
            return mKeepConnect;
        }
        set
        {
            mKeepConnect = value;
            if(Application.isPlaying)
            {
                if(mKeepConnect && !client.Connected )
                {
                    Connect();
                }
                else if(!mKeepConnect && client.Connected)
                {
                    client.Close();
                }
            }
        }
    }

    P2PClient client;
    List<int> sendList;

    /// <summary>
    /// 当作内存快照时,获取游戏当前状态名称
    /// </summary>
    [TextBox("游戏状态")]
    public string gameState="None";

    void Awake()
    {
        AssemblyUtil.RegType(typeof(OrderSend),typeof(LogMsgGet),typeof(ProfilerMsg));

        sendList = new List<int>();
        if(Application.isMobilePlatform)
        {
            client = new P2PClient(string.Format("{0}({1}:{2})", SystemInfo.deviceName, SystemInfo.deviceModel,NetUtil.LocalIP),"game");
        }
        else
        {
            client = new P2PClient(SystemInfo.deviceName,"game");
        }
        
        client.OnDisConnect += OnDisConnect;
        Application.logMessageReceivedThreaded += Application_logMessageReceived;
        Action<int, object> _Client_OnReceiveMsg = (srcId, msg) => GlobalCoroutine.Call(() => Client_OnReceiveMsg(srcId,msg));
        client.OnReceiveMsg += _Client_OnReceiveMsg;
        DestroyHandle.CancelAct += ()=>
        {
            client.OnDisConnect -= OnDisConnect;
            client.Close();
            client.OnReceiveMsg -= _Client_OnReceiveMsg;
            Application.logMessageReceivedThreaded -= Application_logMessageReceived;
            client.Clear();
        };
        if (KeepConnect)
        {
            Connect();
        }
    }
    #region 接收消息
    void Client_OnReceiveMsg(int srcId, object msg)
    {
        if(msg is OrderSend)
        {
            var data = msg as OrderSend;
            switch (data.opr)
            {
                case 0:
                    {
                        if (sendList.Contains(srcId))
                        {
                            sendList.Remove(srcId);
                            //Debug.Log("断开命令端" + srcId);
                        }
                    }
                    break;
                case 1:
                    {
                        if (!sendList.Contains(srcId))
                        {
                            sendList.Add(srcId);
                            //Debug.Log("连接命令端" + srcId);
                        }
                    }
                    break;
                case 2:
                    {
                        if (DoCommand != null)
                        {
                            DoCommand(data.command);
                        }
                    }
                    break;
                case 3:
                    {
                        var x = new OrderBack()
                        {
                            opr = 3,
                            data = Torsion.Serialize(CqProfiler.MakeProfilerMsg()),
                            state = gameState,
                        };
                        
                        client.SendMsg(srcId, x);
                        Debug.Log("上传内存快照");
                    }
                    break;
                case 4:
                    {
                        var x = new OrderBack()
                        {
                            opr = 4,
                            data = Torsion.Serialize(CqProfiler.SaveHierarchyData()),
                            state = gameState,
                        };
                        client.SendMsg(srcId, x);
                        Debug.Log("上传Hierarchy");
                    }
                    break;
            }
        }
    }
    #endregion


    void OnDisConnect()
    {
        state = NetState.Notconnect;
    }
    public System.Func<string,object> DoCommand;

    public void Connect()
    {
        state = NetState.Connecting;
        client.Connect(ip, port,bl=>
        {
            state = bl ? NetState.Connected : NetState.ConnectFailed;
        });
    }
    void Application_logMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
    {
        try
        {
            var x = new LogMsgGet()
            {
                condition = condition,
                stackTrace = stackTrace,
                type = (CqCore.LogType)type
            };
            foreach (var dstId in sendList)
            {
                client.SendMsg(dstId, x);
            }
        }
        catch (Exception e)
        {
            CqDebug.Log(e,CqCore.LogType.Exception);
        }
    }

    
    void OnApplicationPause(bool isPause)
    {
        if (isPause)  //离开程序进入到后台状态
        {
            
        }
        else //进入程序状态更改为前台
        {      

        }
        if (KeepConnect!=client.Connected)
        {
            KeepConnect = client.Connected;
        }
    }
}


