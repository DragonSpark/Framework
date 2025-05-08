namespace DragonSpark.Application.Mobile.Maui.Device.Notifications;

public sealed class MessageKey : Text.Text
{
    public static MessageKey Default { get; } = new();

    MessageKey() : base("message") {}
}
