using DragonSpark.Model.Results;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class ComposeNotificationHubClients : Instance<NotificationHubClients>
{
    public ComposeNotificationHubClients(NotificationHubSettings settings)
        : base(new(new(settings.Server, settings.Name), new(settings.Client, settings.Name))) {}
}