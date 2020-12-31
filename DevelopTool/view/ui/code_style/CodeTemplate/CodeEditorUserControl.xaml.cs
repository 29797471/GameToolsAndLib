using System;
using System.Windows;
using System.Windows.Controls;
using WinCore;

namespace DevelopTool
{
    /// <summary>
    /// FunExpUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CodeEditorUserControl : UserControl
    {

        public static DependencyProperty CodeTemplateProperty = DependencyProperty.Register("CodeTemplate", 
            typeof(IExpression), typeof(CodeEditorUserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CodeTemplatePropertyChangedCallback)));
        private static void CodeTemplatePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            var com = sender as CodeEditorUserControl;
            com.UpdateViewData_CodeTemplate();
        }
        void UpdateViewData_CodeTemplate()
        {
            if (CodeTemplate != null) ctl.Target = CodeTemplate.Content;
            else ctl.Target = "{null}";
        }
        public IExpression CodeTemplate
        {
            get
            {
                return ((IExpression)(GetValue(CodeTemplateProperty)));
            }
            set
            {
                var a = GetBindingExpression(CodeTemplateProperty);
                if (a == null)
                {
                    SetValue(CodeTemplateProperty, value);
                }
                else
                {
                    AssemblyUtil.SetMemberValue(a.DataItem, a.ParentBinding.Path.Path, value);
                }
                UpdateViewData_CodeTemplate();
            }
        }

        public CodeEditorUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 右键查看代码
        /// </summary>
        private void button_Click_ShowCode(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show(CodeTemplate.ExecContent);
        }

        /// <summary>
        /// 编辑生成代码
        /// </summary>
        private void UnderLineTextUserControl_Click(object sender, EventArgs e)
        {
            var editor = EditorCodeTemplate.ToEditor(CodeTemplate, Tag as EventExp);
            //editor.Init();
            editor = WinUtil.OpenEditorWindow(editor);
            if (editor != null )
            {
                if( CodeTemplate.StyleType != editor.StyleType)
                {
                    EventMgr.MsgPrint.Notify("返回类型不同,操作失败", 5);
                }
                else CodeTemplate = editor.GetValue();
            };
        }
    }
}
