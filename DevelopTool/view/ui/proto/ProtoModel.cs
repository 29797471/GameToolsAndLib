using CqCore;
using Proto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using WinCore;

namespace DevelopTool
{
    [Priority(10)]
    [Editor("proto编辑器", "/DevelopTool;component/Res/Images/Icons/google.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class ProtoModel : SingleModel<ProtoModel>
    {
        public override Setting Setting { get { return setting; } }
        public ProtoSetting setting { get { return SettingModel.instance.GetSetting<ProtoSetting>(); } }
        
        public ProtoModel()
        {
            mProtoList = new ProtoList<ProtoFile>();
            InitProtoFiles();

            //string str = FileOpr.ReadFile_UTF8(setting.SetPath);
            //var dic = Torsion.Deserialize<Dictionary<string, bool>>(str);
            //bool temp;
            //foreach (var it in mProtoList)
            //{
            //    foreach (var msg in it.Node.Children)
            //    {
            //        if (msg.nodeObj is ProtoMessage)
            //        {
            //            var pm = msg.nodeObj as ProtoMessage;
            //            if(dic.TryGetValue(pm.mName,out temp))
            //            {
            //                pm.Print = temp;
            //            }
            //        }
            //    }
            //}
        }

        [ListBox(), MinWidth(150), Height(450), SelectedValue("SelectItem")]
        [DoubleSelectedValue("OnDoubleClick"), DisplayMember("Name")]
        public ProtoList<ProtoFile> ProtoList
        {
            get { return mProtoList; }
            set { mProtoList = value; Update("ProtoList"); }
        }
        public ProtoList<ProtoFile> mProtoList;

        public void OnDoubleClick(ProtoFile obj)
        {
            ProtoList.OnOpen(obj);
        }

        public ProtoFile SelectItem
        {
            set
            {
                Root = value.Node;
            }
        }

        [Priority(1)]
        [TreeView("Proto"), SelectedValue("SelectObj"), Width(300), Height(550) ,Margin(30, 0)]
        public TreeNode Root
        {
            get { return mSelectTree; }
            set { mSelectTree = value; Update("Root"); }
        }
        TreeNode mSelectTree;

        [Priority(2)]
        [GroupBox(), MinWidth(500), Height(520), Margin(30, 0)]
        public BaseTreeNotifyObject SelectObj
        {
            get { return mSelectObj; }
            set { mSelectObj = value; Update("SelectObj"); }
        }
        BaseTreeNotifyObject mSelectObj;

        public void InitProtoFiles()
        {
            mProtoList.Clear();
            if (FileOpr.IsFolderPath(setting.ProtoFolder))
            {
                string[] _files = Directory.GetFiles(setting.ProtoFolder);
                foreach (string file in _files)
                {
                    if (file.Contains("~")) continue;
                    string extentsion = Path.GetExtension(file);

                    if (extentsion == ".proto")
                    {
                        var sr = File.OpenText(file);
                        try
                        {
                            var proto = ProtoSerialize.Deserialize(sr.ReadToEnd());
                            proto.file = file;
                            mProtoList.Add(proto);
                        }
                        catch (System.Exception)
                        {
                        }
                    }
                }
                Update("Title");
            }
        }
        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
            setting.TemplateFile.Make(this);

            setting.TemplateFile2.Make(this);
        }

        public override bool OnSave()
        {
            //var dic = new Dictionary<string, bool>();
            //foreach(var it in mProtoList)
            //{
            //    foreach(var msg in it.Node.Children)
            //    {
            //        if(msg.nodeObj is ProtoMessage)
            //        {
            //            var pm = msg.nodeObj as ProtoMessage;
            //            dic[pm.mName] = pm.Print;
            //        }
            //    }
            //}
            //FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(dic));
            return true;
            //foreach(var it in mProtoList)
            //{
            //    string data = ProtoSerialize.Serialize(it);
            //    FileOpr.SaveFile(it.file, data);
            //}
        }


        [Export("%Message~", "~Message%")]
        public List<ProtoMessage> OutProtoMessage
        {
            get
            {
                var list = new List<ProtoMessage>();
                Action<BaseTreeNotifyObject> OnTraversal = (x) =>
                {
                    if (x is ProtoMessage)
                    {
                        list.Add(x as ProtoMessage);
                    }
                };
                foreach (var proto in ProtoList)
                {
                    proto.PreOrderTraversal(OnTraversal);
                }
                return list;
            }
        }
    }
    /// <summary>
    /// 附带右键添加删除菜单的列表结构
    /// </summary>
    [Editor()]
    public class ProtoList<T> : ObservableCollection<T>
    {
        [Priority(1)]
        [MenuItem("打开文件")]
        public void OnOpen(ProtoFile obj)
        {
            if (obj != null)
            {
                FileOpr.RunByRelativePath(obj.file);
            }
        }

        [Priority(2)]
        [MenuItem("打开文件位置")]
        public void OnOpenFolder(ProtoFile obj)
        {
            if (obj != null)
            {
                ProcessUtil.OpenFileOrFolderByExplorer(obj.file);
            }
        }
        [Priority(3)]
        [MenuItem("开启所有打印")]
        public void OnOpenAllPrint(ProtoFile obj)
        {
            if (obj != null)
            {
                obj.PreOrderTraversal(x =>
                {
                    if (x is ProtoMessage)
                    {
                        (x as ProtoMessage).Print = true;
                    }
                });
            }
        }
        [Priority(4)]
        [MenuItem("关闭所有打印")]
        public void OnCloseAllPrint(ProtoFile obj)
        {
            if (obj != null)
            {
                obj.PreOrderTraversal(x =>
                {
                    if (x is ProtoMessage)
                    {
                        (x as ProtoMessage).Print = false;
                    }
                });
            }
        }
    }
}
