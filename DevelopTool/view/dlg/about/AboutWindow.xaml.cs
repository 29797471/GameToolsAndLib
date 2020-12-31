using CqCore;
using System.Diagnostics;
using System.Windows;

namespace DevelopTool
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            ProcessUtil.Start("tencent://message/?uin=29797471&Site=1&Menu=yes");
        }
    }
}
