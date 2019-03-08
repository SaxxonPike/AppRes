using System;
using AppRes.Lib.Infrastructure;

namespace AppRes.Application
{
    [Service]
    public class Keyboard : IKeyboard
    {
        public bool IsKeyAvailable => Console.KeyAvailable;
    }
}