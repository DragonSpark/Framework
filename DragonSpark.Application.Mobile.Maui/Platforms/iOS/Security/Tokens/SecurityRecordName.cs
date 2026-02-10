using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class SecurityRecordName : Text.Text
{
    public static SecurityRecordName Default { get; } = new();

    SecurityRecordName() : base($"{NSBundle.MainBundle.BundleIdentifier}.devicekey") {}
}