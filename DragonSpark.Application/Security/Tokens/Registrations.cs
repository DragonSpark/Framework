using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Tokens;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<ApplyProof>()
                 .And<ProcessResponse>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<ITokens>()
                 .Forward<InMemoryTokens>()
                 .Singleton()
                 //
                 .Then.Start<DevicePoPHandler>()
                 .Scoped();
    }
}

// --------------- Server nonce cache (per-origin) for retries ---------------

// --------------- Passkey Flow (client side) ---------------
/*
public sealed class PasskeyClient
{
    readonly HttpClient _deviceApi; // configured with DevicePoPHandler

    public PasskeyClient(HttpClient deviceApi)
    {
        _deviceApi = deviceApi;
    }

    // 1) Begin-passkey: request a ticket JWT + url to open system browser
    public async Task<(string authorizeUrl, string ticketJwt)> BeginPasskeyAsync(CancellationToken ct)
    {
        var req  = new HttpRequestMessage(HttpMethod.Post, "/api/begin-passkey");
        var resp = await _deviceApi.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        using var s      = await resp.Content.ReadAsStreamAsync(ct);
        using var doc    = await JsonDocument.ParseAsync(s, cancellationToken: ct);
        var       url    = doc.RootElement.GetProperty("authorizeUrl").GetString()!;
        var       ticket = doc.RootElement.GetProperty("ticket").GetString()!;
        return (url, ticket);
    }

    // 2) Open system browser with the URL (including ticket as query param)
    public Task OpenSystemBrowserAsync(string authorizeUrl)
    {
        // MAUI: use Launcher.OpenAsync
        return Microsoft.Maui.ApplicationModel.Launcher.OpenAsync(new Uri(authorizeUrl));
    }

    // 3) Handle deep link back to app: you’ll get a JWE from your redirect URI
    //    Then finish passkey by POSTing that JWE to the server (DevicePoP protected).
    public async Task<(string accessToken, string refreshToken)> FinishPasskeyAsync(
        string jweFromDeepLink, CancellationToken ct)
    {
        var payload = JsonSerializer.Serialize(new { jwe = jweFromDeepLink });
        var req = new HttpRequestMessage(HttpMethod.Post, "/api/finish-passkey")
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };
        var resp = await _deviceApi.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        using var s   = await resp.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(s, cancellationToken: ct);
        var       at  = doc.RootElement.GetProperty("access_token").GetString()!;
        var       rt  = doc.RootElement.GetProperty("refresh_token").GetString()!;
        return (at, rt);
    }
}
*/