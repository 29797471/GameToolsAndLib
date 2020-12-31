using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace System.Windows.Controls
{
    public static class DependencyObjectUtil
    {
        /// <summary>
        /// 设置属性
        /// </summary>
        public static void SetValueX(this FrameworkElement obj, DependencyProperty dp, object value)
        {
            var a = obj.GetBindingExpression(dp);
            if (a == null)
            {
                obj.SetValue(dp, value);
            }
            else
            {
                var objType = a.DataItem.GetType();
                var list = objType.GetMembers();
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].Name == a.ParentBinding.Path.Path)
                    {
                        (list[i] as PropertyInfo).SetValue(a.DataItem, value, null);
                        break;
                    }
                }
            }
        }
    }
}
