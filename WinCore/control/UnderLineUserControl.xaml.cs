using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System;

namespace WinCore
{
    /// <summary>
    /// UnderLineObjectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class UnderLineUserControl : UserControl
    {
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", 
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UnderLineUserControl));
        public event RoutedEventHandler Click
        {
            add { this.AddHandler(ClickEvent, value); }
            remove { this.RemoveHandler(ClickEvent, value); }
        }
    

        public object Target
        {
            get
            {
                return (object)GetValue(TargetProperty);
            }
            set
            {
                this.SetValueX(TargetProperty, value);
                UpdateViewData_Target();
            }
        }
        public void UpdateViewData_Target()
        {
            if(Target!=null) run.Text = Target.ToString();
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(UnderLineUserControl),
               new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetPropertyChangedCallback)));

        private static void TargetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = (UnderLineUserControl)d;
            uc.UpdateViewData_Target();
        }

        public UnderLineUserControl()
        {
            InitializeComponent();
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newEvent = new RoutedEventArgs(UnderLineUserControl.ClickEvent, this);
            this.RaiseEvent(newEvent);
        }
    }
}
