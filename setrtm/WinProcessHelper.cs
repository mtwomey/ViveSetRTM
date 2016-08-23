using System.Diagnostics;

namespace WinProcess
{
    static class WinProcessHelper
    {

        public static void CloseAllByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
        }

        public static void KillAllByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }
        }

        public static void WaitForExit(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.WaitForExit();
            }
        }

        public static int NumProcessByName(string processName)
        {
            return Process.GetProcessesByName(processName).Length;
        }
    }
}
