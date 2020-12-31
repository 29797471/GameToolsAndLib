namespace DevelopTool
{
    [Priority(12)]
    [Editor("行为编辑器", "/DevelopTool;component/Res/Images/Icons/ai.ico")]
    public class AISetting : Setting
    {
        /// <summary>
        /// 生成文件导出路径
        /// </summary>
        [FolderPath("导出目录", true), Priority(4),MinWidth(100)]
        public string MakePath { get { return makePath; } set { makePath = value; Update("MakePath"); } }
        public string makePath;
    }
}