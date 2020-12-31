using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityCore
{
    /// <summary>
    /// 点击
    /// </summary>
    public class ClickAttribute : ControlPropertyAttribute
    {
        /// <summary>
        /// 点击
        /// </summary>
        public ClickAttribute(string method) : base((object)method)
        {

        }
        public ClickAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {

        }
        protected override void OnInit(ControlAttribute ctl)
        {
            if(ctl is ButtonAttribute)
            {
                var btn = ctl as ButtonAttribute;
                btn.Click = () =>
                {
                    var methodName=  GetValue().ToString();

                    //UnityEngine.Debug.Log(methodName);
                    //UnityEngine.Debug.Log(Target);
                    if (AssemblyUtil.HasMethod(Target.GetType(), methodName))
                    {
                        AssemblyUtil.InvokeMethod(Target, GetValue().ToString());
                    }
                    else
                    {
                        var names= methodName.Split('.').ToList();
                        var method = names.Last();
                        names.RemoveAt(names.Count - 1);
                        
                        var info = (System.Func<object[], object>)AssemblyUtil.GetStaticMemberValue(AssemblyUtil.GetType(
                            string.Join(".", names.ToArray())), method);
                        info(new object[] { Target });
                    }
                };
            }
        }
    }
}

