namespace SS15.Launcher.Utility;

public static class HubUtility
{
    public static string GetHubShortName(string hubAddress)
        => Uri.TryCreate(hubAddress, UriKind.Absolute, out var uri)
            ? uri.Host
            : hubAddress;
}
