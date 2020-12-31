using System.IO;
using System.Text;

namespace System.Windows.Controls
{
    public static class WebBrowserUtil
    {
        public static void NavigateToUTF8String(this WebBrowser wb, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }
            MemoryStream memoryStream = new MemoryStream(text.Length);
            StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write(text);
            streamWriter.Flush();
            memoryStream.Position = 0L;
            wb.NavigateToStream(memoryStream);
        }
    }
}
