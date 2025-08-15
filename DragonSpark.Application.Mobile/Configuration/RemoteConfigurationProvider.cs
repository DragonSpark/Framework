using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Configuration.Json;

namespace DragonSpark.Application.Mobile.Configuration;

sealed class RemoteConfigurationProvider : JsonStreamConfigurationProvider
{
    readonly Func<IRemoteConfigurationMessage> _message;

    public RemoteConfigurationProvider(Func<IRemoteConfigurationMessage> message)
        : base(new JsonStreamConfigurationSource())
        => _message = message;

    public override void Load()
    {
        LoadStream().GetAwaiter().GetResult();
    }

    async Task LoadStream()
    {
        var message = await _message().Off(CancellationToken.None);
        message.EnsureSuccessStatusCode();
        var stream = await message.Content.ReadAsStreamAsync().Off();
        base.Load(stream);
    }
    
}