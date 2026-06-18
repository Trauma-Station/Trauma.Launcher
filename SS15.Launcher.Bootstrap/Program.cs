using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SS15.Launcher.Bootstrap
{
    internal static partial class Program
    {
        public static void Main(string[] args)
        {
            var path = AppContext.BaseDirectory;
            var ourDir = Path.GetDirectoryName(path)!;
            Debug.Assert(ourDir != null);

            var architecture = "x64";
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64
                && Directory.Exists(Path.Combine(ourDir, "dotnet_arm64")))
            {
                architecture = "arm64";
            }

            var dotnetDir = Path.Combine(ourDir, $"dotnet_{architecture}");
            var exeDir = Path.Combine(ourDir, $"bin_{architecture}");

            Environment.SetEnvironmentVariable("DOTNET_ROOT", dotnetDir);
            if (Array.IndexOf(args, "--debug") == -1)
            {
                Process.Start(new ProcessStartInfo(Path.Combine(exeDir, "SS15.Launcher.exe")));
            }
            else
            {
                AllocConsole();

                Console.WriteLine("Console yourself some, uhhh");

                var process = Process.Start(
                    Path.Combine(dotnetDir, "dotnet.exe"),
                    [Path.Combine(exeDir, "SS15.Launcher.dll")]);

                process.WaitForExit();
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }

        [LibraryImport("KERNEL32.dll")]
        private static partial int AllocConsole();
    }
}
