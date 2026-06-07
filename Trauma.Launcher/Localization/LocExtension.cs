using System;
using Splat;

namespace Trauma.Launcher.Localization;

public sealed class LocExtension
{
    public string Key { get; }

    public LocExtension(string key)
    {
        Key = key;
    }

    public object ProvideValue(IServiceProvider services)
    {
        var locMgr = Locator.Current.GetService<LocalizationManager>()!;
        return locMgr.GetString(Key);
    }
}
