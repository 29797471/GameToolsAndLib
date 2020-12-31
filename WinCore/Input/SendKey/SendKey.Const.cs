public static partial class SendKey
{
    #region Windows 鼠标事件常量
    public const int MOUSEEVENTF_MOVE = 0x0001;/* mouse move */
    public const int MOUSEEVENTF_LEFTDOWN = 0x0002;/* left button down */
    public const int MOUSEEVENTF_LEFTUP = 0x0004;/* left button up */
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;/* right button down */
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;/* right button up */
    public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;/* middle button down */
    public const int MOUSEEVENTF_MIDDLEUP = 0x0040;/* middle button up */
    #endregion

    #region Windows 键盘事件常量
    public const int KEYEVENTF_KEYUP = 0x0002;/* middle button up */

    public const int MOD_ALT = 0x0001;
    public const int MOD_CONTROL = 0x0002;
    public const int MOD_SHIFT = 0x0004;
    public const int MOD_WIN = 0x0008;

    public const int HOTKEYF_SHIFT = 0x01;
    public const int HOTKEYF_CONTROL = 0x02;
    public const int HOTKEYF_ALT = 0x04;

    public const int VK_SHIFT = 0x10;
    public const int VK_CONTROL = 0x11;
    public const int VK_MENU = 0x12;
    #endregion
}