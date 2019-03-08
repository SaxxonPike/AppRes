namespace AppRes.Lib.Processes
{
    public interface IProcessFinder
    {
        bool IsProcessRunning(string processName);
    }
}