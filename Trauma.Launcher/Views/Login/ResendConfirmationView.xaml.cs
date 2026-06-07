using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Trauma.Launcher.ViewModels.Login;

namespace Trauma.Launcher.Views.Login;

public sealed partial class ResendConfirmationView : UserControl
{
    public ResendConfirmationView()
    {
        InitializeComponent();

        EmailBox.KeyDown += InputBoxOnKeyDown;
    }

    private void InputBoxOnKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter && DataContext is ResendConfirmationViewModel vm)
        {
            vm.SubmitPressed();
        }
    }
}
