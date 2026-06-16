using Trauma.Launcher.Models.Data;

namespace Trauma.Launcher.Models.Logins;

// TODO: make this even more abstract to support pubkey auth
public abstract class LoggedInAccount : ReactiveObject
{
    public string AuthServer => LoginInfo.AuthServer;
    public string Username => LoginInfo.Username;
    public Guid UserId => LoginInfo.UserId;

    protected LoggedInAccount(LoginInfo loginInfo)
    {
        LoginInfo = loginInfo;
    }

    public LoginInfo LoginInfo { get; }

    public abstract AccountLoginStatus Status { get; }
}
