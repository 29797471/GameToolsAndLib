using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace WinCore
{
    /// <summary>
    /// 功能说明：HookManager分部类，钩子事件处理
    /// 开发人员：王旭（http://www.wxzzz.com）
    /// 开发时间：2014年4月3日
    /// 使用3.5.net
    /// </summary>
    public static partial class HookManager
    {
        
        /// <summary>
        /// 该CallWndProc钩子程序与调用SetWindowsHookEx函数一起使用的应用程序定义的或者库定义的回调函数。
        /// 该HOOKPROC类型定义了一个指向此回调函数。
        /// CallWndProc是一个占位符的应用程序定义的或者库定义的函数名。
        /// </summary>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);


        static Version Net_3_5 = new Version("3.5.30729.4926");

        private static IntPtr GetHINSTANCE()
        {
            if(Environment.Version== Net_3_5)return Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            else return GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
        }

        #region 键盘钩子程序

        /// <summary>
        /// 此字段不客观需要的，但我们需要保持一个供参考的将被传递给非托管代码的委托。
        /// 为了避免GC把它清理干净。
        /// </summary>
        private static HookProc s_KeyboardDelegate;

        /// <summary>
        /// 存储句柄键盘钩子程序。
        /// </summary>
        private static int s_KeyboardHookHandle;

        /// <summary>
        /// 键盘检测活动将被称为每次回调函数。
        /// </summary>
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //表示如有underlaing事件设置e.Handled标志
            bool handled = false;

            if (nCode >= 0)
            {
                //在lParam中读取KeyboardHookStruct结构
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (s_KeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VirtualKeyCode;
                    if ((GetKeyState(VK_CONTROL) & 0x80) == 0x80 )
                    {
                        keyData |= Keys.Control;
                    }
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    
                    s_KeyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                // 按键按下
                if (s_KeyPress != null && wParam == WM_KEYDOWN)
                {
                    bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);

                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.VirtualKeyCode,
                              MyKeyboardHookStruct.ScanCode,
                              keyState,
                              inBuffer,
                              MyKeyboardHookStruct.Flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        s_KeyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
                }

                // 按键弹起
                if (s_KeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    s_KeyUp.Invoke(null, e);
                    handled = handled || e.Handled;
                }

            }

            //如果事件在应用程序处理的不换手到其他听众
            if (handled)
                return -1;

            //转发到其它应用程序
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // 安装键盘钩子，只有当它没有安装，必须安装
            if (s_KeyboardHookHandle == 0)
            {
                //为了避免GC把它清理干净。
                s_KeyboardDelegate = KeyboardHookProc;
                //安装钩子
                s_KeyboardHookHandle = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    s_KeyboardDelegate,
                    GetHINSTANCE(),
                    0);
                //如果SetWindowsHookEx函数将失败。
                if (s_KeyboardHookHandle == 0)
                {
                    //返回由上一个非托管函数使用平台调用称为具有DllImportAttribute.SetLastError标志设置返回的错误代码. 
                    int errorCode = Marshal.GetLastWin32Error();

                    //初始化并抛出初始化Win32Exception类的新实例使用指定的错误。
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //如果没有subsribers从钩注册unsubsribe
            if (s_KeyDown == null &&
                s_KeyUp == null &&
                s_KeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (s_KeyboardHookHandle != 0)
            {
                //卸载钩子
                int result = UnhookWindowsHookEx(s_KeyboardHookHandle);
                //重置句柄
                s_KeyboardHookHandle = 0;
                //清理
                s_KeyboardDelegate = null;
                //如果失败，异常必须抛出
                if (result == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

#endregion

    }
}
