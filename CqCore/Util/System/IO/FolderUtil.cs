namespace System.IO
{
    /// <summary>
    /// 文件夹扩展
    /// </summary>
    public static class FolderUtil
    {
        /// <summary>
        /// 递归删除文件夹
        /// </summary>
        public static bool Delete(string folderPath, bool containSelf = false)
        {
            if (Directory.Exists(folderPath))
            {
                PreorderTraversal(folderPath, file => File.Delete(file), folder => Directory.Delete(folder));
                if (!containSelf) Directory.Delete(folderPath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 遍历文件夹下所有文件(包含子文件夹下的文件)
        /// </summary>
        public static void PreorderTraversal(string folderPath, Action<string> OnFile, Action<string> OnFolder)
        {
            string[] allFiles = Directory.GetFiles(folderPath);
            foreach (var file in allFiles)
            {
                OnFile(file);
            }

            string[] allFolders = Directory.GetDirectories(folderPath);
            foreach (var folder in allFolders)
            {
                PreorderTraversal(folder, OnFile, OnFolder);
                OnFolder(folder);
            }
        }
    }
}
