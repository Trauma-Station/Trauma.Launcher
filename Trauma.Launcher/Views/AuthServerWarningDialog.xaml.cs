using Avalonia.Interactivity;
using Trauma.Launcher.Models.Data;

namespace Trauma.Launcher.Views;

public partial class AuthServerWarningDialog : Window
{
    public AuthServerWarningDialog()
    {
        InitializeComponent();
    }

    public AuthServerWarningDialog(DataManager data, string[] servers) : this()
    {
        foreach (var url in servers)
        {
            var name = data.GetAuthServerName(url) ?? "!!UNKNOWN!!";
            ServerList.Children.Add(new TextBlock()
            {
                Text = $"{url} [{name}]",
                Margin = new Thickness(4)
            });
        }
    }

    public void Close(object? sender, RoutedEventArgs args)
    {
        Close();
    }
}
