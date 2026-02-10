namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class StoreAlias : Text.Text
{
    public static StoreAlias Default { get; } = new();

    StoreAlias() : base("dpop-device-key") {}
}