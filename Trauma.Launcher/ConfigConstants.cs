using Trauma.Launcher.Models.Data;
using Trauma.Launcher.Utility;

namespace Trauma.Launcher;

public static class ConfigConstants
{
    public const string CurrentLauncherVersion = "trauma-0.3";
    public static readonly bool DoVersionCheck = true;

    // Refresh login tokens if they're within <this much> of expiry.
    public static readonly TimeSpan TokenRefreshThreshold = TimeSpan.FromDays(15);

    // If the user leaves the launcher running for absolute ages, this is how often we'll update his login tokens.
    public static readonly TimeSpan TokenRefreshInterval = TimeSpan.FromDays(7);

    // The amount of time before a server is considered timed out for status checks.
    public static readonly TimeSpan ServerStatusTimeout = TimeSpan.FromSeconds(5);

    // Check the command queue this often.
    public static readonly TimeSpan CommandQueueCheckInterval = TimeSpan.FromSeconds(1);

    public const string LauncherCommandsNamedPipeName = "Trauma.Launcher.CommandPipe";
    // Amount of time to wait before the launcher decides to ignore named pipes entirely to keep the rest of the launcher functional.
    public const int LauncherCommandsNamedPipeTimeout = 150;
    // Amount of time to wait to let a redialling client properly die
    public const int LauncherCommandsRedialWaitTimeout = 1000;

    // TODO: ed25519 auth
    public static readonly AuthServer[] DefaultAuthServers = [
        new("SS14", "https://account.spacestation14.com/Identity/Account/", "https://auth.spacestation14.com/"),
        new("Wizden", "https://account.playss14.com/Identity/Account/", "https://auth.playss14.com/")
    ];
    public static readonly UrlFallbackSet[] DefaultHubUrls = [
        new(["https://hub.spacestation14.com/", "https://hub.fallback.spacestation14.com/"]),
        new(["https://hub.playss14.com/"])
    ];
    public const string DiscordUrl = "https://discord.traumastation.com/";
    public const string WebsiteUrl = "https://wiki.traumastation.com";
    public const string DownloadUrl = "https://traumastation.com/download";
    public const string NewsFeedUrl = "https://news.traumastation.com/index.xml";

    private static readonly Dictionary<string, UrlFallbackSet> EngineBaseUrls = new()
    {
        {"RobustToolbox", new([
            "https://robust-builds.cdn.spacestation14.com/",
            "https://robust-builds.fallback.cdn.spacestation14.com/"
        ])},
        {"QuietToolbox", new([
            "https://engine.cdn.traumastation.com/"
        ])}
    };

    /// <summary>
    /// ID of every engine built in to the launcher.
    /// </summary>
    public static IEnumerable<string> BuiltinEngines => EngineBaseUrls.Keys;
    /// <summary>
    /// Engine to assume is used if one is not specified.
    /// </summary>
    public const string DefaultEngine = "RobustToolbox";

    private static readonly UrlFallbackSet LauncherDataBaseUrl = new([
        "https://launcher.cdn.traumastation.com/"
    ]);

    public static UrlFallbackSet EngineBuildsManifest(string id) => EngineBaseUrls[id] + "manifest.json";
    public static UrlFallbackSet EngineModulesManifest(string id) => EngineBaseUrls[id] + "modules.json";

    // How long to keep cached copies of engine manifests.
    // TODO: Take this from Cache-Control header responses instead.
    public static readonly TimeSpan EngineManifestCacheTime = TimeSpan.FromMinutes(15);

    public static readonly UrlFallbackSet UrlLauncherInfo = LauncherDataBaseUrl + "info.json";
    public static readonly UrlFallbackSet UrlAssetsBase = LauncherDataBaseUrl + "assets/";

    public const string FallbackUsername = "JoeGenero";
}
