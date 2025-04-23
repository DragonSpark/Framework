namespace DragonSpark.Application.Mobile.Uno.Device.Notifications;

public sealed class TitleKey : Text.Text
{
    public static TitleKey Default { get; } = new();

    TitleKey() : base("title") {}
}