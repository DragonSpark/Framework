using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration.Json;

namespace DragonSpark.Application.Mobile.Configuration;

sealed class RemoteConfigurationProvider : JsonStreamConfigurationProvider
{
    readonly Uri _address;

    public RemoteConfigurationProvider(Uri address) : base(new JsonStreamConfigurationSource())
        => _address = address;

    public override void Load()
    {
        using var client = new HttpClient();
        var       result = client.GetAsync(_address).GetAwaiter().GetResult();
        result.EnsureSuccessStatusCode();
        using var stream = result.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        base.Load(stream);
    }
}