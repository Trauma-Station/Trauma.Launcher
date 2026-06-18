using System.Diagnostics;
using System.Threading.Tasks;
using Splat;
using SS15.Launcher.Localization;
using SS15.Launcher.Models.ContentManagement;
using SS15.Launcher.Models.Data;
using SS15.Launcher.Models.EngineManager;
using SS15.Launcher.Utility;

namespace SS15.Launcher.ViewModels.MainWindowTabs;

public sealed class OptionsTabViewModel : MainWindowTabViewModel
{
    public DataManager Cfg { get; }
    private readonly IEngineManager _engineManager;
    private readonly ContentManager _contentManager;

    public LanguageSelectorViewModel Language { get; } = new();

    public OptionsTabViewModel()
    {
        Cfg = Locator.Current.GetRequiredService<DataManager>();
        _engineManager = Locator.Current.GetRequiredService<IEngineManager>();
        _contentManager = Locator.Current.GetRequiredService<ContentManager>();

        DisableIncompatibleMacOS = OperatingSystem.IsMacOS();
    }
    public bool DisableIncompatibleMacOS { get; }

    public override string Name => LocalizationManager.Instance.GetString("tab-options-title");

    public bool CompatMode
    {
        get => Cfg.GetCVar(CVars.CompatMode);
        set => Cfg.SetCVar(CVars.CompatMode, value, commit: true);
    }

    public bool LogLauncherVerbose
    {
        get => Cfg.GetCVar(CVars.LogLauncherVerbose);
        set => Cfg.SetCVar(CVars.LogLauncherVerbose, value, commit: true);
    }

    public bool OverrideAssets
    {
        get => Cfg.GetCVar(CVars.OverrideAssets);
        set => Cfg.SetCVar(CVars.OverrideAssets, value, commit: true);
    }

    public bool ShowBanner
    {
        get => Cfg.GetCVar(CVars.ShowBanner);
        set => Cfg.SetCVar(CVars.ShowBanner, value, commit: true);
    }

    public void ClearEngines()
    {
        _engineManager.ClearAllEngines();
    }

    public async Task<bool> ClearServerContent()
        => await _contentManager.ClearAll();

    public void OpenLogDirectory()
    {
        Process.Start(new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = LauncherPaths.DirLogs
        });
    }

    public void OpenAccountSettings()
    {
        if (Cfg.CurrentAuthServer is not { } server)
            return; // not logged in so no settings to manage

        Helpers.OpenUri(server.ManagementUrl);
    }
}
