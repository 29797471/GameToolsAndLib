namespace DevelopTool
{
    /// <summary>
    /// 新手指引 设置
    /// </summary>
    [Priority(13)]
    [Editor("剧情编辑器","/DevelopTool;component/Res/Images/Icons/play.ico")]
    public class StorySetting : Setting
    {
        /// <summary>
        /// 生成文件导出路径
        /// </summary>
        [FolderPath("导出目录", true), Priority(4),MinWidth(100)]
        public string MakePath { get { return makePath; } set { makePath = value; Update("MakePath"); } }
        public string makePath;
    }
}