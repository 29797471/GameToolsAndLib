using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WinCore
{
    /// <summary>
    /// 带标题的区域框
    /// </summary>
    public partial class AreaBoxUserControl : UserControl
    {
        public AreaBoxUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(AreaBoxUserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetPropertyChangedCallback)));

        private static void TargetPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            var com = sender as AreaBoxUserControl;
            com.UpdateViewData_Target();
        }
        public object Target
        {
            get
            {
                return GetValue(TargetProperty);
            }
            set
            {
                this.SetValueX(TargetProperty, value);
                UpdateViewData_Target();
            }
        }
        void UpdateViewData_Target()
        {
            if (Target == null)
            {
                cc.Visibility = Visibility.Collapsed;
                return;
            }
            cc.Visibility = Visibility.Visible;
            cc.Content = WinUtil.DrawHCC<GroupBox>(Target);
        }
    }
}
