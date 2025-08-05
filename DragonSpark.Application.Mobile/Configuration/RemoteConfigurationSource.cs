using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Configuration;

public sealed class RemoteConfigurationSource : IConfigurationSource
{
    readonly string _address;

    public RemoteConfigurationSource(string address) => _address = address;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new RemoteConfigurationProvider(_address);
}