namespace DragonSpark.Application.Mobile.Uno.Device.Notifications;

public sealed class MessageKey : Text.Text
{
    public static MessageKey Default { get; } = new();

    MessageKey() : base("message") {}
}
