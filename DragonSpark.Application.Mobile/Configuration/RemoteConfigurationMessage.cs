using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model;

namespace DragonSpark.Application.Mobile.Configuration;

public sealed class RemoteConfigurationMessage : RemoteConfigurationMessageBase, IRemoteConfigurationMessage
{
    readonly IHttpClientFactory _clients;
    readonly Uri                _address;

    public RemoteConfigurationMessage(IHttpClientFactory clients, IExceptionLogger logger,
                                      RemoteConfigurationSettings settings)
        : this(clients, logger, new Uri(settings.Address)) {}

    public RemoteConfigurationMessage(IHttpClientFactory clients, IExceptionLogger logger, Uri address) : base(logger)
    {
        _clients = clients;
        _address = address;
    }

    public async ValueTask<HttpResponseMessage> Get(CancellationToken parameter)
    {
        using var client = _clients.CreateClient();
        var       result = await client.GetAsync(_address, parameter).Off();
        return result;
    }

    public bool Get(None parameter) => true;
}