using CqCore;
using MVL;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCore
{

    /// <summary>
    /// 缓动一个对象的成员,这个成员是可以作插值计算的类型
    /// </summary>
    public class CqTweenMember : MonoBehaviourExtended
    {

        [CheckBox("执行"), OnValueChanged("OnDonePlayingChanged")]
        public bool donePlaying;

        /*
        /// <summary>
        /// 缓动组
        /// </summary>
        [CqLabel("缓动组"), OnValueChanged("AddToGroup")]
        public CqTweenGroup group;
        */

        /// <summary>
        /// 缓动描述
        /// </summary>
        [TextBox("描述", true), ToolTip("对该缓动的解释"), Height(40)]
        public string desc;

        /// <summary>
        /// 缓动方式
        /// 0.起始 到 终止
        /// 1.当前 到 终止
        /// 2.当前 到 起始
        /// 3.终止 到 起始
        /// </summary>
        [ComBox("缓动方式", ComBoxStyle.RadioBox)]
        public TweenMemberMode mode;

        /// <summary>
        /// 缓动曲线
        /// </summary>
        [Curve("缓动曲线"), Height(100), ToolTip("到起始时曲线旋转180度,成为反向曲线{y=1-f(1-x)}")]
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// 缓动时间
        /// </summary>
        [TextBox("一次时间(s)"), ToolTip("完成一次缓动的时间")]
        public float duration = 1f;

        /// <summary>
        /// 起始延迟
        /// </summary>
        [TextBox("起始延迟(s)"), ToolTip("执行播放后延迟一段时间然后再开始缓动")]
        public float startDelay = 0f;

        /// <summary>
        /// 间隔时间
        /// </summary>
        [TextBox("间隔时间(s)"), ToolTip("当缓动多次时,每次缓动之间的停顿时间")]
        public float loopDelay = 0f;

        /// <summary>
        /// 重复次数
        /// </summary>
        [Slider("重复次数", -1, 10), ToolTip("-1表示循环执行")]
        public int loopTimes = 1;


        /// <summary>
        /// 往复运动
        /// 开启这个模式时当执行一次后曲线取反,运动模式切换
        /// (起始到终止 变成 终止到起始)
        /// (当前到终止 变成 当前到起始)
        /// </summary>
        [CheckBox("往复运动"), ToolTip("在缓动完成一次后,改变缓动方式(起始和终止作交换)")]
        public bool pingpong;


        [ComBox("成员类型", ComBoxStyle.RadioBox), OnValueChanged("OnTypeChanged")]
        public BindingTweenType bindingTweenType;

        void OnTypeChanged()
        {
            startTorsionValue = null;
            endTorsionValue = null;
            attr = null;
            comp = null;
            mTweenHandle = null;
        }
        EnumLabelAttribute attr;

        public Type GetTypeByStyle(int index)
        {
            if (attr == null)
            {
                attr = EnumUtil.GetEnumAttr<EnumLabelAttribute>(bindingTweenType);
            }
            return attr.types[index];
        }

        public Type TweenType
        {
            get
            {
                return GetTypeByStyle(0);
            }
        }

        /// <summary>
        /// 缓动属性
        /// </summary>
        [ComponentProperty("缓动属性"), ComponentFPType("TweenType"),OnValueChanged("OnComponentPropertyChanged")]
        public ComponentProperty comp;

        void OnComponentPropertyChanged()
        {
            mTweenHandle = null;
            if(comp!=null)
            {
                startValue = comp.Value;
                endValue = comp.Value;
            }
        }

        //[HideInInspector]
        //[SerializeField]
        [ContextMenuItem("=当前值", "SetStart")]
        [TorsionValue("起始"), ComponentMember("bindingTweenType")]
        public string startTorsionValue;

        public object startValue
        {
            set
            {
                startTorsionValue = Torsion.Serialize(value);
            }
            get
            {
                return Torsion.Deserialize(startTorsionValue, TweenType);
            }
        }

        public void SetStart()
        {
            if (comp != null)
            {
                startValue = comp.Value;
            }
        }

        //[HideInInspector]
        //[SerializeField]
        [ContextMenuItem("=当前值", "SetEnd")]
        [TorsionValue("终止"), ComponentMember("bindingTweenType")]
        public string endTorsionValue;
        public object endValue
        {
            set
            {
                endTorsionValue = Torsion.Serialize(value);
            }
            get
            {
                return Torsion.Deserialize(endTorsionValue, TweenType);
            }
        }
        public void SetEnd()
        {
            if (comp != null)
            {
                endValue = comp.Value;
            }
        }

        [Header("完成后回调")]
        public UnityEvent OnComplete;

        CqCoroutine cc;

        CqTweenLerp mTweenHandle;
        /// <summary>
        /// 单次缓动句柄
        /// </summary>
        CqTweenLerp TweenHandle
        {
            get
            {
                if (mTweenHandle == null)
                {
                    mTweenHandle = (CqTweenLerp)AssemblyUtil.CreateInstance(GetTypeByStyle(1));
                    mTweenHandle.Evaluate = curve.Evaluate;
                    mTweenHandle.memberProxy = comp.MemProxy;
                }
                return mTweenHandle;
            }
        }

        void Start()
        {
            OnDonePlayingChanged();
        }
        public void Play()
        {
            Cancel();

            TweenHandle.StartValue = startValue;
            TweenHandle.EndValue = endValue;
            cc = StartCoroutine(Play_IT());
        }

        IEnumerator Play_IT()
        {
            var times = loopTimes;
            yield return GlobalCoroutine.Sleep(startDelay);
            while (true)
            {
                switch (mode)
                {
                    case TweenMemberMode.ToEnd:
                        {
                            yield return TweenHandle.PlayTo_IT(duration, 1);
                            break;
                        }
                    case TweenMemberMode.StartToEnd:
                        {
                            yield return TweenHandle.Play_IT(duration, 0, 1);
                            break;
                        }
                    case TweenMemberMode.EndToStart:
                        {
                            yield return TweenHandle.Play_IT(duration, 1, 0);
                            break;
                        }
                    case TweenMemberMode.ToStart:
                        {
                            yield return TweenHandle.PlayTo_IT(duration, 0);
                            break;
                        }
                }

                times--;
                if (times == 0) break;
                yield return GlobalCoroutine.Sleep(loopDelay);
                if (pingpong)
                {
                    mode = 3 - mode;
                }
            }
            donePlaying = false;
            OnComplete.Invoke();
        }

        public void Cancel()
        {
            if (cc != null)
            {
                cc.Stop();
                cc = null;
            }
        }
        public void OnDonePlayingChanged()
        {
            if (donePlaying)
            {
                Play();
            }
            else
            {
                Cancel();
            }
        }
    }
}
