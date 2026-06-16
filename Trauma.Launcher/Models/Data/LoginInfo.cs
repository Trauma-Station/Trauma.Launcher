namespace Trauma.Launcher.Models.Data;

public sealed partial class LoginInfo : ReactiveObject
{
    [Reactive] public string authServer = "";
    [Reactive] public Guid _userId;
    [Reactive] public string _username = "";
    [Reactive] public LoginToken _token;

    public override string ToString() => $"{AuthServer}:{Username}/{UserId}";

    public bool Matches((string, Guid)? pair)
        => AuthServer == pair?.Item1 && UserId == pair?.Item2;
}
