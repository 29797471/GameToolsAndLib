using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DevelopTool
{
    /// <summary>
    /// ChessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChessWindow : Window
    {
        public ChessWindow()
        {
            InitializeComponent();
            DataContext = ChessModel.instance;
            ChessModel.instance.Init();
        }

        private void MoveInChess(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(CanvasPanel);
            var x = ChessModel.ViewPosToDataPos(pos.X);
            var y = ChessModel.ViewPosToDataPos(pos.Y);
            if (x >= 0 && x < ChessModel.cell_number && y >= 0 && y < ChessModel.cell_number)
            {
                ChessModel.instance.players[0].MoveChess(x, y);
            }
        }

        private void AIInChess(object sender, RoutedEventArgs e)
        {
            ChessModel.instance.players[0].AIMove();
        }

        private void Window_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChessModel.instance.Remove();
        }
    }
}
