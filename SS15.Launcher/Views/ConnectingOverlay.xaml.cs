using Avalonia.Threading;
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
    }
}
