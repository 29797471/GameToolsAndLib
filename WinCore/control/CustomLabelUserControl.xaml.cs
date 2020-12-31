using CqCore;
using System.Windows;
using System.Windows.Controls;

namespace WinCore
{
    /// <summary>
    /// 双击后可以编辑的label
    /// </summary>
    public partial class CustomLabelUserControl : UserControl
    {
        public CustomLabelUserControl()
        {
            InitializeComponent();
            ChangeMode(false);
        }

        void ChangeMode(bool editMode)
        {
            textBlock.Visibility = editMode ? Visibility.Hidden : Visibility.Visible;
            textBox.Visibility = editMode ? Visibility.Visible : Visibility.Hidden;
            if (editMode) GlobalCoroutine.DelayCall(100, SetFocus);
        }
        void SetFocus()
        {
            var bl = textBox.Focus();
            if (bl == false) GlobalCoroutine.DelayCall(100, SetFocus);
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ChangeMode(false);
        }

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangeMode(true);

        }
        
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                this.SetValueX(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(CustomLabelUserControl));



        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = textBox.Text;
        }
    }
}