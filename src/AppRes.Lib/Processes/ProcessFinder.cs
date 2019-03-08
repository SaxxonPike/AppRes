using System.Diagnostics;
using System.Linq;
using AppRes.Lib.Infrastructure;

namespace AppRes.Lib.Processes
{
    [Service]
    public class ProcessFinder : IProcessFinder
    {
        public bool IsProcessRunning(string processName) => 
            Process.GetProcessesByName(processName).Any();
    }
}