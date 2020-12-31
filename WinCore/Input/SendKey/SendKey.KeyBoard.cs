using System;
using System.Runtime.InteropServices;

public static partial class SendKey
{
    #region SendMessage参数
    private const int WM_KEYDOWN = 0X100;
    private const int WM_KEYUP = 0X101;
    private const int WM_SYSCHAR = 0X106;
    private const int WM_SYSKEYUP = 0X105;
    private const int WM_SYSKEYDOWN = 0X104;
    private const int WM_CHAR = 0X102;
    #endregion

    

    

    

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int keybd_event(byte bVk, byte bScan, int dwFlags, UIntPtr dwExtraInfo);

    /// <summary>
    /// 转换
    /// 0.热键码hotkey 
    /// 1.控件码mod 
    /// 2.VK码
    /// </summary>
    private static byte ToKeyCode(int source, int dst, byte data)
    {
        int wRet = data;

        var map = new int[,]
        {
            { HOTKEYF_SHIFT, HOTKEYF_CONTROL, HOTKEYF_ALT },
            { MOD_SHIFT, MOD_CONTROL, MOD_ALT },
            { VK_SHIFT, VK_CONTROL, VK_MENU }
        };
        for (int i = 0; i < 3; i++)
        {
            
            if ( (data & map[source, i])== map[source, i])
            {
                wRet &= ~map[source,i];
                wRet |= map[dst,i];
            }
        }
        return (byte)wRet;
    }
    
    /// <summary>
    /// 在按下并放手key 
    /// </summary>
    public static void KeyClick(byte key)
    {
        if (key >= 'a' && key <= 'z')
        {
            key -= 32;
        }
        keybd_event(key, 0, 0, UIntPtr.Zero);
        keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    /// <summary>
    /// 按下mod 
    /// </summary>
    public static void KeyDown( byte mod)
    {
        //模拟按键具体实现
        if (mod != 0)
        {
            keybd_event(ToKeyCode(1, 2, mod), 0, 0, UIntPtr.Zero);
        }
    }
    /// <summary>
    /// 松开mod
    /// </summary>
    public static void KeyUp(byte mod)
    {
        if (mod != 0)
        {
            keybd_event(ToKeyCode(1, 2, mod), 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }
}