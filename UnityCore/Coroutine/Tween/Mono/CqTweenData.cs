using System;
using UnityCore;
using UnityEngine;

/// <summary>
/// 缓动数据定义基类
/// </summary>
[ExecuteInEditMode]
public class CqTweenData:MonoBehaviour
{
    [CheckBox("执行"),OnValueChanged("TestPlay")]
    public bool testPlaying;

    /// <summary>
    /// 缓动组
    /// </summary>
    [CqLabel("缓动组"),OnValueChanged("AddToGroup")]
    public CqTweenGroup group;

    /// <summary>
    /// 缓动描述
    /// </summary>
    [TextBox("缓动描述", true), ToolTip("对该缓动的解释"), Height(40)]
    public string desc;

    /// <summary>
    /// 缓动方式
    /// 0.起始 到 终止
    /// 1.终止 到 起始
    /// 2.当前 到 起始
    /// 3.当前 到 终止
    /// </summary>
    [ComBox("缓动方式", ComBoxStyle.RadioBox)]
    public TweenMode mode;
    /// <summary>
    /// 缓动方式
    /// 0.起始 到 终止
    /// 1.终止 到 起始
    /// 2.当前 到 起始
    /// 3.当前 到 终止
    /// </summary>
    public TweenMode Mode
    {
        set
        {
            mode = value;
        }
    }
    /// <summary>
    /// 缓动曲线
    /// </summary>
    [Curve("缓动曲线"),Height(100),ToolTip("到起始时曲线旋转180度,成为反向曲线{y=1-f(1-x)}")]
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// 缓动时间
    /// </summary>
    [TextBox("缓动时间"), ToolTip("完成一次缓动的时间")]
    public float duration = 1f;

    /// <summary>
    /// 起始延迟
    /// </summary>
    [TextBox("起始延迟"), ToolTip("执行播放后延迟一段时间然后再开始缓动")]
    public float startDelay = 0f;

    /// <summary>
    /// 间隔时间
    /// </summary>
    [TextBox("间隔时间"), ToolTip("当缓动多次时,每次缓动之间的停顿时间")]
    public float loopDelay=0f;

    /// <summary>
    /// 重复次数
    /// </summary>
    [Slider("重复次数", -1, 10), ToolTip("-1表示循环执行")]
    public int loopTimes=1;

    /// <summary>
    /// 往复运动
    /// 开启这个模式时当执行一次后曲线取反,运动模式切换
    /// (起始到终止 变成 终止到起始)
    /// (当前到终止 变成 当前到起始)
    /// </summary>
    [CheckBox("往复运动"), ToolTip("在缓动完成一次后,改变缓动方式(起始和终止作交换)")]
    public bool pingpong;

    [CheckBox("自定义到起始曲线")]
    public bool useBackCurve;

    /// <summary>
    /// 返回的缓动曲线
    /// </summary>
    [Curve("到起始的曲线"), Height(100),Visible("useBackCurve")]
    public AnimationCurve backCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// 缓动属性类型
    /// </summary>
    public System.Type TweenType
    {
        get
        {
            return AssemblyUtil.GetMemberValue(this, "mStart").GetType();
        }
    }

    /// <summary>
    /// 缓动属性
    /// </summary>
    [ComponentProperty("缓动属性"), ComponentFPTypeAttribute("TweenType")]
    public ComponentProperty comp;
}
