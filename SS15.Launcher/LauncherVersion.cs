namespace Trauma.Launcher;

public static class LauncherVersion
{
    public const string Name = "SS15.Launcher";
    public static Version? Version => typeof(LauncherVersion).Assembly.GetName().Version;
}
