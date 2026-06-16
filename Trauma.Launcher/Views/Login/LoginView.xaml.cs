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

        this.WhenAnyValue(v => v.DataContext)
            .Subscribe(vm => ((LoginViewModel) vm!).WhenAnyValue(vm => vm.Cfg)
                .Subscribe(cfg => AuthServerSelector.Update(cfg)));
    }

    private void InputBoxOnKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter && DataContext is LoginViewModel vm)
        {
            vm.OnLogInButtonPressed();
        }
    }
}
