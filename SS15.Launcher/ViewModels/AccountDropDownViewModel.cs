using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using JetBrains.Annotations;
using Serilog;
using Splat;
using SS15.Launcher.Api;
using SS15.Launcher.Localization;
using SS15.Launcher.Models.Data;
using SS15.Launcher.Models.Logins;
using SS15.Launcher.Utility;

namespace SS15.Launcher.ViewModels;

public sealed partial class AccountDropDownViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainVm;
    private readonly DataManager _cfg;
    private readonly AuthApi _authApi;
    private readonly LoginManager _loginMgr;
    private readonly ReadOnlyObservableCollection<AvailableAccountViewModel> _accounts;
    private readonly LocalizationManager _loc;

    public ReadOnlyObservableCollection<AvailableAccountViewModel> Accounts => _accounts;

    public AccountDropDownViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
        _cfg = Locator.Current.GetRequiredService<DataManager>();
        _authApi = Locator.Current.GetRequiredService<AuthApi>();
        _loginMgr = Locator.Current.GetRequiredService<LoginManager>();
        _loc = LocalizationManager.Instance;

        this.WhenAnyValue(x => x._loginMgr.ActiveAccount)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(LoginText));
                this.RaisePropertyChanged(nameof(AccountSwitchText));
                this.RaisePropertyChanged(nameof(LogoutText));
                this.RaisePropertyChanged(nameof(AccountControlsVisible));
                this.RaisePropertyChanged(nameof(AccountSwitchVisible));
            });

        _loginMgr.Logins.Connect().Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(LogoutText));
            this.RaisePropertyChanged(nameof(AccountSwitchVisible));
        });

        var filterObservable = this.WhenAnyValue(x => x._loginMgr.ActiveAccount)
            .Select(MakeFilter);

        _loginMgr.Logins
            .Connect()
            .Filter(filterObservable)
            .Transform(p => new AvailableAccountViewModel(p))
            .Bind(out _accounts)
            .Subscribe();
    }

    private static Func<LoggedInAccount?, bool> MakeFilter(LoggedInAccount? selected)
    {
        return l => l != selected;
    }

    public string LoginText => _loginMgr.ActiveAccount is { } account
        ? $"{account.Username} [{account.AuthServer}]"
        : _loc.GetString("account-drop-down-none-selected");

    public string LogoutText => _cfg.Logins.Count == 1
        ? _loc.GetString("account-drop-down-log-out")
        : _loc.GetString("account-drop-down-log-out-of", ("name", _loginMgr.ActiveAccount?.Username));

    public bool AccountSwitchVisible => _cfg.Logins.Count > 1 || _loginMgr.ActiveAccount == null;
    public string AccountSwitchText => _loginMgr.ActiveAccount != null
        ? _loc.GetString("account-drop-down-switch-account")
        : _loc.GetString("account-drop-down-select-account");

    public bool AccountControlsVisible => _loginMgr.ActiveAccount != null;

    [Reactive] public bool _isDropDownOpen;

    public async void LogoutPressed()
    {
        IsDropDownOpen = false;

        if (_loginMgr.ActiveAccount is { } account)
        {
            if (_cfg.GetAuthServer(account.AuthServer) is { } server)
                await _authApi.LogoutTokenAsync(server, account.LoginInfo.Token.Token);
            _cfg.RemoveLogin(account.LoginInfo);
        }
    }

    [UsedImplicitly]
    public void AccountButtonPressed(object account)
    {
        if (account is not LoggedInAccount loggedInAccount)
        {
            Log.Warning($"Tried to switch account but parameter was not of type {nameof(LoggedInAccount)}");
            return;
        }

        IsDropDownOpen = false;
        _mainVm.TrySwitchToAccount(loggedInAccount);
    }

    public void AddAccountPressed()
    {
        IsDropDownOpen = false;

        _loginMgr.ActiveAccount = null;
    }
}

public sealed partial class AvailableAccountViewModel : ViewModelBase
{
    [ObservableAsProperty]
    private string _statusText = "!!!";

    public LoggedInAccount Account { get; }

    public AvailableAccountViewModel(LoggedInAccount account)
    {
        Account = account;

        _statusTextHelper = this.WhenAnyValue<AvailableAccountViewModel, AccountLoginStatus, string, string>(p => p.Account.Status, p => p.Account.Username, p => p.Account.AuthServer)
            .Select(p => p.Item1 switch
            {
                AccountLoginStatus.Available => $"{p.Item2} [{p.Item3}]",
                AccountLoginStatus.Expired => $"{p.Item2} (!) [{p.Item3}]",
                _ => $"{p.Item2} (?) [{p.Item3}]"
            })
            .ToProperty(this, x => x.StatusText);
    }
}
