using System;
using System.Drawing;
using AppRes.Lib.Native;

namespace AppRes.Lib.Display
{
    public class DisplayDevice : IDisplayDevice
    {
        private readonly IUser32 _user32;
        private readonly int _index;
        private readonly DISPLAY_DEVICE _displayDevice;

        public DisplayDevice(IUser32 user32, int index, DISPLAY_DEVICE displayDevice)
        {
            _user32 = user32;
            _index = index;
            _displayDevice = displayDevice;
        }

        private void GetDevice(ref DEVMODE dm)
        {
            var result = _user32.EnumDisplaySettings(
                _displayDevice.DeviceName,
                User32.ENUM_CURRENT_SETTINGS,
                ref dm);

            if (result == 0)
                throw new Exception("Failed to get device.");
        }

        private void SetDevice(ref DEVMODE dm)
        {
            var result = _user32.ChangeDisplaySettingsEx(
                _displayDevice.DeviceName,
                ref dm,
                IntPtr.Zero,
                0,
                IntPtr.Zero);

            if (result != DISP_CHANGE.Successful)
                throw new Exception("Failed to set resolution.");
        }

        public Size Resolution
        {
            get
            {
                var dm = new DEVMODE();
                GetDevice(ref dm);
                return new Size(dm.dmPelsWidth, dm.dmPelsHeight);
            }
            set
            {
                var dm = new DEVMODE();
                GetDevice(ref dm);
                dm.dmPelsWidth = value.Width;
                dm.dmPelsHeight = value.Height;
                SetDevice(ref dm);
            }
        }

        public string Name => _displayDevice.DeviceName;
        public int Index => _index;

        public bool IsPrimaryDesktop =>
            _displayDevice.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice |
                                              DisplayDeviceStateFlags.AttachedToDesktop);
    }
}