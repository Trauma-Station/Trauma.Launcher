using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using SS15.Launcher.Utility;

namespace SS15.Launcher.Models.EngineManager;

public sealed partial class EngineManagerDynamic
{
    // This part of the code is responsible for downloading and caching engine build manifests.

    private readonly SemaphoreSlim _manifestSemaphore = new(1);
    private readonly Stopwatch _manifestStopwatch = Stopwatch.StartNew();

    private Dictionary<string, EngineCache> _cachedEngines = new();

    private void InitManifest()
    {
        // TODO: load custom engines
        foreach (var id in ConfigConstants.BuiltinEngines)
        {
            _cachedEngines[id] = new(id);
        }
    }

    /// <summary>
    /// Look up information about an engine version.
    /// </summary>
    /// <param name="id">The engine ID to use.</param>
    /// <param name="version">The version number to look up.</param>
    /// <param name="followRedirects">Follow redirections in version info.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>
    /// Information about the version, or null if it could not be found.
    /// The returned version may be different than what was requested if redirects were followed.
    /// </returns>
    private async ValueTask<FoundVersionInfo?> GetVersionInfo(
        string id,
        string version,
        bool followRedirects = true,
        CancellationToken cancel = default)
    {
        await _manifestSemaphore.WaitAsync(cancel);
        try
        {
            return await GetVersionInfoCore(id, version, followRedirects, cancel);
        }
        finally
        {
            _manifestSemaphore.Release();
        }
    }

    private async ValueTask<FoundVersionInfo?> GetVersionInfoCore(
        string id,
        string version,
        bool followRedirects,
        CancellationToken cancel)
    {
        if (!_cachedEngines.TryGetValue(id, out var engine))
        {
            Log.Error("Tried to get version of unknown engine {id}", id);
            return null;
        }

        // If we have a cached copy, and it's not expired, we check it.
        if (engine.VersionInfo != null && engine.ValidUntil > _manifestStopwatch.Elapsed)
        {
            // Check the version. If this fails, we immediately re-request the manifest as it may have changed.
            // (Connecting to a freshly-updated server with a new Robust version, within the cache window.)
            if (engine.FindVersionInfoInCached(version, followRedirects) is { } foundVersionInfo)
                return foundVersionInfo;
        }

        await UpdateBuildManifest(engine, cancel);

        return engine.FindVersionInfoInCached(version, followRedirects);
    }

    private async Task UpdateBuildManifest(EngineCache engine, CancellationToken cancel)
    {
        // TODO: If-Modified-Since and If-None-Match request conditions.

        var url = engine.ManifestUrl;
        Log.Debug("Loading manifest from {manifestUrl}...", url);
        engine.VersionInfo = await url.GetFromJsonAsync<Dictionary<string, VersionInfo>>(_http, cancel);
        engine.ValidUntil = _manifestStopwatch.Elapsed + ConfigConstants.EngineManifestCacheTime;
    }

    private record struct FoundVersionInfo(string Version, VersionInfo Info);

    private sealed record VersionInfo(
        bool Insecure,
        [property: JsonPropertyName("redirect")]
        string? RedirectVersion,
        Dictionary<string, BuildInfo> Platforms);

    private sealed class BuildInfo
    {
        [JsonInclude] [JsonPropertyName("url")]
        public string Url = default!;

        [JsonInclude] [JsonPropertyName("sha256")]
        public string Sha256 = default!;

        [JsonInclude] [JsonPropertyName("sig")]
        public string Signature = default!;
    }

    private sealed class EngineCache(string id)
    {
        public UrlFallbackSet ManifestUrl = ConfigConstants.EngineBuildsManifest(id);
        public Dictionary<string, VersionInfo>? VersionInfo;
        public TimeSpan ValidUntil;

        public FoundVersionInfo? FindVersionInfoInCached(string version, bool followRedirects)
        {
            Debug.Assert(VersionInfo != null);

            if (!VersionInfo.TryGetValue(version, out var info))
                return null;

            if (followRedirects)
            {
                while (info.RedirectVersion != null)
                {
                    version = info.RedirectVersion;
                    info = VersionInfo[info.RedirectVersion];
                }
            }

            return new FoundVersionInfo(version, info);
        }
    }
}
