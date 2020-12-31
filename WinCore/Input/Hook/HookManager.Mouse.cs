
using CqCore;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WinCore
{
    /// <summary>
    /// 鼠标钩子处理
    /// </summary>
    public static partial class HookManager
    {
        /// <summary>
        /// 此字段不客观需要的，但我们需要保持一个供参考的将被传递给非托管代码的委托。为了避免GC把它清理干净。
        /// </summary>
        private static HookProc s_MouseDelegate;

        /// <summary>
        /// 存储句柄的鼠标钩子程序
        /// </summary>
        private static int s_MouseHookHandle;

        /// <summary>
        /// 鼠标检测活动将被称为每次回调函数
        /// </summary>
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Marshall 从回调的数据
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));
                int x = mouseHookStruct.Point.X;
                int y = mouseHookStruct.Point.Y;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        MouseDown(MouseButton.Left, x, y);
                        break;
                    case WM_LBUTTONUP:
                        MouseUp(MouseButton.Left, x, y);
                        break;
                    case WM_LBUTTONDBLCLK:
                        MouseDoubleClick(MouseButton.Left, x, y);
                        break;
                    case WM_RBUTTONDOWN:
                        MouseDown(MouseButton.Right, x, y);
                        break;
                    case WM_RBUTTONUP:
                        MouseUp(MouseButton.Right, x, y);
                        break;
                    case WM_RBUTTONDBLCLK:
                        MouseDoubleClick(MouseButton.Right, x, y);
                        break;
                    case WM_MOUSEWHEEL:
                        //如果消息是WM_MOUSEWHEEL，MouseData成员是滚轮。
                        //一个轮击定义为WHEEL_DELTA，这是120。
                        //(value >> 16) & 0xffff; 从给定的32位值检索高位字。
                        var mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);

                        //TODO: X BUTTONS (这个按钮暂时没有测试)
                        break;
                }
                //鼠标的通知不应该阻止继续派发if (false)return -1;
            }
            
            //转发到其它应用程序
            return CallNextHookEx(s_MouseHookHandle, nCode, wParam, lParam);
        }
        public static void RunWin32GlobalMouseEvents()
        {
            // 安装鼠标钩子
            if (s_MouseHookHandle == 0)
            {
                //为了避免GC把它清理干净。
                s_MouseDelegate = MouseHookProc;
                //安装钩子
                s_MouseHookHandle = SetWindowsHookEx(
                    WH_MOUSE_LL,
                    s_MouseDelegate,
                    GetHINSTANCE(),
                    0);
                //如果SetWindowsHookEx函数将失败。
                if (s_MouseHookHandle == 0)
                {
                    //返回由上一个非托管函数使用平台调用称为具有DllImportAttribute.SetLastError标志设置返回的错误代码。
                    int errorCode = Marshal.GetLastWin32Error();

                    //初始化并抛出初始化Win32Exception类的新实例使用指定的错误。 
                    throw new Win32Exception(errorCode);
                }
            }
        }


        public static void StopWin32GlobalMouseEvents()
        {
            if (s_MouseHookHandle != 0)
            {
                //卸载钩子
                int result = UnhookWindowsHookEx(s_MouseHookHandle);
                //复位无效句柄
                s_MouseHookHandle = 0;
                //释放用于GC
                s_MouseDelegate = null;
                //如果失败，异常必须抛出
                if (result == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }
        #region 由按下抬起生成点击事件
        static HookManager()
        {
            MouseDownHandler += EventHandler_MouseDown;
        }
        /// <summary>
        /// 优雅的点击事件
        /// </summary>
        private static void EventHandler_MouseDown(object sender, MouseDownEventArgs e)
        {
            EventHandler<MouseUpEventArgs> call = null;
            call =( s,  ee) => 
            {
                MouseUpHandler -= call;
                if (e.button == ee.button && e.x==ee.x && e.y==ee.y) MouseClick(e.button,e.x,e.y);
            };
            MouseUpHandler += call;
            
        }

        #endregion
    }
}
