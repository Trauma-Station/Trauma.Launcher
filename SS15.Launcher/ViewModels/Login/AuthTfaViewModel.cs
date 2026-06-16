using SS15.Launcher.Api;
using SS15.Launcher.Models.Data;
using SS15.Launcher.Models.Logins;

namespace SS15.Launcher.ViewModels.Login;

public sealed partial class AuthTfaViewModel : BaseLoginViewModel
{
    private readonly AuthApi.AuthenticateRequest _request;
    private readonly LoginManager _loginMgr;
    private readonly AuthApi _authApi;
    private readonly DataManager _cfg;
    private readonly AuthServer _server;

    [Reactive] public string _code = "";

    [Reactive] public partial bool IsInputValid { get; private set; }

    public AuthTfaViewModel(
        MainWindowLoginViewModel parentVm,
        AuthApi.AuthenticateRequest request,
        LoginManager loginMgr,
        AuthApi authApi,
        DataManager cfg,
        AuthServer server) : base(parentVm)
    {
        _request = request;
        _loginMgr = loginMgr;
        _authApi = authApi;
        _cfg = cfg;
        _server = server;

        this.WhenAnyValue(x => x.Code)
            .Subscribe(s => { IsInputValid = CheckInputValid(s); });
    }

    private static bool CheckInputValid(string code)
    {
        var trimmed = code.AsSpan().Trim();
        if (trimmed.Length != 6)
            return false;

        foreach (var chr in trimmed)
        {
            if (!char.IsDigit(chr))
                return false;
        }

        return true;
    }

    public async void ConfirmTfa()
    {
        if (Busy)
            return;

        var tfaLogin = _request with { TfaCode = Code.Trim() };

        Busy = true;
        try
        {
            var resp = await _authApi.AuthenticateAsync(_server, tfaLogin);

            await LoginViewModel.DoLogin(this, tfaLogin, resp, _loginMgr, _authApi, _server);

            _cfg.CommitConfig();
        }
        finally
        {
            Busy = false;
        }
    }

    public void RecoveryCode()
    {
        // I don't want to implement recovery code stuff, so if you need them,
        // bloody use them to disable your authenticator app online.
        Helpers.OpenUri(_server.ManagementUrl);
    }

    public void Cancel()
    {
        ParentVM.SwitchToLogin();
    }
}
