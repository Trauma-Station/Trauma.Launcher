using Avalonia.Threading;
using Trauma.Launcher.Models.Data;
using Trauma.Launcher.ViewModels;

namespace Trauma.Launcher.Views;

public partial class ConnectingOverlay : UserControl
{
    public ConnectingOverlay()
    {
        InitializeComponent();

        ConnectingViewModel.StartedConnecting += () => Dispatcher.UIThread.Post(() =>
        {
            CancelButton.Focus();
            Messages.Refresh();
        });

        ConnectingViewModel.OnShowAuthServerWarning += (data, servers) =>
        {
            ShowAuthServerWarning(data, servers);
        };
    }

    private async void ShowAuthServerWarning(DataManager data, string[] servers)
    {
        await new AuthServerWarningDialog(data, servers).ShowDialog((Window)TopLevel.GetTopLevel(this)!);
    }
}
