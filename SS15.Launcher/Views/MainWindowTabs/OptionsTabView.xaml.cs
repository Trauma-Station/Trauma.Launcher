using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Splat;
using SS15.Launcher.Localization;
using SS15.Launcher.Utility;
using SS15.Launcher.ViewModels.MainWindowTabs;

namespace SS15.Launcher.Views.MainWindowTabs;

public partial class OptionsTabView : UserControl
{
    public OptionsTabView()
    {
        InitializeComponent();

        Flip.Command = ReactiveCommand.Create(() =>
        {
            if (TopLevel.GetTopLevel(this) is not Window window)
                return;

            window.Classes.Add("DoAFlip");

            DispatcherTimer.RunOnce(() => { window.Classes.Remove("DoAFlip"); }, TimeSpan.FromSeconds(1));
        });
    }

    public async void ClearEnginesPressed(object? _1, RoutedEventArgs _2)
    {
        ((OptionsTabViewModel)DataContext!).ClearEngines();
        await ClearEnginesButton.DisplayDoneMessage();
    }

    public async void ClearServerContentPressed(object? _1, RoutedEventArgs _2)
    {
        var blocked = !await ((OptionsTabViewModel)DataContext!).ClearServerContent();
        var locMgr = Locator.Current.GetService<LocalizationManager>()!;

        await ClearServerContentButton.DisplayDoneMessage(
            blocked ? locMgr.GetString("tab-options-clear-content-close-client") : null);
    }

    private async void OpenHubSettings(object? sender, RoutedEventArgs args)
    {
        await new HubSettingsDialog().ShowDialog((Window)TopLevel.GetTopLevel(this)!);
    }

    private async void OpenAuthSettings(object? sender, RoutedEventArgs args)
    {
        await new AuthSettingsDialog().ShowDialog((Window)TopLevel.GetTopLevel(this)!);
    }
}
