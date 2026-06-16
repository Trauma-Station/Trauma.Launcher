using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Trauma.Launcher.ViewModels.Login;

namespace Trauma.Launcher.Views.Login;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();

        NameBox.KeyDown += OnTextBoxKeyDown;
        EmailBox.KeyDown += OnTextBoxKeyDown;
        PasswordBox.KeyDown += OnTextBoxKeyDown;
        PasswordConfirmBox.KeyDown += OnTextBoxKeyDown;

        this.WhenAnyValue(v => v.DataContext)
            .Subscribe(vm => ((RegisterViewModel) vm!).WhenAnyValue(vm => vm.Cfg)
                .Subscribe(cfg => AuthServerSelector.Update(cfg)));
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter && DataContext is RegisterViewModel vm)
        {
            vm.OnRegisterInButtonPressed();
        }
    }
}
