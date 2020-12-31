using System;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// 对象成员代理类<para/>
    /// 可对成员赋值和获取值
    /// </summary>
    public class MemberProxy
    {
        object obj;
        string memberName;
        bool convertType;
        public string MemberName { get => memberName; }
        MemberInfo mi;
        private MemberProxy()
        {
        }

        Func<object> GetValue;
        Action<object> SetValue;
        public object Value
        {
            get
            {
                if(GetValue==null)
                {
                    if (obj is ISetGetMemberValue)
                    {
                        var setObj = (ISetGetMemberValue)obj;
                        GetValue = () => setObj[memberName];
                    }
                    else
                    {
                        GetValue = ()=> mi.GetValue(obj);
                    }
                }
                return GetValue();
            }
            set
            {
                if(SetValue==null)
                {
                    if (obj is ISetGetMemberValue)
                    {
                        var setObj = (ISetGetMemberValue)obj;
                        if (convertType && setObj[memberName] != null)
                        {
                            SetValue = (v) =>
                            {
                                setObj[memberName] = ConvertUtil.ConvertType(v, setObj[memberName].GetType());
                            };

                        }
                        else
                        {
                            SetValue = (v) =>
                            {
                                setObj[memberName] = v;
                            };
                        }
                    }
                    else
                    {
                        if (convertType)
                        {
                            SetValue = (v) =>
                            {
                                mi.SetValue(obj, ConvertUtil.ConvertType(value, mi.GetMemberType()));
                            };
                        }
                        else
                        {
                            //if (mi.MemberType == MemberTypes.Property)
                            //{
                            //    SetObjPropertyValue = Delegate.CreateDelegate(
                            //        typeof(Action<,>).MakeGenericType(obj.GetType(), typeof(T)),
                            //        ((PropertyInfo)mi).GetSetMethod());
                            //}
                            //else
                            {
                                SetValue = (v) =>
                                {
                                    mi.SetValue(obj, v);
                                };
                            }
                        }
                    }
                }
                SetValue(value);
            }
        }

        public static MemberProxy GetMemberProxy(object obj,string memberName,bool convertType=false, BindingFlags flag= BindingFlags.Public | BindingFlags.NonPublic| BindingFlags.Instance)
        {
            if (obj == null) return null;
            var memberInfo = AssemblyUtil.GetMemberInfo( obj.GetType(),memberName, flag);

            if(memberInfo==null)
            {
                return null;
            }

            return new MemberProxy() { obj = obj, memberName = memberName, mi = memberInfo, convertType= convertType };
        }
    }
}
