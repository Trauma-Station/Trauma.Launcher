using SS15.Launcher.Api;
using SS15.Launcher.Localization;
using SS15.Launcher.Models.Data;

namespace SS15.Launcher.ViewModels.Login;

public sealed partial class ForgotPasswordViewModel : BaseLoginViewModel
{
    private readonly AuthApi _authApi;
    private readonly AuthServer _server;
    private readonly LocalizationManager _loc = LocalizationManager.Instance;

    [Reactive] public string _editingEmail = "";

    private bool _errored;

    public ForgotPasswordViewModel(
        MainWindowLoginViewModel parentVM,
        AuthApi authApi,
        AuthServer server)
        : base(parentVM)
    {
        _authApi = authApi;
        _server = server;
    }

    public async void SubmitPressed()
    {
        if (Busy)
            return;

        Busy = true;
        try
        {
            BusyText = "Sending email...";
            var errors = await _authApi.ForgotPasswordAsync(_server, EditingEmail);

            _errored = errors != null;

            if (!_errored)
            {
                // This isn't an error lol but that's what I called the control.
                OverlayControl = new AuthErrorsOverlayViewModel(this, _loc.GetString("login-forgot-success-title"), new[]
                {
                    _loc.GetString("login-forgot-success-message")
                });
            }
            else
            {
                OverlayControl = new AuthErrorsOverlayViewModel(this, _loc.GetString("login-forgot-error"), errors!);
            }
        }
        finally
        {
            Busy = false;
        }
    }

    public override void OverlayOk()
    {
        if (_errored)
        {
            base.OverlayOk();
        }
        else
        {
            // If the overlay was a success overlay, switch back to login.
            ParentVM.SwitchToLogin();
        }
    }
}
