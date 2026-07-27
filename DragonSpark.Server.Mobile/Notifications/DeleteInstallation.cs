using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class DeleteInstallation : IStopAware<string>
{
    readonly NotificationHubClient _client;

    public DeleteInstallation(NotificationHubClients clients) : this(clients.Server) {}

    public DeleteInstallation(NotificationHubClient client) => _client = client;

    public ValueTask Get(Stop<string> parameter)
    {
        var (subject, stop) = parameter;
        return _client.DeleteInstallationAsync(subject, stop).ToOperation();
    }
}