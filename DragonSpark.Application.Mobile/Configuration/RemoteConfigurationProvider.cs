using System;
using System.Net;
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
        var instance = _message();
        try
        {
            var message = await instance.Get(CancellationToken.None).Off();
            message.EnsureSuccessStatusCode();
            switch (message.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                default:
                    var stream = await message.Content.ReadAsStreamAsync().Off();
                    base.Load(stream);
                    break;
            }
        }
        catch (Exception e)
        {
            var exception = new InvalidOperationException("Could not load Remote Configuration.  This is considered a critical exception.", e);
            await instance.Get(new(exception, CancellationToken.None)).Off();
            throw exception;
        }
    }
}