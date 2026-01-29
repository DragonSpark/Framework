namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class KeyStoreName : Text.Text
{
    public static KeyStoreName Default { get; } = new();

    KeyStoreName() : base("AndroidKeyStore") {}
}