using CqCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 修饰Mono脚本,在GUI中绘制类成员
    /// </summary>
    public class DrawGUIAttribute: ObjectAttribute
    {
        public void Draw()
        {
            var attrs = AssemblyUtil.GetMemberAttributesInObject<ControlAttribute>(Target);
            foreach (var attr in attrs)
            {
                GUILayout.BeginHorizontal();
                attr.Draw();
                GUILayout.EndHorizontal();
            }
        }
    }
}
