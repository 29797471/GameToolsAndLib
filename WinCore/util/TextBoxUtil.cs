//using System;
//using System.Runtime.InteropServices;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;

//namespace WinCore.util
//{
//    public static class TextBoxUtil
//    {
//        // set tab stops to a width of 4
//        private const int EM_SETTABSTOPS = 0x00CB;

//        [DllImport("User32.dll", CharSet = CharSet.Auto)]
//        public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

//        public static void SetTabWidth(this TextBox textbox, int tabWidth)
//        {
//            Graphics graphics = textbox.CreateGraphics();
//            var characterWidth = (int)graphics.MeasureString("M", textbox.Font).Width;
//            SendMessage(textbox.Handle, EM_SETTABSTOPS, 1,
//                        new int[] { tabWidth * characterWidth });
//        }
//    }
//}
