using CqCore;
using P2P;
using System;
using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;


public class CommandClientWindow : EditorWindow
{
    public CommandClientWindow()
    {
        mInst = this;
    }
    static CommandClientWindow mInst;
    public static CommandClientWindow Inst
    {
        get
        {
            return mInst;
        }
    }
    const string winName = "命令端窗口";
    //[MenuItem("Assets/" + winName)]
    [MenuItem("Window/" + winName)]
    static void Open()
    {
        GetWindow<CommandClientWindow>(winName);
    }

    [PreferenceItem("命令端配置")]
    static void PreferencesGUI()
    {
        //EditorGUILayout.LabelField("SVN Settings", EditorStyles.boldLabel);
        var data = ConsoleConfig.Inst;
        var ip = EditorGUILayout.TextField("IP", data.ip);
        if (ip != data.ip)
        {
            data.ip = ip;
            ConsoleConfig.Inst = data;
        }
        var port = EditorGUILayout.IntField("端口", data.port);
        if (port != data.port)
        {
            data.port = port;
            ConsoleConfig.Inst = data;
        }
        var hierarchyNameTblCount = EditorGUILayout.IntField("Hierarchy制表符数量", data.hierarchyNameTblCount);
        if (hierarchyNameTblCount != data.hierarchyNameTblCount)
        {
            data.hierarchyNameTblCount = hierarchyNameTblCount;
            ConsoleConfig.Inst = data;
        }
    }
    public class GameClientX
    {
        public int id;
        public ClientInfo info;
    }
    

    GUISkin skin;
    P2PClient client;

    /// <summary>
    /// 跟踪日志的目标游戏端
    /// </summary>
    GameClientX dstClient;
    GameClientX DstClient
    {
        get
        {
            return dstClient;
        }
        set
        {
            if (dstClient != value)
            {
                if (dstClient != null)
                {
                    client.SendMsg(dstClient.id, new OrderSend() { opr = 0 });
                    Debug.Log("断开:" + dstClient.info);
                }
                dstClient = value;
                if (dstClient != null)
                {
                    client.SendMsg(dstClient.id, new OrderSend() { opr = 1 });
                    Debug.Log("连接:" + dstClient.info);
                }

                //for (UnityEngine.LogType i = 0; i <= UnityEngine.LogType.Exception; i++)
                //{
                //    PlayerSettings.SetStackTraceLogType(i, dstClient == null ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
                //}
            }
        }
    }

    List<GameClientX> gameList = new List<GameClientX>();
    

    void OnGUI()
    {
        var data = ConsoleConfig.Inst;
        if (skin == null)
        {
            skin = GUI.skin;
        }
        BeginWindows();
        if (client == null)
        {
            GUILayout.Label("未连接控制台服务器");
        }
        else if(connectFailed)
        {
            GUILayout.Label("连接失败" + data.ip + ":" + data.port);
        }
        else if (client.Connected)
        {
            GUILayout.Label(string.Format("命令端:{0}(已连接{1}:{2})", SystemInfo.deviceName, data.ip, data.port));
        }
        else
        {
            GUILayout.Label("正在连接" + data.ip + ":" + data.port);
        }
        
        bool refesh = false;
        GUILayoutUtil.Vertical(() =>
        {
            if (gameList != null)
            {
                scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.ExpandHeight(true), GUILayout.Height(200));
                foreach (var game in gameList)
                {
                    var it = game;
                    var bl = (it == dstClient);
                    GUILayoutUtil.Vertical(() =>
                    {
                        GUILayout.Label(it.info.clientName);
                        GUILayoutUtil.Horizontal(() =>
                        {
                            if (GUILayout.Button(new GUIContent("对象层级快照", "Hierarchy"), GUILayout.Width(100)))
                            {
                                client.SendMsg(it.id, new OrderSend() { opr = 4 });
                            }
                            if (GUILayout.Button(new GUIContent("内存快照", "Memory")))
                            {
                                client.SendMsg(it.id, new OrderSend() { opr = 3 });
                            }
                            var _bl = GUILayout.Toggle(bl, "日志");
                            if (_bl != bl)
                            {
                                if (_bl)
                                {
                                    DstClient = it;
                                }
                                else
                                {
                                    DstClient = null;
                                }
                            }
                        });
                    });
                }
                EditorGUILayout.EndScrollView();
                if (DstClient == null) return;
                GUILayoutUtil.Horizontal(() =>
                {
                    EditorGUILayout.LabelField("命令(发送:shift+Enter,换行:Enter)");

                    
                    {
                        if (GUILayout.Button("代码模版"))
                        {
                            CodeTemplateWindow.Inst.Open((str) =>
                            {
                                if (!str.IsNullOrEmpty())
                                {
                                    InputText = str;
                                }
                            });
                        }
                    }
                });
                
                {
                    var temp = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));
                    //var temp = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));
                    if (temp != inputText)
                    {
                        inputText = temp;
                    }
                    if (Event.current.isKey && Event.current.type == EventType.KeyUp)
                    {
                        if (Event.current.keyCode == KeyCode.UpArrow)
                        {
                            if (historyCommandIndex > 0) historyCommandIndex--;
                            InputText = historyCommand[historyCommandIndex];
                        }
                        else if (Event.current.keyCode == KeyCode.DownArrow)
                        {
                            if (historyCommandIndex < historyCommand.Count - 1) historyCommandIndex++;
                            InputText = historyCommand[historyCommandIndex];
                        }
                        if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter && Event.current.modifiers == EventModifiers.Shift)
                        {
                            if (!inputText.IsNullOrEmpty())
                            {
                                DoCommand(inputText);
                                inputText = null;
                            }
                        }
                    }
                }

