using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration.Json;

namespace DragonSpark.Application.Mobile.Configuration;

sealed class RemoteConfigurationProvider : JsonStreamConfigurationProvider
{
    readonly Func<IRemoteConfigurationMessage> _message;
    readonly ICommand<Task>                    _register;

    public RemoteConfigurationProvider(Func<IRemoteConfigurationMessage> message)
        : this(message, RegisterInitialization.Default) {}

    public RemoteConfigurationProvider(Func<IRemoteConfigurationMessage> message, ICommand<Task> register)
        : base(new JsonStreamConfigurationSource())
    {
        _message  = message;
        _register = register;
    }

    public override void Load()
    {
        _register.Execute(LoadStream());
    }

    async Task LoadStream()
    {
        var message = _message();
        try
        {
            var response = await message.Get(CancellationToken.None).Off();
            response.EnsureSuccessStatusCode();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent: // TODO: Throw
                    break;
                default:
                    var stream = await response.Content.ReadAsStreamAsync().Off();
                    base.Load(stream);
                    break;
            }
        }
        catch (Exception e)
        {
            var exception =
                new
                    InvalidOperationException("Could not load Remote Configuration.  This is considered a critical exception.",
                                              e);
            await message.Get(new(exception, CancellationToken.None)).Off();
            throw exception;
        }
    }
}