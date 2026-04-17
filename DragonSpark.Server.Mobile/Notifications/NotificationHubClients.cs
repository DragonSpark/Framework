using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

public sealed record NotificationHubClients(NotificationHubClient Server, NotificationHubClient Client);