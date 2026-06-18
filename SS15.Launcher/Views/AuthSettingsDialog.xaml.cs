using Avalonia.Interactivity;
using SS15.Launcher.ViewModels;

namespace SS15.Launcher.Views;

public partial class AuthSettingsDialog : Window
{
    private readonly AuthSettingsViewModel _vm = new();

    public AuthSettingsDialog()
    {
        InitializeComponent();

        DataContext = _vm;
        _vm.ServerList.CollectionChanged += (_, _) => Verify();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        _vm.Populate();
        Verify(); // Just in case the settings are messed up somehow.
    }

    private void Save(object? sender, RoutedEventArgs args)
    {
        _vm.Save();
        Close();
    }

    private void Cancel(object? sender, RoutedEventArgs args) => Close();

    private void AuthServerChanged(object? sender, AvaloniaPropertyChangedEventArgs e) => Verify();

    private void Verify()
    {
        foreach (var server in _vm.ServerList)
        {
            if (!server.IsValid)
            {
                SaveButton.IsEnabled = false;
                return;
            }
        }

        SaveButton.IsEnabled = true;
    }
}
