using CodeStyle;
using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinCore;

/// <summary>
/// 表达式代码模版
/// </summary>
[Editor("代码-表达式")]
public class ExprCodeTemplate : NotifyObject, IExpression
{
    public ExprCodeTemplate()
    {

    }
    [MenuItem("查看表达式对应的代码", "/WinCore;component/Res/find.ico")]
    public void ShowViewCode()
    {
        CustomMessageBox.Show(ExecContent);
    }
    EventExp ee;
    public EventExp Ee
    {
        set
        {
            ee = value;
            
            if (childs != null)
            {
                foreach (var it in childs)
                {
                    if (value != null) value.LinkTo(it);
                }
            }
        }
    }

    public override string ToString()
    {
        return Content;
    }

    /// <summary>
    /// 编程语言
    /// </summary>
    public string Language
    {
        get { if (mLanguage == null) mLanguage = CodeStyleNewModel.instance.setting.CodeSettingList[0].Name; return mLanguage; }
        set { mLanguage = value; }
    }
    public string mLanguage;

    CodeStyleLanguage CodeLanguage
    {
        get
        {
            return CodeStyleNewModel.instance.GetNodeList().ToList().Find(x => x.Name == Language);
        }
    }

    /// <summary>
    /// 模版返回类型
    /// </summary>
    public string StyleType
    {
        get {return mStyleType;}
        set{mStyleType = value;Update("StyleType");}
    }
    public string mStyleType;

    
    //public string mStyleName;

    /// <summary>
    /// 引用的代码模板
    /// </summary>
    public CodeStyleNode CodeTemplate
    {
        get
        {
            return CodeLanguage.ExpRoot[ExpChooses].nodeObj as CodeStyleNode;
        }
    }

    /// <summary>
    /// 子表达式
    /// </summary>
    public List<IExpression> childs;

    /// <summary>
    /// 显示内容 来源于代码模版的定义
    /// </summary>
    public string Content
    {
        get
        {
            string str=null;
            if (CodeTemplate != null)
            {
                str = CodeTemplate.Content;
            }
            if(str==null)return "Miss";
            if (childs != null)
            {
                for (var i = 0; i < childs.Count; i++)
                {
                    var itt = childs[i];
                    str = str.Replace("%" + itt.StyleType + i + "%", itt.Content);
                }
            }
            return str;
        }
    }

    /// <summary>
    /// 对应的代码
    /// </summary>
    public string ExecContent
    {
        get
        {
            string str = null;

            if (CodeTemplate != null)
            {
                str = CodeTemplate.ExecContent;
            }
            if (str == null) return "Miss";

            if (childs != null)
            {
                for(var i=0;i<childs.Count;i++)
                {
                    var itt = childs[i];
                    str=str.Replace("%" + itt.StyleType + i + "%" ,itt.ExecContent);
                }
            }
            return str;
        }
    }

    [Priority(1)]
    [SelectTreeNode("表达式"), SelectedValue("ExpChooses")]
    public TreeNode ExpRoot
    {
        get
        {
            if(mExpTree==null)
            {
                mExpTree = Torsion.Clone(CodeLanguage.ExpRoot);
                var removeList = new List<TreeNode>();
                mExpTree.PreorderTraversal(node =>
                {
                    if (node.IsLeaf())
                    {
                        var nodeObj = node.nodeObj as CodeStyleNode;
                        if (nodeObj != null && nodeObj.TypeName != StyleType)
                        {
                            removeList.Add(node);
                        }
                    }
                });
                foreach (var it in removeList)
                {
                    it.DeleteToParent();
                }
            }
            
            return mExpTree;
        }
    }
    TreeNode mExpTree;

    public ObservableCollection<string> ExpChooses
    {
        get
        {
            //ChangeData();
            if (mExpChooses==null)
            {
                mExpChooses = ExpRoot.FindByPreorder(x => x.IsLeaf() && (x.nodeObj is CodeStyleNode) ).GetPath();
            }
            return mExpChooses;
        }
        set
        {
            //if (mExpChooses == value) return;
            mExpChooses = value;
            Update("ExpChooses");
            var node = CodeLanguage.ExpRoot[mExpChooses].nodeObj as CodeStyleNode;
            /// 由代码模版生成表达式参数列表
            childs = new List<IExpression>();
            var ary = Content.Split('%');

            for (int k = 0; k < 6; k++)
            {
                foreach (var it in node.CodeSet.LinkTypeList)
                {
                    if (ary.Contains(it.Key + k))
                    {
                        childs.Add(CodeStyleNewModel.instance.DefaultValue(it.Key, Language));
                    }
                }
            }
            Update("RcList");
        }
    }
    public ObservableCollection<string> mExpChooses;

    /// <summary>
    /// 链接编辑变量的内容表现形式
    /// </summary>
    [VerticalAlignment(System.Windows.VerticalAlignment.Center)]
    [Priority(2)]
    [RichText("内容"),Width(300)]
    public ObservableCollection<RichTextUCItem> RcList
    {
        get
        {
            var mRcList = new ObservableCollection<RichTextUCItem>();
            if (CodeTemplate == null) return null;
            var str = CodeTemplate.Content;//.Replace("\r\n", "%\r\n%");
            var strList = str.Split(new string[]{"%" }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach(var it in strList)
            {
                if(it!="")
                {
                    RichTextUCItem rt = new RichTextUCItem() { mText = it };
                    for (var i = 0; i < childs.Count; i++)
                    {
                        var itt = childs[i];
                        if (it == itt.StyleType + i)
                        {
                            rt = new RichTextUCItem()
                            {
                                mText = itt.Content,
                                CallBack = () =>
                                {
                                    var editor = EditorCodeTemplate.ToEditor(itt, ee);
                                    //editor.Init();
                                    editor = WinUtil.OpenEditorWindow(editor);
                                    if (editor != null)
                                    {
                                        childs[i] = editor.GetValue();
                                        Update("RcList");
                                    }
                                }
                            };
                            break;
                        }
                    }
                    mRcList.Add(rt);
                }
            }
            return mRcList;
        }
    }


    //void ChangeData()
    //{
    //    if (!once)
    //    {
    //        once = true;
    //        if (mStyleName != null)
    //        {
    //            var v = CodeLanguage.ExpRoot.FindByPreorder(x => x.IsLeaf() && x.Name == mStyleName &&
    //            (x.nodeObj as CodeStyleNode).TypeName == StyleType);
    //            if (v != null)
    //            {
    //                mExpChooses = v.GetPath();
    //            }
                
    //            mStyleName = null;
    //        }
    //    }
    //}
    //private bool once;
}