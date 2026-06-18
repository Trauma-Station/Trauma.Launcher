using Avalonia.Threading;
using SS15.Launcher.ViewModels;

namespace SS15.Launcher.Views;

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
