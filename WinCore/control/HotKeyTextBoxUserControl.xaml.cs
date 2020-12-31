using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinCore
{

    /// <summary>
    /// 屏蔽输入法
    /// </summary>
    public partial class HotKeyTextBoxUserControl : UserControl
    {
        public string ShortcutKey
        {

            get
            {
                return (string)GetValue(ShortcutKeyProperty);
            }
            set
            {
                this.SetValueX(ShortcutKeyProperty, value);
                UpdateView_ShortcutKey();
            }
        }
        public static readonly DependencyProperty ShortcutKeyProperty =
           DependencyProperty.Register("ShortcutKey", typeof(string), typeof(HotKeyTextBoxUserControl),
               new FrameworkPropertyMetadata(new PropertyChangedCallback(ShortcutKeyPropertyChangedCallback)));

        void UpdateView_ShortcutKey()
        {
            textBox.Text = ShortcutKey;
        }
        private static void ShortcutKeyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (HotKeyTextBoxUserControl)d;
            userControl.UpdateView_ShortcutKey();
        }


        public HotKeyTextBoxUserControl()
        {
            InitializeComponent();
            //System.Windows.Input.InputMethod.SetIsInputMethodEnabled(ctl, false);
        }
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            //int keyValue = KeyInterop.VirtualKeyFromKey(e.Key);
            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                ShortcutKey = e.Key.ToString();
            }
            else
            {
                switch (e.Key)
                {
                    case Key.LeftCtrl:
                    case Key.RightCtrl:
                    case Key.LeftAlt:
                    case Key.RightAlt:
                    case Key.LeftShift:
                    case Key.RightShift:
                    case Key.System:
                        ShortcutKey = Keyboard.Modifiers.ToString();
                        break;
                    default:
                        try
                        {
                            ShortcutKey = new KeyGestureValueSerializer().ConvertToString(new KeyGesture(e.Key, Keyboard.Modifiers), null);
                        }
                        catch (Exception)
                        {

                        }
                        break;
                }
            }
            e.Handled = true;//事件不再传递
        }
    }
}
