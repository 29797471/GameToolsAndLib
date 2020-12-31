namespace DevelopTool
{
    [Priority(8)]
    [Editor("关卡布局编辑器", "/DevelopTool;component/Res/Images/Icons/map.ico")]
    public class LevelMapSetting : Setting
    {

        /// <summary>
        /// 显示地图大小
        /// </summary>
        [MaxWidth(200)]
        [MinWidth(100)]
        [Priority(5)]
        [UnderLine("单元格大小"),Click]
        public UVData CellSize
        {
            get { if (mCellSize == null) mCellSize = new UVData(); return mCellSize; }
            set { mCellSize = value; Update("CellSize"); }
        }
        public UVData mCellSize;

        [Priority(4)]
        [Button("生成文件"),Click]
        public MakeFile TemplateFile
        {
            get { if (mTempleteFile == null) mTempleteFile = new MakeFile(); return mTempleteFile; }
            set { mTempleteFile = value; Update("TemplateFile"); }
        }
        public MakeFile mTempleteFile;
    }
}