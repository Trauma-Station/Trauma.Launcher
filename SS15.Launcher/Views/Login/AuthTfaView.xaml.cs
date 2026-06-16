using Avalonia.Input;
using Trauma.Launcher.ViewModels.Login;

namespace Trauma.Launcher.Views.Login;

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
