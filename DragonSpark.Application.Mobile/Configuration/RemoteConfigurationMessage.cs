using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Configuration;

public sealed class RemoteConfigurationMessage : IRemoteConfigurationMessage
{
    readonly Uri _address;

    public RemoteConfigurationMessage(RemoteConfigurationSettings settings) : this(new Uri(settings.Address)) {}

    public RemoteConfigurationMessage(Uri address) => _address = address;

    public async ValueTask<HttpResponseMessage> Get(CancellationToken parameter)
    {
        using var client = new HttpClient();
        var       result = await client.GetAsync(_address, parameter).Off();
        return result;
    }
}