using System;

namespace Trauma.Launcher.Utility;

public static class HubUtility
{
    public static string GetHubShortName(string hubAddress)
    {
        return Uri.TryCreate(hubAddress, UriKind.Absolute, out var uri)
            ? uri.Host
            : hubAddress;
    }
}
