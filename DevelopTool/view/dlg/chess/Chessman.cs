using DevelopTool;
using System.Windows;
/// <summary>
/// 棋子
/// </summary>
public class Chessman
{
    public Visibility ShowBlack
    {
        get
        {
            return isBlack?Visibility.Visible:Visibility.Hidden;
        }
    }
    public Visibility ShowWhite
    {
        get
        {
            return !isBlack ? Visibility.Visible : Visibility.Hidden;
        }
    }
    public double Left
    {
        get
        {
            return ChessModel.DataPosToViewPos(x);
        }
    }
    public double Top
    {
        get
        {
            return ChessModel.DataPosToViewPos(y);
        }
    }
    public bool isBlack;
    public int x, y;
    
}