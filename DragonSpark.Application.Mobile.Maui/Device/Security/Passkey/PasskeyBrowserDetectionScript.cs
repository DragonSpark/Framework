namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class PasskeyBrowserDetectionScript : Text.Text
{
    public static PasskeyBrowserDetectionScript Default { get; } = new();

    PasskeyBrowserDetectionScript() 
        : base("(async () => { return ('credentials' in navigator) && typeof navigator.credentials.create === 'function' && typeof navigator.credentials.get === 'function'; })().then(r => window.result = r);") {}
}