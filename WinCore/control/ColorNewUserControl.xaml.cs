using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCore
{
    /// <summary>
    /// ColorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ColorNewUserControl : UserControl
    {
        public static readonly DependencyProperty WindowsMediaColorProperty =
            DependencyProperty.Register("WindowsMediaColor", typeof(Color), typeof(ColorNewUserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(WindowsMediaColorPropertyChangedCallback)));


        private static void WindowsMediaColorPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            var com = sender as ColorNewUserControl;
            com.UpdateViewData();
        }
        public Color WindowsMediaColor
        {
            get
            {
                return (Color)GetValue(WindowsMediaColorProperty);
            }
            set
            {
                this.SetValueX(WindowsMediaColorProperty, value);
                UpdateViewData();
            }
        }
        void UpdateViewData()
        {
            if (((byte)slider.Value) != WindowsMediaColor.A) slider.Value = WindowsMediaColor.A;
        }
        
        public ColorNewUserControl()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.Color = WinUtil.ToWinColor(WindowsMediaColor);

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WindowsMediaColor = WinUtil.ToMediaColor(colorDialog.Color);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.Value = (byte)(slider.Value);
            Color c = WindowsMediaColor;
            c.A = (byte)(slider.Value);
            WindowsMediaColor = c;

        }
    }
}
