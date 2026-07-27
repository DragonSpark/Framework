namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public readonly record struct DeterminePasskeySupportViaBrowserInput(
    WebView Subject,
    TaskCompletionSource<bool> Source);