using Trauma.Launcher.Api;
using Trauma.Launcher.Models.Data;
using Trauma.Launcher.Models.Logins;

namespace Trauma.Launcher.ViewModels.Login;

public sealed partial class ExpiredLoginViewModel : BaseLoginViewModel
{
    private readonly DataManager _cfg;
    private readonly AuthApi _authApi;
    private readonly LoginManager _loginMgr;

    public ExpiredLoginViewModel(
        MainWindowLoginViewModel parentVm,
        DataManager cfg,
        AuthApi authApi,
        LoginManager loginMgr,
        LoggedInAccount account)
    : base(parentVm)
    {
        _cfg = cfg;
        _authApi = authApi;
        _loginMgr = loginMgr;
        Account = account;
    }

    [Reactive] public string _editingPassword = "";
    public LoggedInAccount Account { get; }

    public async void OnLogInButtonPressed()
    {
        if (Busy)
            return;

        if (_cfg.GetAuthServer(Account.AuthServer) is not { } server)
            return; // how did you manage to get here

        Busy = true;
        try
        {
            var request = new AuthApi.AuthenticateRequest(Account.UserId, EditingPassword);
            var resp = await _authApi.AuthenticateAsync(server, request);

            await LoginViewModel.DoLogin(this, request, resp, _loginMgr, _authApi, server);

            _cfg.CommitConfig();
        }
        finally
        {
            Busy = false;
        }
    }

    public void OnLogOutButtonPressed()
    {
        _cfg.RemoveLogin(Account.LoginInfo);
        _cfg.CommitConfig();

        ParentVM.SwitchToLogin();
    }

    public void SwitchToForgotPassword()
    {
        if (_cfg.GetAuthServer(Account.AuthServer) is not { } server)
            return; // how did you manage to get here

        ParentVM.SwitchToForgotPassword(server);
    }
}
