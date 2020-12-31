using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
[Editor("关卡布局")]
public class EditorMapNode : NotifyObject
{
    public List<LevelMapNodeItem> DataList
    {
        get => mDataList;
        set { mDataList = value; Update("DataList"); }
    }
    public List<LevelMapNodeItem> mDataList;

    public UVData CellSize
    {
        get
        {
            return mCellSize;
        }
    }
    public UVData mCellSize;
    /// <summary>
    /// 地图格子行列数
    /// </summary>
    public Vector2Int mMapSize;

    public float WidthSize
    {
        get=>CellSize.U * mMapSize.X;
    }
    public float CellWidth
    {
        get=>CellSize.U;
    }
    public float CellHeight
    {
        get => CellSize.V;
    }
    public float HeightSize
    {
        get => CellSize.V * mMapSize.Y;
    }
    public float ItemWidthSize
    {
        get=>CellSize.U;
    }
    public float ItemHeightSize
    {
        get=>CellSize.V;
    }
}
namespace DevelopTool
{
    /// <summary>
    /// LevelMapEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LevelMapEditorWindow : Window
    {
        public LevelMapEditorWindow()
        {
            InitializeComponent();
            
        }
        public const float scale = 0.8f;//按钮大小

        public void ShowEditDialog(ref bool[,] map, UVData cellSize, Vector2Int MapSize)
        {
            if (map == null || map.GetLength(0) != MapSize.Y || map.GetLength(1) != MapSize.X)
            {
                map = new bool[MapSize.Y, MapSize.X];
            }
            var mapData = new EditorMapNode()
            {
                mMapSize = MapSize,
                mCellSize = cellSize,
                mDataList = new List<LevelMapNodeItem>()
            };
            for (int i = 0; i < MapSize.Y; i++)
            {
                for (int j = 0; j < MapSize.X; j++)
                {
                    mapData.mDataList.Add(new LevelMapNodeItem()
                    {
                        style = map[i, j]?1:0,
                        mCellWidth = cellSize.U * scale,
                        mCellHeight = cellSize.V * scale
                    });
                }
            }
            DataContext = mapData;
            Change = data => data.Style = (data.style + 1) % 2;
            ShowDialog();
            for (int y = 0; y < MapSize.Y; y++) 
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    map[y,x]=mapData.mDataList[x + y* MapSize.X].style==1;
                }
            }
            
        }

        public void ShowEditDialog(ref int[,] map, UVData cellSize, Vector2Int MapSize)
        {
            if (map == null || map.GetLength(0) != MapSize.Y || map.GetLength(1) != MapSize.X)
            {
                map = new int[MapSize.Y, MapSize.X];
            }
            var mapData = new EditorMapNode()
            {
                mMapSize = MapSize,
                mCellSize = cellSize,
                mDataList = new List<LevelMapNodeItem>()
            };
            for (int i = 0; i < MapSize.Y; i++)
            {
                for (int j = 0; j < MapSize.X; j++)
                {
                    mapData.mDataList.Add(new LevelMapNodeItem()
                    {
                        style = map[i, j] ,
                        mCellWidth = cellSize.U * scale,
                        mCellHeight = cellSize.V * scale
                    });
                }
            }
            DataContext = mapData;
            Change = data => data.Style = (data.style + 1) % 3;
            ShowDialog();
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    map[y, x] = mapData.mDataList[x + y * MapSize.X].style ;
                }
            }
            
        }
        private void OnChangeSelect(object sender, RoutedEventArgs e)
        {
            panel.SelectedIndex = -1;
            var data = (sender as Button).DataContext as LevelMapNodeItem;

            Change(data);
            //if(data.style==0) data.Style = 1;
            //else data.Style = 0;
        }
        Action<LevelMapNodeItem> Change;
        private void image_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void Button_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            panel.SelectedIndex = -1;
            var data = (sender as Button).DataContext as LevelMapNodeItem;

            if (data.style == 0) data.Style = 2;
            else data.Style = 0;
        }
    }
}
