using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace SentinelX.Modules
{
    public class ProxyManager
    {
        private const string REG_PATH = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessageTimeout(
            IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam,
            uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        private const int HWND_BROADCAST = 0xffff;
        private const int WM_SETTINGCHANGE = 0x001A;

        public void EnableProxy(string server, int port)
        {
            Registry.SetValue(REG_PATH, "ProxyEnable", 1);
            Registry.SetValue(REG_PATH, "ProxyServer", $"{server}:{port}");
            BroadcastProxyChange();
        }

        public void DisableProxy()
        {
            Registry.SetValue(REG_PATH, "ProxyEnable", 0);
            BroadcastProxyChange();
        }

        private void BroadcastProxyChange()
        {
            UIntPtr result;
            SendMessageTimeout(
                (IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE, UIntPtr.Zero, "Internet Settings",
                0, 1000, out result);
        }
    }
} 