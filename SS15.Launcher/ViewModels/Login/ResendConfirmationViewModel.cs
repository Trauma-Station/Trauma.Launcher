using SS15.Launcher.Api;
using SS15.Launcher.Models.Data;

namespace SS15.Launcher.ViewModels.Login;

public sealed partial class ResendConfirmationViewModel : BaseLoginViewModel
{
    private readonly AuthApi _authApi;
    private readonly AuthServer _server;

    [Reactive] public partial string EditingEmail { get; set; } = "";

    private bool _errored;

    public ResendConfirmationViewModel(MainWindowLoginViewModel parentVM, AuthApi authApi, AuthServer server) : base(parentVM)
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
            BusyText = "Resending email...";
            var errors = await _authApi.ResendConfirmationAsync(_server, EditingEmail);

            _errored = errors != null;

            if (!_errored)
            {
                // This isn't an error lol but that's what I called the control.
                OverlayControl = new AuthErrorsOverlayViewModel(this, "Confirmation email sent", new []
                {
                    "A confirmation email has been sent to your email address."
                });
            }
            else
            {
                OverlayControl = new AuthErrorsOverlayViewModel(this, "Error", errors!);
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
