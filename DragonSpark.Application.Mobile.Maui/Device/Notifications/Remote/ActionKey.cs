namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed class ActionKey : Text.Text
{
    public static ActionKey Default { get; } = new();

    ActionKey() : base("action") {}
}