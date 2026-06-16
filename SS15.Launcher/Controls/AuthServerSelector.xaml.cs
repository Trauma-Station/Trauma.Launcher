using Avalonia.Data;
using Avalonia.Interactivity;
using SS15.Launcher.Models.Data;
using SS15.Launcher.Views;

namespace SS15.Launcher.Controls;

public sealed partial class AuthServerSelector : UserControl
{
    public static readonly StyledProperty<AuthServer> ServerProperty =
        AvaloniaProperty.Register<AuthServerSelector, AuthServer>(
            nameof(Server),
            defaultValue: ConfigConstants.DefaultAuthServers[0],
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// The selected auth server.
    /// </summary>
    public AuthServer Server
    {
        get => GetValue(ServerProperty);
        set => SetValue(ServerProperty, value);
    }

    public AuthServerSelector()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Update the list of auth servers.
    /// </summary>
    public void Update(DataManager data)
    {
        ServerList.Children.Clear();
        foreach (var server in data.AllAuthServers())
        {
            var copy = server; // awesome language :D
            var button = new RadioButton()
            {
                GroupName = "AuthServers",
                Content = server.Name,
                IsChecked = server == Server
            };
            ToolTip.SetTip(button, server.AuthUrl);
            button.IsCheckedChanged += (_, _) =>
            {
                if (button.IsChecked == true)
                    Server = copy;
            };
            ServerList.Children.Add(button);
        }
    }

    public void OpenChangeServersWindow(object? sender, RoutedEventArgs args)
    {
        new AuthSettingsDialog().ShowDialog((Window)TopLevel.GetTopLevel(this)!);
    }
}
