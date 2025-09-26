using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model;

namespace DragonSpark.Application.Mobile.Configuration;

public sealed class RemoteConfigurationMessage : RemoteConfigurationMessageBase, IRemoteConfigurationMessage
{
    readonly Uri _address;

    public RemoteConfigurationMessage(IExceptionLogger logger, RemoteConfigurationSettings settings)
        : this(logger, new Uri(settings.Address)) {}

    public RemoteConfigurationMessage(IExceptionLogger logger, Uri address) : base(logger) => _address = address;

    public async ValueTask<HttpResponseMessage> Get(CancellationToken parameter)
    {
        using var client = new HttpClient();
        var       result = await client.GetAsync(_address, parameter).Off();
        return result;
    }

    public bool Get(None parameter) => true;
}