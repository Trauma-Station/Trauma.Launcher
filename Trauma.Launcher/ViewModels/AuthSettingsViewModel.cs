using System.Collections.ObjectModel;
using Splat;
using Trauma.Launcher.Models.Data;
using Trauma.Launcher.Utility;
using static Trauma.Launcher.ViewModels.HubSettingsViewModel;

namespace Trauma.Launcher.ViewModels;

public sealed class AuthSettingsViewModel : ViewModelBase
{
    public ObservableCollection<AuthServerViewModel> ServerList { get; set; } = new();

    private DataManager _data = Locator.Current.GetRequiredService<DataManager>();

    public void Populate()
    {
        ServerList.Clear();
        foreach (var server in _data.AuthServers)
        {
            ServerList.Add(new(new(server), this));
        }
    }

    public void Save()
    {
        var servers = new List<AuthServer>(ServerList.Count);
        foreach (var model in ServerList)
        {
            servers.Add(new(model.ServerName.Trim(), EnsureSlash(model.AccountBaseUrl), EnsureSlash(model.AuthUrl)));
        }
        _data.SetAuthServers(servers);
    }

    public void Clear()
    {
        ServerList.Clear();
    }

    public void Add()
    {
        var server = new AuthServer("", "", "");
        ServerList.Add(new AuthServerViewModel(server, this));
    }

    private static string EnsureSlash(string url)
        => url.EndsWith('/')
            ? url
            : url + '/';
}

public sealed class AuthServerViewModel(AuthSettingsViewModel parent) : ViewModelBase
{
    private readonly AuthSettingsViewModel _parent = parent;

    public string ServerName { get; set; } = "";
    public string AccountBaseUrl { get; set; } = "";
    public string AuthUrl { get; set; } = "";

    public AuthServerViewModel(AuthServer server, AuthSettingsViewModel parent) : this(parent)
    {
        ServerName = server.Name;
        AccountBaseUrl = server.AccountBaseUrl;
        AuthUrl = server.AuthUrl;
    }

    public void Remove()
    {
        _parent.ServerList.Remove(this);
    }

    public bool IsValid
        => !string.IsNullOrEmpty(ServerName.Trim()) &&
            IsValidHubUri(AccountBaseUrl) &&
            IsValidHubUri(AuthUrl);
}
