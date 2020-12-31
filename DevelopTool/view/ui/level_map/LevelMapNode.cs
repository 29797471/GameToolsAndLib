using DevelopTool;
using System.Collections.Generic;

[Editor("关卡布局")]
public class LevelMapNode : NotifyObject
{
    public string Title { get { return "地图"; } }

    [Priority(0)]
    [TextBox("名称", 100),MinWidth(200)]
    [Export("%Name%")]
    public string Name
    {
        get { return name; }
        set { name = value; Update("Name"); }
    }
    public string name = "temp";

    [Priority(2)]
    [TextBox("顺时针旋转角度", 100), MinWidth(200)]
    [Export("%Rote%")]
    public string Rote
    {
        get { return mRote; }
        set { mRote = value; Update("Rote"); }
    }
    public string mRote;

    [Priority(1)]
    [TextBox("资源路径",100), MinWidth(200)]
    [Export("%ResPath%")]
    public string ResPath
    {
        get { return mResPath; }
        set { mResPath = value; Update("ResPath"); }
    }
    public string mResPath;

    /// <summary>
    /// 地图格子行列数
    /// </summary>
    [MaxWidth(200)]
    [MinWidth(100)]
    [Priority(5)]
    [UnderLine("地图格子行列数"), Click]
    public Vector2Int MapSize
    {
        get { if (mMapSize == null) mMapSize = new Vector2Int(); return mMapSize; }
        set { mMapSize = value; Update("MapSize"); }
    }
    public Vector2Int mMapSize;

    [Priority(10)]
    [Button("二维地图"),Click("OnEditMap")]
    public string Btn {get { return "编辑地图"; } }

    [Priority(12)]
    [CheckBox("禁用", 100)]
    public bool Disable { set { mDisable = value; Update("Disable"); } get { return mDisable; } }
    public bool mDisable;

    public int[,] mMap;
    public void OnEditMap(object obj)
    {
        //for (int i = 0; i < mMap.GetLength(0); i++)
        //{
        //    for (int j = 0; j < mMap.GetLength(1); j++)
        //    {
        //        mMap[i, j] = 2;
        //    }
        //}
        var win = new LevelMapEditorWindow();
        win.ShowEditDialog(ref mMap, LevelMapModel.instance.setting.CellSize, MapSize);
    }
}

