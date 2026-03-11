using System;
using System.Net.Http;
using System.Net.Http.Headers;
using DragonSpark.Model.Commands;

namespace DragonSpark.Grok.Chat;

sealed class ConfigureClient : ICommand<HttpClient>
{
    readonly Uri                       _location;
    readonly AuthenticationHeaderValue _bearer;

    public ConfigureClient(GrokApiSettings settings) : this(settings.Location, new("Bearer", settings.Key)) {}

    public ConfigureClient(Uri location, AuthenticationHeaderValue bearer)
    {
        _location = location;
        _bearer   = bearer;
    }

    public void Execute(HttpClient parameter)
    {
        parameter.BaseAddress                         = _location;
        parameter.Timeout                             = TimeSpan.FromSeconds(100);
        parameter.DefaultRequestHeaders.Authorization = _bearer;
    }
}