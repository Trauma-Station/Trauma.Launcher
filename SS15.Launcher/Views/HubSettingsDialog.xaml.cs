using System.Linq;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using SS15.Launcher.Localization;
using SS15.Launcher.ViewModels;
using static SS15.Launcher.ViewModels.HubSettingsViewModel;

namespace SS15.Launcher.Views;

public partial class HubSettingsDialog : Window
{
    private readonly HubSettingsViewModel _viewModel = new();
    private readonly LocalizationManager _loc = LocalizationManager.Instance;

    public HubSettingsDialog()
    {
        InitializeComponent();

        DataContext = _viewModel;
        _viewModel.HubList.CollectionChanged += (_, _) => Verify();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        _viewModel.Populate();
        Verify(); // Just in case the settings are messed up somehow.
    }

    private void Done(object? sender, RoutedEventArgs args)
    {
        _viewModel.Save();
        Close();
    }

    private void Cancel(object? sender, RoutedEventArgs args) => Close();

    private void HubTextChanged(object? sender, AvaloniaPropertyChangedEventArgs e) => Verify();

    private void Verify()
    {
        var dupes = _viewModel.GetDupes();

        foreach (var t in Hubs.GetLogicalDescendants().OfType<TextBox>())
        {
            if (!IsValidHubUri(t.Text))
                t.Classes.Add("Invalid");
            else
                t.Classes.Remove("Invalid");

            if (dupes.Contains(NormalizeHubUri(t.Text ?? "")))
                t.Classes.Add("Duplicate");
            else
                t.Classes.Remove("Duplicate");
        }

        var allValid = _viewModel.HubList.All(h => IsValidHubUri(h.Address));
        var noDupes = !dupes.Any();

        DoneButton.IsEnabled = allValid && noDupes;

        if (!allValid)
            Warning.Text = _loc.GetString("hub-settings-warning-invalid");
        else if (!noDupes)
            Warning.Text = _loc.GetString("hub-settings-warning-duplicate");
        else
            Warning.Text = "";
    }
}
