using DevelopTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeStyle
{
    /// <summary>
    /// 代码模板
    /// </summary>
    [MenuItemPath("添加/代码模板(N)", null, System.Windows.Input.Key.N)]
    [Editor("代码模板")]
    public class CodeStyleNode : BaseTreeNotifyObject, ICustomSerialize
    {
        
        public string TypeName
        {
            get { if (mTypeName == null) mTypeName = CodeSet.LinkTypeList[0].Key; return mTypeName; }
            set { mTypeName = value; Update("TypeName"); }
        }
        public string mTypeName;

        public CodeStyleLanguage Language
        {
            get
            {
                if (mLanguage == null) mLanguage = CodeStyleNewModel.instance.GetNodeList().ToList().Find(x => x.ValueRoot == Node.Root);
                if (mLanguage == null) mLanguage = CodeStyleNewModel.instance.GetNodeList().ToList().Find(x => x.ExpRoot == Node.Root);
                return mLanguage;
            }
        }
        CodeStyleLanguage mLanguage;

        public CodeSetting CodeSet
        {
            get
            {
                if (mCodeSet == null)
                    mCodeSet = CodeStyleNewModel.instance.setting.CodeSettingList.ToList().Find(x => x.Name == Language.Name);
                return mCodeSet;
            }
        }
        CodeSetting mCodeSet;

        [Priority(1)]
        [ComboBox("结果类型", 100),  SelectedValue("TypeName"), MinWidth(150)]
        public object TypeNameList
        {
            get
            {
                return CodeSet.LinkTypeList.ToList().ConvertAll(x => x.Key);
            }
        }

        /// <summary>
        /// 由变量(%[type][number]% )和文本组成的显示内容
        /// </summary>
        [Priority(2)]
        [TextBox("显示内容",100),AcceptsReturn(true), AcceptsTab(true),MinHeight(50),MinWidth(350) ]
        public string Content { get { return mContent; } set { mContent = value; Update("Content"); } }
        public string mContent;

        /// <summary>
        /// 由变量(%[type][number]% )和文本组成的转译代码
        /// </summary>
        [Priority(3)]
        [TextBox("对应代码", 100), AcceptsReturn(true), AcceptsTab(true), MinHeight(50), MinWidth(350)]
        public string ExecContent { get { return mExecContent; } set { mExecContent = value; Update("ExecContent"); } }
        public string mExecContent;


        /// <summary>
        /// 获取一个由代码模版生成的表达式
        /// </summary>
        public ExprCodeTemplate NewExpr
        {
            get
            {
                var expr = new ExprCodeTemplate() { StyleType = TypeName, Language = Language.Name, ExpChooses = Node.GetPath() };

                expr.childs = new List<IExpression>();
                var ary = Content.Split('%');

                for (int k = 0; k < 6; k++)
                {
                    foreach (var it in CodeSet.LinkTypeList)
                    {
                        if (ary.Contains(it.Key + k))
                        {
                            expr.childs.Add(CodeStyleNewModel.instance.DefaultValue(it.Key, Language.Name));
                        }
                    }
                }
                return expr;
            }
        }


        /// <summary>
        /// 获取一个由代码模版生成的值
        /// </summary>
        public ValueCodeTemplate NewValue
        {
            get
            {
                return new ValueCodeTemplate()
                {
                    StyleType = TypeName,
                    Language = Language.Name,
                    ValueChooses = Node.GetPath()
                };
            }
        }

        public string InjectData(string content)
        {
            content = content.Replace("%Name%", Name);
            content = content.Replace("%Content%", Content);
            content = content.Replace("%ExecContent%", ExecContent);
            return content;
        }

    }
}

