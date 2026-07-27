namespace DragonSpark.Server.Mobile.Notifications;

public sealed record CleanUpSettings
{
    public TimeSpan TimerDuration { get; set; } = TimeSpan.FromDays(1);
    public byte BatchSize { get; set; } = byte.MaxValue;
}