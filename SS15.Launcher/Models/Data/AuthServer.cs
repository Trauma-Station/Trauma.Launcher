using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SS15.Launcher.Models.Data;

/// <summary>
/// A centralized auth server the user can log in to.
/// </summary>
public sealed record AuthServer(string Name, string AccountBaseUrl, string AuthUrl)
{
    public string ManagementUrl => $"{AccountBaseUrl}Manage";
    public string RegisterUrl => $"{AccountBaseUrl}Register";
    public string ResendConfirmationUrl => $"{AccountBaseUrl}ResendEmailConfirmation";

    /// <summary>
    /// Create a copy of another auth server
    /// </summary>
    public AuthServer(AuthServer other)
    {
        Name = other.Name;
        AccountBaseUrl = other.AccountBaseUrl;
        AuthUrl = other.AuthUrl;
    }

    public HttpRequestMessage HttpMessage(string api, HttpMethod method)
        => new HttpRequestMessage(method, AuthUrl + api);

    public HttpRequestMessage AuthenticatedMessage(string api, string token)
    {
        var message = HttpMessage(api, HttpMethod.Get);
        message.Headers.Authorization = new("SS14Auth", token);
        return message;
    }

    /// <summary>
    /// Send a POST request to an unauthenticated API with some json content.
    /// </summary>
    public Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient http, string api, T data)
    {
        var message = HttpMessage(api, HttpMethod.Post);
        message.Content = JsonContent.Create(data);
        return http.SendAsync(message);
    }
}