                //DoInput();
            }

        }, "", skin.box);


        EndWindows();

        if (refesh)
        {
            EditorGUIUtility.editingTextField = false;
            Repaint();
        }
    }
    List<string> historyCommand = new List<string>();
    int historyCommandIndex = 0;
    public void DoCommand(string command)
    {
        if (command.IsNullOrEmpty()) return;

        if (client != null && client.Connected && DstClient != null)
        {
            client.SendMsg(DstClient.id, new OrderSend() { opr = 2, command = command });
        }
    }

    Vector2 scroll;
    string inputText;
    string InputText
    {
        set
        {
            inputText = value;
            EditorGUIUtility.editingTextField = false;
            Repaint();
        }
    }
    /// <summary>
    /// 当窗口获得焦点时调用一次
    /// </summary>
    void OnFocus()
    {
    }
    private void OnEnable()
    {
        var data = ConsoleConfig.Inst;
        //Debug.Log("OnEnable", this);
        AssemblyUtil.RegType(typeof(OrderSend), typeof(LogMsgGet), typeof(ProfilerMsg));
        client = new P2PClient(SystemInfo.deviceName, "command");
        client.OnReceiveMsg += Client_OnReceiveMsg;
        client.Connect(data.ip, data.port, bl =>
        {
            if (bl)
            {
                client.OnP2PListChange += () =>
                {
                    {
                        gameList.Clear();
                        if (client != null)
                        {
                            foreach (var it in client.dic)
                            {
                                if (it.Value.groupName=="game")
                                {
                                    gameList.Add(new GameClientX()
                                    {
                                        id = it.Key,
                                        info = it.Value,
                                    });
                                }
                            }
                        }
                    };
                };
            }
            else
            {
                connectFailed = true;
            }
        });
    }
    bool connectFailed;
    private void OnDisable()
    {
        DstClient = null;
        //Debug.Log("OnDisable", this);
        if (client != null)
        {
            client.Close();
            client.OnReceiveMsg -= Client_OnReceiveMsg;
            client.Clear();
            client = null;
        }
    }
    private void Client_OnReceiveMsg(int srcId, object msgData)
    {
        GlobalCoroutine.Call(() =>
        {
            if (msgData is OrderBack)
            {
                var data = msgData as OrderBack;
                var clientName = gameList.Find(x => x.id == srcId).info.clientName;
                switch (data.opr)
                {
                    case 3:
                        {
                            var msg = Torsion.Deserialize<ProfilerMsg>(data.data);
                            var obj = new GameObject(string.Format("{0}({1} {2})", data.state, clientName,TimeUtil.GetTimeStringByDate(DateTime.Now)));
                            var mono=obj.AddComponent<ProfilerMsgMono>();
                            mono.msg = msg;
                            mono.data = data.data;
                            mono.time = DateTime.Now.Ticks;
                            mono.MakeMsgTree();
                            break;
                        }
                    case 4:
                        {
                            //FileOpr.SaveFile_UTF8("temp.txt", data.data);
                            var zz = Torsion.Deserialize<TreeNode<SerGameObject>>(data.data);
                            var obj = new GameObject(string.Format("{0}({1} {2})", data.state, clientName, TimeUtil.GetTimeStringByDate(DateTime.Now)));
                            //生成Hierarchy;
                            var child = (GameObject)zz.Data;
                            child.transform.SetParent(obj.transform);
                            Selection.activeObject = obj;
                            break;
                        }

                }
            }
            else if (msgData is LogMsgGet)
            {
                var data = msgData as LogMsgGet;
                //防止本地打印,本地客户端上传,导致死循环打印
                if (!Application.isPlaying)
                {
                    for (UnityEngine.LogType i = 0; i <= UnityEngine.LogType.Exception; i++)
                    {
                        PlayerSettings.SetStackTraceLogType(i,  StackTraceLogType.None);
                    }
                    Debug.unityLogger.Log((UnityEngine.LogType)data.type, data.condition + "\n" + data.stackTrace);
                    for (UnityEngine.LogType i = 0; i <= UnityEngine.LogType.Exception; i++)
                    {
                        PlayerSettings.SetStackTraceLogType(i, StackTraceLogType.ScriptOnly);
                    }
                }
            }
        });
    }
    /// <summary>
    /// 当窗口丢失焦点时调用一次
    /// </summary>
    void OnLostFocus()
    {
    }
    /// <summary>
    /// 当Hierarchy视图中的任何对象发生改变时调用一次
    /// </summary>
    void OnHierarchyChange()
    {
    }
    /// <summary>
    /// 当Project视图中的资源发生改变时调用一次
    /// </summary>
    void OnProjectChange()
    {
    }
    /// <summary>
    /// 窗口面板的更新
    /// 这里开启窗口的重绘，不然窗口信息不会刷新
    /// </summary>
    void OnInspectorUpdate()
    {
        Repaint();
    }
    /// <summary>
    /// 当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
    /// 有可能是多选，
    /// </summary>
    void OnSelectionChange()
    {
        //这里开启一个循环打印选中游戏对象的名称
        //foreach (Transform t in Selection.transforms)
        //{
        //    //Debug.Log("OnSelectionChange" + t.name);
        //}
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
    }
}

