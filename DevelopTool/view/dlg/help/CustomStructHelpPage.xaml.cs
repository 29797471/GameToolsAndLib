using System.Windows.Controls;

namespace DevelopTool
{
    /// <summary>
    /// HelpPage1.xaml 的交互逻辑
    /// </summary>
    public partial class CustomStructHelpPage : Page
    {
        public CustomStructHelpPage()
        {
            InitializeComponent();
            DataContext = Properties.Resources.customStructHelp;
        }
    }
}
