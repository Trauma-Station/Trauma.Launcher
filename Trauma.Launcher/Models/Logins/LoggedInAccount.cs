using System;
using ReactiveUI;
using Trauma.Launcher.Models.Data;

namespace Trauma.Launcher.Models.Logins;

public abstract class LoggedInAccount : ReactiveObject
{
    public string Username => LoginInfo.Username;
    public Guid UserId => LoginInfo.UserId;

    protected LoggedInAccount(LoginInfo loginInfo)
    {
        LoginInfo = loginInfo;
    }

    public LoginInfo LoginInfo { get; }

    public abstract AccountLoginStatus Status { get; }
}