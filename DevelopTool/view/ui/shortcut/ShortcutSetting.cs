namespace DevelopTool
{
    [Priority(20)]
    [Editor("快捷方式设置", "/DevelopTool;component/Res/Images/Icons/app.ico")]
    public class ShortcutSetting : Setting
    {
        /// <summary>
        /// 生成文件导出路径
        /// </summary>
        [FolderPath("导出目录", true), Priority(4),MinWidth(100)]
        public string MakePath { get { return makePath; } set { makePath = value; Update("MakePath"); } }
        public string makePath;
    }
}