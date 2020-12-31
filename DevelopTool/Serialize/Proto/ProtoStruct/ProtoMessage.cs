
using DevelopTool;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Proto
{
    [MenuItemPath("添加/结构")]
    [System.Serializable]
    [Editor]
    public class ProtoMessage : BaseTreeNotifyObject
    {
        [Export("%Name%")]
        public string ExportName
        {
            get
            {
                return Name;
            }
        }

        [Export("%Item~", "~Item%")]
        [Priority(3)]
        [ListView, MinHeight(200)]
        public ObservableCollection<ProtoExpression> Expres
        {
            get
            {
                if (expres == null) expres = new ObservableCollection<ProtoExpression>();
                foreach (var it in expres) { if (it.parent == null) it.parent = this; }
                return expres;
            }
            set { expres = value; Update("Expres"); }
        }
        public ObservableCollection<ProtoExpression> expres;

        [Export("%Print~","~Print%")]
        [Priority(2)]
        [CheckBox("打印"), MinWidth(100)]
        public bool Print
        {
            get { return UserSetting.Data["ProtoMessage_" + Name]; }
            set
            {
                UserSetting.Data["ProtoMessage_" + Name] = value;
                Update("Print");
            }
            //get
            //{
            //    return mPrint;
            //}
            //set
            //{
            //    mPrint = value;
            //    Update("Print");
            //}
        }

        [Export("%Comment%")]
        [Priority(1)]
        [TextBox("注释"), MinWidth(100)]
        public string Comment { get { return mComment; } set { mComment = value; Update("Comment"); } }
        string mComment;

        /// <summary>
        /// 扩展
        /// </summary>
        [Export("%Extensions%")]
        public int Extensions { get { return mExtensions; } set { mExtensions = value; Update("Extensions"); } }
        public int mExtensions;

        /// <summary>
        /// 表达式的可编辑类型
        /// </summary>
        public List<string> EditTypes
        {
            get
            {
                //合并导入类型和自定义类型
                var it = Parent;
                while ((it is ProtoFile) == false)
                {
                    it = it.Parent;
                }
                var proto = it as ProtoFile;
                var h = proto.ImportTypeList.Concat(proto.TypeList).ToList();

                ///合并默认类型
                var c = ProtoModel.instance.setting.baseTypeList.Concat(h).ToList();
                //排除自己
                c.Remove(Name);
                return c;
            }
        }
    }
}
