using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Trauma.Launcher.ViewModels.Login;

namespace Trauma.Launcher.Views.Login;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();

        NameBox.KeyDown += InputBoxOnKeyDown;
        PasswordBox.KeyDown += InputBoxOnKeyDown;
    }

    private void InputBoxOnKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter && DataContext is LoginViewModel vm)
        {
            vm.OnLogInButtonPressed();
        }
    }
}
