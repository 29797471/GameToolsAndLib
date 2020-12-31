using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinCore
{
    /// <summary>
    /// 从一个列表中操作选出一个元素
    /// </summary>
    public partial class ChooseWindow : Window
    {
        public static readonly RoutedUICommand EscCommand =
            new RoutedUICommand("EscCommand", "EscCommand", typeof(ChooseWindow));

        public object SelectValue;

        public int SelectIndex;
        public void Edit(IEnumerable list)
        {
            listBox.ItemsSource = list;
            SelectIndex=listBox.SelectedIndex;
        }
        public ChooseWindow()
        {
            InitializeComponent();
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectValue = listBox.SelectedValue;
            SelectIndex = listBox.SelectedIndex;
            Close();
        }

        private void OnEscCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
