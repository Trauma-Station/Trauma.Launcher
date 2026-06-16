using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SS15.Launcher.ViewModels.Login;

namespace SS15.Launcher.Views.Login;

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
