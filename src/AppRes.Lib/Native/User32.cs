using System;
using System.Runtime.InteropServices;

// ReSharper disable All

namespace AppRes.Lib.Native
{
    public class User32 : IUser32
    {
        [DllImport("user32.dll")]
        private static extern DISP_CHANGE ChangeDisplaySettingsEx(
            string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd,
            DisplaySettingsFlags dwflags, IntPtr lParam);

        bool IUser32.EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
            uint dwFlags) =>
            EnumDisplayDevices(lpDevice, iDevNum, ref lpDisplayDevice, dwFlags);

        DISP_CHANGE IUser32.ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd,
            DisplaySettingsFlags dwflags, IntPtr lParam) =>
            ChangeDisplaySettingsEx(lpszDeviceName, ref lpDevMode, hwnd, dwflags, lParam);

        int IUser32.EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode) =>
            EnumDisplaySettings(lpszDeviceName, iModeNum, ref lpDevMode);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayDevices(
            string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
            uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        private static extern int EnumDisplaySettings(
            string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        public const int DMDO_DEFAULT = 0;
        public const int DMDO_90 = 1;
        public const int DMDO_180 = 2;
        public const int DMDO_270 = 3;

        public const int ENUM_CURRENT_SETTINGS = -1;
    }
}