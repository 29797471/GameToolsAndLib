using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WinCore
{
    /// <summary>
    /// MapEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditorWindow : Window
    {
        public MapEditorWindow()
        {
            InitializeComponent();
        }
        private void OnChangeSelect(object sender, RoutedEventArgs e)
        {
            panel.SelectedIndex = -1;
            //var data = (sender as Button).DataContext as LevelMapNodeItem;
            //data.Select = !data.Select;
        }
    }
}
