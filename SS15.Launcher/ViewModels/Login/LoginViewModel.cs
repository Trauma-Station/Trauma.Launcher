using System.Threading.Tasks;
using Trauma.Launcher.Api;
using Trauma.Launcher.Localization;
using Trauma.Launcher.Models.Data;
using Trauma.Launcher.Models.Logins;

namespace Trauma.Launcher.ViewModels.Login;

public sealed partial class LoginViewModel : BaseLoginViewModel
{
    private readonly AuthApi _authApi;
    private readonly LoginManager _loginMgr;
    [Reactive] public partial DataManager Cfg { get; private set; }
    private readonly LocalizationManager _loc = LocalizationManager.Instance;

    [Reactive] public AuthServer _server = ConfigConstants.DefaultAuthServers[0];
    [Reactive] public string _editingUsername = "";
    [Reactive] public string _editingPassword = "";

    [Reactive] public partial bool IsInputValid { get; private set; }
    [Reactive] public bool _isPasswordVisible;

    public LoginViewModel(MainWindowLoginViewModel parentVm, AuthApi authApi,
        LoginManager loginMgr, DataManager cfg) : base(parentVm)
    {
        BusyText = _loc.GetString("login-login-busy-logging-in");
        _authApi = authApi;
        _loginMgr = loginMgr;
        Cfg = cfg;

        this.WhenAnyValue(x => x.EditingUsername, x => x.EditingPassword)
            .Subscribe(s => { IsInputValid = !string.IsNullOrEmpty(s.Item1) && !string.IsNullOrEmpty(s.Item2); });
    }

    public async void OnLogInButtonPressed()
    {
        if (!IsInputValid || Busy)
        {
            return;
        }

        Busy = true;
        try
        {
            var request = new AuthApi.AuthenticateRequest(EditingUsername, EditingPassword);
            var resp = await _authApi.AuthenticateAsync(Server, request);

            await DoLogin(this, request, resp, _loginMgr, _authApi, Server);

            Cfg.CommitConfig();
        }
        finally
        {
            Busy = false;
        }
    }

    public static async Task<bool> DoLogin<T>(
        T vm,
        AuthApi.AuthenticateRequest request,
        AuthenticateResult resp,
        LoginManager loginMgr,
        AuthApi authApi,
        AuthServer server)
        where T : BaseLoginViewModel, IErrorOverlayOwner
    {
        var loc = LocalizationManager.Instance;
        if (resp.IsSuccess)
        {
            var loginInfo = resp.LoginInfo;
            var oldLogin = loginMgr.Logins.Lookup((loginInfo.AuthServer, loginInfo.UserId));
            if (oldLogin.HasValue)
            {
                // Already had this login, apparently.
                // Thanks user.
                //
                // Log the OLD token out since we don't need two of them.
                // This also has the upside of re-available-ing the account
                // if the user used the main login prompt on an account we already had, but as expired.

                await authApi.LogoutTokenAsync(server, oldLogin.Value.LoginInfo.Token.Token);
                loginMgr.ActiveAccountId = (server.Name, loginInfo.UserId);
                loginMgr.UpdateToNewToken(loginMgr.ActiveAccount!, loginInfo.Token);
                return true;
            }

            loginMgr.AddFreshLogin(loginInfo);
            loginMgr.ActiveAccountId = (server.Name, loginInfo.UserId);
            return true;
        }

        if (resp.Code == AuthApi.AuthenticateDenyResponseCode.TfaRequired)
        {
            vm.ParentVM.SwitchToAuthTfa(server, request);
            return false;
        }

        var errors = AuthErrorsOverlayViewModel.AuthCodeToErrors(resp.Errors, resp.Code);
        vm.OverlayControl = new AuthErrorsOverlayViewModel(vm, loc.GetString("login-login-error-title"), errors);
        return false;
    }

    public void RegisterPressed()
    {
        // Registration is purely via website now, sorry.
        Helpers.OpenUri(_server.RegisterUrl);
    }

    public void SwitchToForgotPassword()
    {
        ParentVM.SwitchToForgotPassword(_server);
    }

    public void ResendConfirmationPressed()
    {
        // Registration is purely via website now, sorry.
        Helpers.OpenUri(_server.ResendConfirmationUrl);
    }
}
