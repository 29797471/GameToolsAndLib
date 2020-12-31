using System.Windows.Controls;

namespace DevelopTool
{
    /// <summary>
    /// HelpPage2.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelHelpPage : Page
    {
        public ExcelHelpPage()
        {
            InitializeComponent();
            DataContext = Properties.Resources.excelHelp;
        }
    }
}
