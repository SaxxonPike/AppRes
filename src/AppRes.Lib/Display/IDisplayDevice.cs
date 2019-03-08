using System.Drawing;

namespace AppRes.Lib.Display
{
    public interface IDisplayDevice
    {
        Size Resolution { get; set; }
        string Name { get; }
        int Index { get; }
    }
}