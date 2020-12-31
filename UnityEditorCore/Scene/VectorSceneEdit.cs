using CqCore;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCore
{
    public class VectorSceneEdit : Singleton<VectorSceneEdit>
    {
        Causality<MonoBehaviour, VectorEditAttribute> causality;

        [InitializeOnLoadMethod]
        static void Init()
        {
            instance.causality = new Causality<MonoBehaviour, VectorEditAttribute>(
                   b =>
                   {
                       var list = AssemblyUtil.GetMemberAttributesInObject<VectorEditAttribute>(b);
                       return list.Count > 0 ? list[0] : null;
                   }
                   );
            SceneView.onSceneGUIDelegate += instance.OnSceneGUICall;
        }
        void ReSetColor<T>(Action<T> Draw, T t)
        {
            var Handlescolor = Handles.color;
            var GUIcolor = GUI.color;
            Draw(t);
            Handles.color = Handlescolor;
            GUI.color = GUIcolor;
        }
        void ReSetColor(Action Draw)
        {
            var Handlescolor = Handles.color;
            var GUIcolor = GUI.color;
            Draw();
            Handles.color = Handlescolor;
            GUI.color = GUIcolor;
        }

        private void OnSceneGUICall(SceneView sceneView)
        {
            var selects = GizmoMgr.instance.Selects;

            foreach (var it in selects)
            {
                var att = causality.Call(it);

                if (att != null && att.Mono != null && att.Mono.enabled && att.Mono.gameObject.activeInHierarchy)
                {
                    ReSetColor(SceneEdit, att);
                    break;
                }
            }
            
        }
        void SceneEdit(VectorEditAttribute att)
        {
            var tran = att.Mono.transform;
            if (att.Target is Vector2)
            {
                var p = tran.localToWorldMatrix.MultiplyPoint((Vector2)att.Target);
                if (att.Label != null)
                {
                    if (att.Color != null) GUI.color = (Color)att.Color;
                    Handles.Label(p, att.Label);
                }
                var p1 = Handles.DoPositionHandle(p, Quaternion.identity);
                if (p1 != p)
                {
                    att.Target = (Vector2)tran.worldToLocalMatrix.MultiplyPoint(p1);
                }
            }
            else if(att.Target is Vector3)
            {
                var p = tran.localToWorldMatrix.MultiplyPoint((Vector3)att.Target);
                if (att.Label != null)
                {
                    if (att.Color != null) GUI.color = (Color)att.Color;
                    Handles.Label(p, att.Label);
                }
                var p1 = Handles.DoPositionHandle(p, Quaternion.identity);
                if (p1 != p)
                {
                    att.Target = tran.worldToLocalMatrix.MultiplyPoint(p1);
                }
            }
        }
    }
}
