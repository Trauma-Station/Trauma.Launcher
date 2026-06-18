using System.Data;
using Microsoft.Data.Sqlite;

namespace SS15.Launcher.Models.Data.Migrations;

public sealed class Script0002_ContentDB : Migrator.IMigrationScript
{
    public string Up(SqliteConnection connection)
    {
        if (Directory.Exists(LauncherPaths.DirServerContent))
            Directory.Delete(LauncherPaths.DirServerContent, true);

        return @"
DROP TABLE ServerContent;

DELETE FROM Config WHERE Key='NextInstallationId';
";
    }
}
