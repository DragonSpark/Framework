namespace DragonSpark.Application.Mobile.Device.Notifications;

public sealed class MessageKey : Text.Text
{
    public static MessageKey Default { get; } = new();

    MessageKey() : base("message") {}
}
