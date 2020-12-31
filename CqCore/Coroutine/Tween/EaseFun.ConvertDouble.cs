using CqCore;
using System;

namespace System
{
    public static partial class EaseFun
    {
        /// <summary>
        /// 基于0~1的缓动转缓出
        /// </summary>
        public static Func<double, double> ConvertToOut(this System.Func<double,double> fun)
        {
            return x => 1 - fun(1 - x);
        }

        /// <summary>
        /// 基于0~1的缓动转缓入缓出
        /// </summary>
        public static Func<double, double> ConvertToInOut(this System.Func<double, double> fun)
        {
            return x =>
            {
                if(x<=0.5)
                {
                    return fun(x * 2) / 2;
                }
                else
                {
                    return 1 - fun(2*(1 - x)) / 2;
                }
            };
        }

        public static Func<double, double> GetEase(EaseFunEnum efe,EaseStyleEnum ese)
        {
            Func<double, double> fun = null;
            switch (efe)
            {
                case EaseFunEnum.Linear:
                    fun=LinearEase;
                    break;
                case EaseFunEnum.Quadratic:
                    fun = QuadraticEase;
                    break;
                case EaseFunEnum.Cubic:
                    fun = CubicEase;
                    break;
                case EaseFunEnum.Quartic:
                    fun = QuarticEase;
                    break;
                case EaseFunEnum.Quintic:
                    fun= QuinticEase;
                    break;
                case EaseFunEnum.Expo:
                    fun = ExpoEase;
                    break;
                case EaseFunEnum.Back:
                    fun= BackEase;
                    break;
                case EaseFunEnum.Sine:
                    fun= SineEase;
                    fun = fun.ConvertToOut();
                    break;
                case EaseFunEnum.Circle:
                    fun = CircleEase;
                    break;
                case EaseFunEnum.Elastic:
                    fun = ElasticEase;
                    break;
                case EaseFunEnum.Bounce:
                    fun = BounceEase;
                    fun = fun.ConvertToOut();
                    break;
            }
            if (fun == null) return null;
            switch (ese)
            {
                case EaseStyleEnum.EaseIn:
                    break;
                case EaseStyleEnum.EaseOut:
                    fun = fun.ConvertToOut();
                    break;
                case EaseStyleEnum.EaseInOut:
                    fun = fun.ConvertToInOut();
                    break;
            }
            return fun;
        }
    }
}
