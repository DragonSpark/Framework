namespace DragonSpark.Server.Mobile.Notifications;

public sealed record NotificationHubSettings
{
    public required string Name { get; set; }
    public required string Client { get; set; }
    public required string Server { get; set; }
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromDays(2);
}