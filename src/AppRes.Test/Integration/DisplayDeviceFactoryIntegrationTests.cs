using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using AppRes.Lib.Display;
using AppRes.Test.Testing;
using NUnit.Framework;

namespace AppRes.Test.Integration
{
    public class DisplayDeviceFactoryIntegrationTests : BaseIntegrationTestFixture<DisplayDeviceFactory>
    {
        [Test]
        [Explicit("This will actually change your screen resolution temporarily, be careful!")]
        public void Test1()
        {
            var display = Subject.GetAll().First();
            var resolution = display.Resolution;

            try
            {
                display.Resolution = new Size(1920, 1080);
                Thread.Sleep(3000);
            }
            finally
            {
                display.Resolution = resolution;
            }
        }
    }
}