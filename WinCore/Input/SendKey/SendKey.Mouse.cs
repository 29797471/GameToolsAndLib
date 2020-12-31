using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static partial class SendKey
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

    /// <summary>   
    /// 设置鼠标的坐标   
    /// </summary>   
    /// <param name="x">横坐标</param>   
    /// <param name="y">纵坐标</param>   
    [DllImport("User32")]
    public extern static void SetCursorPos(int x, int y);
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    /// <summary>   
    /// 获取鼠标的坐标   
    /// </summary>   
    /// <param name="lpPoint">传址参数，坐标point类型</param>   
    /// <returns>获取成功返回真</returns>   
    [DllImport("user32.dll", CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool GetCursorPos(out POINT pt);

    public static void MouseClick(bool left, int x, int y)
    {
        if (x == 0 && y == 0)
        {
            mouse_event(left ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(left ? MOUSEEVENTF_LEFTUP : MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
            
        }else
        {
            POINT currentPoint;
            GetCursorPos(out currentPoint);
            SetCursorPos(x, y);
            mouse_event(left ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(left ? MOUSEEVENTF_LEFTUP : MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
            SetCursorPos(currentPoint.X, currentPoint.Y);
        }
    }
    public static void MouseMove(int x, int y)
    { 
        //mouse_event(MOUSEEVENTF_MOVE, x- Control.MousePosition.X , y- Control.MousePosition.Y , 0, UIntPtr.Zero);
        SetCursorPos(x, y);
        //mouse_event(MOUSEEVENTF_MOVE, currentPoint.X-x, currentPoint.Y-y, 0, UIntPtr.Zero);
        //mouse_event(MOUSEEVENTF_MOVE, x, y, 0, UIntPtr.Zero);
    }
}