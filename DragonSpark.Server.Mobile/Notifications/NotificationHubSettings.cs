using System;

namespace DragonSpark.Server.Mobile.Notifications;

public sealed record NotificationHubSettings
{
    public required string Name { get; set; }
    public required string Connection { get; set; }
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromDays(2);
}