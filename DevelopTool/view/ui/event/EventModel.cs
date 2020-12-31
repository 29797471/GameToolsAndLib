using CqCore;
using CqEvent;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 编辑数据结构面板关联的数据模块
/// </summary>
namespace DevelopTool
{
    [Priority(0)]
    [Editor("事件编辑器", "/DevelopTool;component/Res/Images/Icons/event.ico",Orientation.Horizontal)]
    public class EventModel : SingleModel<EventModel>
    {
        
        public override Setting Setting { get { return setting; } }
        public EventSetting setting { get { return SettingModel.instance.GetSetting<EventSetting>(); } }

        
        [Priority(0)]
        [Image,Width(20)]
        public string FindIcon
        {
            get
            {
                return "/WinCore;component/Res/find.ico";
            }
        }

        [Priority(0, 0,1)]
        //[Icon("/WinCore;component/Res/find.ico",20f)]
        [TextBox(), ToolTip("检索名称或者事件id"), MinWidth(170)]
        public string Seach
        {
            get { return mSeach; }
            set { mSeach = value; Update("Seach"); Update("FilterItem"); }
        }
        string mSeach;

        public Predicate<object> FilterItem
        {
            get
            {
                if (string.IsNullOrEmpty(Seach)) return null;
                return o => o.ToString().ToLower().Contains(Seach.ToLower()) ||
                ((o as TreeNode).nodeObj is EventNode && ((o as TreeNode).nodeObj as EventNode).EventId.ToLower().Contains(Seach.ToLower()));
            }
        }

        [Export("%Start_Export%", "%End_Export%")]
        [TreeView("CqEvent"), Priority(0,1),Width(300),Height(520),SelectedValue("SelectObj"), Filter("FilterItem")]
        public TreeNode Root
        {
            get { return mRoot; }
            set { mRoot = value; Update("Root"); }
        }
        TreeNode mRoot;

        BaseTreeNotifyObject mSelectObj;

        [GroupBox(), Width(800), Height(570), Margin(30, 0), Priority(1)]
        public BaseTreeNotifyObject SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }

        protected override void ReLoad()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            if(str==null)
            {
                Root = new TreeNode();
            }
            else
            {
                Root = Torsion.Deserialize<TreeNode>(str);
            }
        }
        public override bool OnSave()
        {
            if (!CheckName()) return false;
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(Root));
            return true;
        }
        /// <summary>
        /// 事件重名检查,或者未定义名称
        /// </summary>
        bool CheckName()
        {
            Dictionary<string, EventNode> dic = new Dictionary<string, EventNode>();

            var xx=Root.FindByPreorder(x =>
            {
                if (x.nodeObj is EventNode)
                {
                    var node = (x.nodeObj as EventNode);
                    if (node.EventId == null)
                    {
                        MessageBox.Show(string.Format("事件({0})未定义名称", node.Path));
                        return true;
                    }
                    if(dic.ContainsKey(node.EventId))
                    {
                        MessageBox.Show(string.Format("两事件({0},{1})名称({2})相同", dic[node.EventId].Path,node.Path, node.EventId));
                        return true;
                    }
                    else
                    {
                        dic[node.EventId] = node;
                        return false;
                    }
                }
                return false;
            });
            return xx == null;
        }

        public void CanncelPrint()
        {
            Root.PreorderTraversal(x =>
            {
                if(x.nodeObj is EventNode)
                {
                    (x.nodeObj as EventNode).Print = false;
                }
            });
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            if (CheckName())
            {
                foreach (var makefile in setting.TemplateFileList)
                {
                    makefile.Make(this);
                    yield return null;
                }
            }
            
        }
        public override void OnHide()
        {
            SelectObj = null;
        }
    }
}

