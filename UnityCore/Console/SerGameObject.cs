using CqCore;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 一个对应于GameObject的自定义序列化对象类
    /// </summary>
    public class SerGameObject : ITreeDataNode<SerGameObject>
    {
        
        #region GameObject需要序列化的相关数据
        public bool activeSelf;
        public int layer;
        public Matrix4x4? localToWorldMatrix;
        public string mName;

        public Dictionary<string, string> monoDic;
        #endregion


        public override string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public static implicit operator SerGameObject(GameObject obj)
        {
            var sObj = new TreeNode<SerGameObject>();
            var data = new SerGameObject();
            data.activeSelf = obj.activeSelf;
            data.layer = obj.layer;
            data.monoDic = new Dictionary<string, string>();
            var monos=obj.GetComponents<Component>();
            //var monos = obj.GetComponents<Behaviour>();
            foreach (var it in monos)
            {
                var type = it.GetType();
                if(it is Transform)
                {
                    continue;
                }
                else if(it is MonoBehaviour)
                {
                    SerializeTypeUtil.RegType(type,type.Name, SerializeTypeStyle.Field);
                }
                else
                {
                    SerializeTypeUtil.RegType(type, type.Name, SerializeTypeStyle.Property);
                }
                var format = new SerializeFormat(SerializeFormatStyle.Torsion, false, false, true);
                data.monoDic[type.Name] = CqSerialize.Serialize(it, format);
                //try
                //{
                //    data.monoDic[type.Name] = Torsion.Serialize(it, true, false, false, 1);
                //}
                //catch (System.Exception e)
                //{
                //    Debug.LogError(it.PathInHierarchy());
                //    Debug.LogError(type);
                //    Debug.LogException(e);
                //}
            }
            data.localToWorldMatrix = obj.transform.localToWorldMatrix;
            data.mName = obj.name;
            sObj.Data = data;

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var childObj= obj.transform.GetChild(i).gameObject;
                if (MathUtil.StateCheck(childObj.hideFlags, HideFlags.HideInHierarchy)) continue;
                var child = (SerGameObject)childObj;
                sObj.AddChildren(child.Node);
            }
            return data;
        }
        public static implicit operator GameObject(SerGameObject sObj)
        {
            var obj = new GameObject();
            obj.name = sObj.Name;
            obj.SetActive(sObj.activeSelf);
            obj.layer = sObj.layer;
            if (sObj.localToWorldMatrix != null)
            {
                obj.transform.SetWorldMatrix((Matrix4x4)sObj.localToWorldMatrix);
            }
            
            if(sObj.monoDic!=null)
            {
                foreach (var it in sObj.monoDic)
                {
                    var type = AssemblyUtil.GetType(it.Key);
                    if(type!=null)
                    {
                        var com=obj.AddComponent(type);
                    }
                    else
                    {
                        var mr = obj.AddComponent<MonoReflection>();
                        mr.scriptName = it.Key;
                        mr.scriptData = it.Value;
                    }
                }
            }

            if (sObj.Node.mChildren != null)
            {
                foreach (var it in sObj.Node.mChildren)
                {
                    var itObj = (GameObject)it.Data;
                    itObj.transform.SetParent(obj.transform);
                }
            }
            return obj;
        }
    }
}
