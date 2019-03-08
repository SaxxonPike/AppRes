using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AppRes.Lib.Infrastructure;
using AppRes.Lib.Native;

namespace AppRes.Lib.Display
{
    [Service]
    public class DisplayDeviceFactory : IDisplayDeviceFactory
    {
        private readonly IUser32 _user32;

        public DisplayDeviceFactory(IUser32 user32)
        {
            _user32 = user32;
        }

        public IDisplayDevice Get(int index)
        {
            var d = new DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);

            return !_user32.EnumDisplayDevices(null, (uint) index, ref d, 0)
                ? null
                : new DisplayDevice(_user32, index, d);
        }

        public IEnumerable<IDisplayDevice> GetAll()
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var dd = Get(i);
                if (dd == null)
                    yield break;
                yield return dd;
            }
        }

        public IDisplayDevice GetDesktop()
        {
            return GetAll().First(d => d.IsPrimaryDesktop);
        }
    }
}