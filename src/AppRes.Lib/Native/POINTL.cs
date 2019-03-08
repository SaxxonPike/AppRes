using System.Runtime.InteropServices;

// ReSharper disable All

namespace AppRes.Lib.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        public long x;
        public long y;
    }
}