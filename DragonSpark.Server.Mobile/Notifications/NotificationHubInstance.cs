using DragonSpark.Model.Results;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class NotificationHubInstance : Instance<NotificationHubClient>
{
    public NotificationHubInstance(NotificationHubSettings settings) : this(settings.Name, settings.Connection) {}

    public NotificationHubInstance(string name, string connection)
        : base(new(connection, name)) {}
}