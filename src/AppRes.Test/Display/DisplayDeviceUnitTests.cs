using System;
using System.Drawing;
using AppRes.Lib.Display;
using AppRes.Lib.Native;
using AppRes.Test.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AppRes.Test.Display
{
    public class DisplayDeviceUnitTests : BaseUnitTestFixture<DisplayDevice>
    {
        private DEVMODE _devModeIn;
        private DEVMODE _devModeOut;

        delegate void EnumDisplaySettingsCallback(string name, int mode, ref DEVMODE dm);

        delegate void ChangeDisplaySettingsExCallback(string name, ref DEVMODE dm, IntPtr hwnd,
            DisplaySettingsFlags dwFlags, IntPtr lParam);

        [SetUp]
        public void __Setup()
        {
            _devModeOut = Create<DEVMODE>();
            _devModeIn = default(DEVMODE);
            
            // This is some of the ugliest test setup I've ever had to do, and it's all because of
            // 'ref' parameters, something that unmanaged interop absolutely loves to do.

            MockService<IUser32>(mock =>
            {
                // Make these calls always succeed.
                mock.Setup(x => x.EnumDisplaySettings(
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        ref It.Ref<DEVMODE>.IsAny))
                    .Callback(new EnumDisplaySettingsCallback((string name, int mode, ref DEVMODE dm) =>
                    {
                        dm = _devModeOut;
                    }))
                    .Returns(1);

                mock.Setup(x => x.ChangeDisplaySettingsEx(
                        It.IsAny<string>(),
                        ref It.Ref<DEVMODE>.IsAny,
                        It.IsAny<IntPtr>(),
                        It.IsAny<DisplaySettingsFlags>(),
                        It.IsAny<IntPtr>()))
                    .Callback(new ChangeDisplaySettingsExCallback(
                        (string name, ref DEVMODE dm, IntPtr hwnd, DisplaySettingsFlags flags, IntPtr param) =>
                        {
                            _devModeIn = dm;
                        }))
                    .Returns(DISP_CHANGE.Successful);
            });
        }

        protected override DisplayDevice __CreateSubject()
        {
            return new DisplayDevice(MockService<IUser32>().Object, Create<int>(), Create<DISPLAY_DEVICE>());
        }

        [Test]
        public void Resolution_Get_ShouldRetrieveResolution()
        {
            // Act.
            var observed = Subject.Resolution;

            // Assert.
            observed.Width.Should().Be(_devModeOut.dmPelsWidth);
            observed.Height.Should().Be(_devModeOut.dmPelsHeight);
        }

        [Test]
        public void Resolution_Set_ShouldChangeResolution()
        {
            // Arrange.
            var expectedWidth = Create<int>();
            var expectedHeight = Create<int>();

            // Act.
            Subject.Resolution = new Size(expectedWidth, expectedHeight);

            // Assert.
            _devModeIn.dmPelsWidth.Should().Be(expectedWidth);
            _devModeIn.dmPelsHeight.Should().Be(expectedHeight);
        }
    }
}