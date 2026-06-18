using Avalonia.Input;
using SS15.Launcher.ViewModels.Login;

namespace SS15.Launcher.Views.Login;

public sealed partial class AuthTfaView : UserControl
{
    public AuthTfaView()
    {
        InitializeComponent();

        CodeBox.KeyDown += InputBoxOnKeyDown;
    }

    private void InputBoxOnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is AuthTfaViewModel vm && vm.IsInputValid)
        {
            vm.ConfirmTfa();
        }
    }
}
