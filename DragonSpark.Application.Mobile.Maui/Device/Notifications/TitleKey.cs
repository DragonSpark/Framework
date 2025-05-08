namespace DragonSpark.Application.Mobile.Maui.Device.Notifications;

public sealed class TitleKey : Text.Text
{
    public static TitleKey Default { get; } = new();

    TitleKey() : base("title") {}
}