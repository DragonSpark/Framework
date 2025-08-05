using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration.Json;

namespace DragonSpark.Application.Mobile.Configuration;

public sealed class RemoteConfigurationProvider : JsonStreamConfigurationProvider
{
    readonly string _address;

    public RemoteConfigurationProvider(string address) : base(new JsonStreamConfigurationSource())
        => _address = address;

    public override void Load()
    {
        using var httpClient = new HttpClient();
        var       result     = httpClient.GetAsync(_address).GetAwaiter().GetResult();

        if (!result.IsSuccessStatusCode)
        {
            throw new
                InvalidOperationException($"Failed to load configuration from {_address}. Status code: {result.StatusCode}");
        }

        using var stream = result.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        base.Load(stream);
    }
}