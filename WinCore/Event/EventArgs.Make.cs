using CqCore;
using System;
using System.Collections.Generic;

 
/// <summary>弹框消息</summary>
[CqEvent("弹框消息",false)]
[System.Serializable]
public class MsgBoxEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 内容
	/// </summary>
	public readonly string content;
 
	/// <summary>
	/// 标题
	/// </summary>
	public readonly string title;
 
	/// <summary>
	/// 弹框样式
	/// </summary>
	public readonly int style;

	/// <summary>弹框消息</summary>
	public MsgBoxEventArgs( string content ,  string title ,  int style   )
	{
		this.content=content;
		this.title=title;
		this.style=style;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }

} 
/// <summary>气泡消息</summary>
[CqEvent("气泡消息",false)]
[System.Serializable]
public class MsgBalloonEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 内容
	/// </summary>
	public readonly string content;
 
	/// <summary>
	/// 标题
	/// </summary>
	public readonly string title;
 
	/// <summary>
	/// 图标<para>0.无</para><para>1.信息</para><para>2.警告</para><para>3.错误</para>
	/// </summary>
	public readonly int icon;
 
	/// <summary>
	/// 持续时间
	/// </summary>
	public readonly float duration;

	/// <summary>气泡消息</summary>
	public MsgBalloonEventArgs( string content ,  string title ,  int icon ,  float duration   )
	{
		this.content=content;
		this.title=title;
		this.icon=icon;
		this.duration=duration;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 
/// <summary>打印消息</summary>
[CqEvent("打印消息",false)]
[System.Serializable]
public class MsgPrintEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 内容
	/// </summary>
	public readonly string content;
 
	/// <summary>
	/// 持续时间
	/// </summary>
	public readonly float duration;

	/// <summary>打印消息</summary>
	public MsgPrintEventArgs( string content ,  float duration   )
	{
		this.content=content;
		this.duration=duration;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 
/// <summary>批处理</summary>
[CqEvent("批处理",false)]
[System.Serializable]
public class CmdCommandEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 命令
	/// </summary>
	public readonly string command;

	/// <summary>批处理</summary>
	public CmdCommandEventArgs( string command   )
	{
		this.command=command;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 

/// <summary>Android交互</summary>
[CqEvent("Android交互",true)]
[System.Serializable]
public class AndroidClickEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 按键<para>0.Escape</para><para>1.Home</para>
	/// </summary>
	public readonly int style;

	/// <summary>Android交互</summary>
	public AndroidClickEventArgs( int style   )
	{
		this.style=style;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 
/// <summary>窗口显示</summary>
[CqEvent("窗口显示",true)]
[System.Serializable]
public class WindowShowEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 窗口名称
	/// </summary>
	public readonly string winName;

	/// <summary>窗口显示</summary>
	public WindowShowEventArgs( string winName   )
	{
		this.winName=winName;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 
/// <summary>窗口隐藏</summary>
[CqEvent("窗口隐藏",true)]
[System.Serializable]
public class WindowHideEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 窗口名称
	/// </summary>
	public readonly string winName;

	/// <summary>窗口隐藏</summary>
	public WindowHideEventArgs( string winName   )
	{
		this.winName=winName;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
} 
/// <summary>窗口销毁</summary>
[CqEvent("窗口销毁",true)]
[System.Serializable]
public class WindowDestroyEventArgs:CustomEventArgs 
{
 
	/// <summary>
	/// 窗口名称
	/// </summary>
	public readonly string winName;

	/// <summary>窗口销毁</summary>
	public WindowDestroyEventArgs( string winName   )
	{
		this.winName=winName;
	}
    public override bool Notify(object sender = null)
    {
        throw new NotImplementedException();
    }
}
