using DevelopTool;
using System.Windows;

public class LevelMapNodeItem : NotifyObject
{
    public Visibility Show2
    {
        get => (style == 2) ? Visibility.Visible : Visibility.Collapsed;
    }
    public Visibility Show1
    {
        get => (style==1)? Visibility.Visible:Visibility.Collapsed;
    }
    public Visibility Show0
    {
        get => (style == 0) ? Visibility.Visible : Visibility.Collapsed;
    }
    public float Alp
    {
        get
        {
            return (style!=0) ? 0.7f : 0;
        }
    }
    public int style;
    public int Style
    {
        set
        {
            style = value;
            Update("Show0");
            Update("Show1");
            Update("Show2");
            Update("Alp");
        }
    }

    public const float scale = 0.8f;//按钮大小

    public float mCellWidth;
    public float CellWidth
    {
        get => mCellWidth;
    }
    public float mCellHeight;
    public float CellHeight
    {
        get => mCellHeight;
    }
}