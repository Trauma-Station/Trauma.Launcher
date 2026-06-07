using System;

namespace Trauma.Launcher;

public static class LauncherVersion
{
    public const string Name = "Trauma.Launcher";
    public static Version? Version => typeof(LauncherVersion).Assembly.GetName().Version;
}
