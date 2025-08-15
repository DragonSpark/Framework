using System;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Configuration;

sealed class RemoteConfigurationSource : IConfigurationSource
{
    readonly Func<IRemoteConfigurationMessage> _message;

    public RemoteConfigurationSource(Func<IRemoteConfigurationMessage> message) => _message = message;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new RemoteConfigurationProvider(_message);
}