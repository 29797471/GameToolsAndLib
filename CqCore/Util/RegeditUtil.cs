using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    public static class RegistryUtil
    {
        public static void StartExe(string path)
        {
            RegistryKey RKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            RKey.SetValue(FileOpr.GetNameByShort(path), path);
        }
    }
}
