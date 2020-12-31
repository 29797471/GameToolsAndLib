using CqCore;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinCore
{
    /// <summary>
    /// EditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditorWindow : Window
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(EditorWindow));
            //new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetPropertyChangedCallback)));

        public object Target
        { 
            get
            {
                return GetValue(TargetProperty);
            }
            set
            {
                this.SetValueX(TargetProperty, value);
                if(Target!=null)
                {
                    var titleAttr = AssemblyUtil.GetClassAttribute<EditorAttribute>(Target.GetType());
                    if (titleAttr != null && titleAttr.name != null) Title = titleAttr.name;
                    else Title = Target.GetType().Name;
                }
                var panel = WinUtil.DrawPanel(Target);
                //处理不必要的层级
                while(panel.Children.Count==1 )
                {
                    var it = panel.Children[0];
                    panel.Children.Remove(it);
                    (it as FrameworkElement).DataContext = Target;
                    if (it is Panel)
                    {
                        panel = (Panel)it;
                    }
                    else
                    {
                        cc.Content = it;
                        return;
                    }
                }
                
                cc.Content = panel;

            }
        }
        
        public EditorWindow()
        {
            InitializeComponent();
            var lastWindow = ObjectPoolX<EditorWindow>.Peek();
            if (lastWindow != null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Left = lastWindow.Left + 50;
                Top = lastWindow.Top + 50;
                Tag = lastWindow.Tag;
            }

            ObjectPoolX<EditorWindow>.Push(this);
            WinUtil.SetInputCommand(this, () => Paste(), Key.V, ModifierKeys.Control);
            WinUtil.SetInputCommand(this, () => Copy(), Key.C, ModifierKeys.Control);
            WinUtil.SetInputCommandX(this, () => Button_Cancel(null,null), Key.Escape);
            //LayoutUpdated += (obj, e) =>
            //{
            //    sv.ScrollToBottom();
            //};
        }

        private void Copy()
        {
            ClipboardUtil.CopyObject(Target);
        }

        private void Paste()
        {
            object clone = ClipboardUtil.PasteObject(Target.GetType());
            if (clone != null)
            {
                Target = clone;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ObjectPoolX<EditorWindow>.Pop();
        }
        /// <summary>
        /// 是否点击在按钮上(否则点击在x上)
        /// </summary>
        public bool onBtn;
        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            onBtn = true;
            DialogResult = true;
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            onBtn = true;
            DialogResult = false;
        }
    }
}
