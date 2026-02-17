using System.Diagnostics;

namespace KeyboardLayoutSwitcher;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Prevent multiple instances: kill existing ones
        Process current = Process.GetCurrentProcess();
        Process[] processes = Process.GetProcessesByName(current.ProcessName);
        foreach (Process process in processes)
        {
            if (process.Id != current.Id)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(1000);
                }
                catch { /* Ignore errors if process is already closing */ }
            }
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new TrayApplicationContext());
    }    
}