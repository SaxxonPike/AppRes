using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using AppRes.Lib.Display;
using AppRes.Lib.Infrastructure;
using AppRes.Lib.Processes;

namespace AppRes.Application
{
    [Service]
    public class App : IApp
    {
        private readonly TextWriter _console;
        private readonly IDisplayDeviceFactory _displayDeviceFactory;
        private readonly IProcessFinder _processFinder;
        private readonly IKeyboard _keyboard;

        public App(TextWriter console, IDisplayDeviceFactory displayDeviceFactory, IProcessFinder processFinder,
            IKeyboard keyboard)
        {
            _console = console;
            _displayDeviceFactory = displayDeviceFactory;
            _processFinder = processFinder;
            _keyboard = keyboard;
        }
        
        public void Start(string[] args)
        {
            var options = args.TakeWhile(a => a.StartsWith('/')).Select(a => a.ToLowerInvariant()).ToArray();
            var effectiveArgs = args.Skip(options.Length).ToArray();

            // Show documentation if there aren't enough args.
            if (effectiveArgs.Length < 2)
            {
                _console.WriteLine("Usage:");
                _console.WriteLine("AppRes [<options>] <width> <height> [<application>]");
                _console.WriteLine();
                _console.WriteLine("Omitting <application> will simply set the screen resolution and exit.");
                _console.WriteLine("But if <application> is specified, the resolution will be set for as long");
                _console.WriteLine("as the application runs.");
                _console.WriteLine();
                _console.WriteLine("By default, this program runs in Watcher mode: you would specify the name of");
                _console.WriteLine("a *process* - usually the executable file name without the .exe part.");
                _console.WriteLine("I wrote this for use with WoW initially, so the application would be Wow.");
                _console.WriteLine();
                _console.WriteLine("Option /w will run this application in Wait mode: it will wait for the");
                _console.WriteLine("application with the specified file name to run, and will only enforce the");
                _console.WriteLine("screen resolution while that application is running. After the application");
                _console.WriteLine("returns control, the resolution change will be reverted. In this mode, the");
                _console.WriteLine("full file name must be specified, along with any command line parameters you");
                _console.WriteLine("would like to use.");
                _console.WriteLine();
                return;
            }
            
            // Arg 0: width
            if (!int.TryParse(effectiveArgs[0], out var width))
            {
                _console.WriteLine($"Invalid width. Expected an integer, but found {args[0]}");
                return;
            }
            
            // Arg 1: height
            if (!int.TryParse(effectiveArgs[1], out var height))
            {
                _console.WriteLine($"Invalid height. Expected an integer, but found {args[0]}");
                return;
            }

            if (options.Contains("/w"))
            {
                RunStandard(width, height, effectiveArgs);
            }
            else
            {
                RunWatcher(width, height, effectiveArgs);
            }
        }

        private void RunStandard(int width, int height, string[] args)
        {
            // Set the new resolution.
            var display = _displayDeviceFactory.GetDesktop();
            var oldResolution = display.Resolution;
            display.Resolution = new Size(width, height);

            // Short circuit if no app is specified.
            if (args.Length == 2)
                return;

            // Start and wait for the app.
            var appCommand = string.Join(" ", args.Skip(2));
            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = appCommand
            }))
            {
                process?.WaitForExit();
            }

            // Clean up after ourselves.
            display.Resolution = oldResolution;
        }

        private void RunWatcher(int width, int height, string[] args)
        {
            var resolutionActive = false;
            var display = _displayDeviceFactory.GetDesktop();
            var oldResolution = display.Resolution;
            var appCommand = string.Join(" ", args.Skip(2));

            _console.WriteLine("Running in Watcher mode.");
            _console.WriteLine($"Process name: {appCommand}");
            
            while (!_keyboard.IsKeyAvailable)
            {
                var appActive = _processFinder.IsProcessRunning(appCommand);
                if (appActive != resolutionActive)
                {
                    _console.WriteLine($"Resolution is {(appActive ? "active" : "not active")}");
                    display.Resolution = appActive ? new Size(width, height) : oldResolution;
                    resolutionActive = appActive;
                }
                Thread.Sleep(1000);
            }
        }
    }
}