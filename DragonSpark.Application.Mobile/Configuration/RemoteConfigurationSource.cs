using System;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Configuration;

sealed class RemoteConfigurationSource : IConfigurationSource
{
    readonly Uri _address;

    public RemoteConfigurationSource(Uri address) => _address = address;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new RemoteConfigurationProvider(_address);
}