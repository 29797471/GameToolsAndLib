using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WinCore
{
    /// <summary>
    /// RichTextUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class RichTextUserControl : UserControl
    {
        
        public IList<RichTextUCItem> ItemsSource
        {
            get
            {
                return (IList<RichTextUCItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                this.SetValueX(ItemsSourceProperty, value);

                UpdateView();
            }
        }
        void UpdateView()
        {
            tb.Inlines.Clear();
            foreach (var it in ItemsSource)
            {
                var span = (Span)FindResource("ResSpan");
                span.DataContext = it;
                tb.Inlines.Add(span);
            }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList<RichTextUCItem>), typeof(RichTextUserControl),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsSourcePropertyChangedCallback)));

        private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var com = (RichTextUserControl)d;
            com.UpdateView();
        }

        public RichTextUserControl()
        {
            InitializeComponent();
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var rtu=(sender as Hyperlink).DataContext as RichTextUCItem;
            if(rtu.CallBack!=null) rtu.CallBack();
        }
    }
}
