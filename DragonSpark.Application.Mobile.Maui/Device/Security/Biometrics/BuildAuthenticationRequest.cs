using DragonSpark.Model.Results;
using Plugin.Maui.Biometric;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Biometrics;

public class BuildAuthenticationRequest : Instance<AuthenticationRequest>
{
    // ReSharper disable once TooManyDependencies
    protected BuildAuthenticationRequest(string title, string subtitle, string cancel = "Cancel",
                                         AuthenticatorStrength level = AuthenticatorStrength.Strong)
        : base(new()
        {
            Title = title, Subtitle = subtitle, NegativeText = cancel, AuthStrength = level
        }) {}
}