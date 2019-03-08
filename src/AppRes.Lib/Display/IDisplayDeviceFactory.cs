using System.Collections.Generic;

namespace AppRes.Lib.Display
{
    public interface IDisplayDeviceFactory
    {
        IDisplayDevice Get(int index);
        IEnumerable<IDisplayDevice> GetAll();
        IDisplayDevice GetDesktop();
    }
}