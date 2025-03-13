namespace DragonSpark.Application.Mobile.Device.Notifications;

public sealed class TitleKey : Text.Text
{
    public static TitleKey Default { get; } = new();

    TitleKey() : base("title") {}
}