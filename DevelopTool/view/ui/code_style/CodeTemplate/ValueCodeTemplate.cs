using CodeStyle;
using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinCore;
/// <summary>
/// 编辑的值
/// 值有两种情况：1.有定义的模板名字 2.模板名字为空,自定义的值
/// </summary>
[Editor("代码-值")]
public class ValueCodeTemplate : NotifyObject, IExpression
{
    [MenuItem("查看值对应的代码", "/WinCore;component/Res/find.ico")]
    public void ShowViewCode()
    {
        CustomMessageBox.Show(ExecContent);
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
        get { return mLanguage; }
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
    /// 模版类型
    /// </summary>
    public string StyleType
    {
        get { return mStyleType; }
        set { mStyleType = value; Update("StyleType"); }
    }
    public string mStyleType;

    /// <summary>
    /// 值: 关联的代码模版名称
    /// </summary>
    //public string mStyleName;

    /// <summary>
    /// 引用的代码模板
    /// </summary>
    public CodeStyleNode CodeTemplate
    {
        get
        {
            return CodeLanguage.ValueRoot[ValueChooses].nodeObj as CodeStyleNode;
        }
    }

    public string Content
    {
        get
        {
            return (!customValue) ? CodeTemplate.Content : mData;
        }
    }

    public string ExecContent
    {
        get
        {
            return (!customValue) ? CodeTemplate.ExecContent : mData;
        }
    }


    [Priority(2)]
    [Visibility("CustomValue", AttributeTarget.Parent, "x=0")]
    [SelectTreeNode("值"), SelectedValue("ValueChooses")]
    public TreeNode ValueRoot
    {
        get
        {
            if (mValueRoot == null)
            {
                mValueRoot = Torsion.Clone(CodeLanguage.ValueRoot);
                var removeList = new List<TreeNode>();
                mValueRoot.PreorderTraversal(node =>
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

            return mValueRoot;
        }
    }
    TreeNode mValueRoot;
    public ObservableCollection<string> ValueChooses
    {
        get
        {
            if (mValueChooses == null)
            {
                mValueChooses = ValueRoot.FindByPreorder(x => x.IsLeaf()).GetPath();
            }
            return mValueChooses;
        }
        set
        {
            mValueChooses = value;
            var node = ValueRoot[mValueChooses];
            Update("ValueChooses");
        }
    }

    public ObservableCollection<string> mValueChooses;
    
    public bool customValue=false;

    public string mData;
    
}