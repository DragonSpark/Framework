using DragonSpark.Application.Communication.Http.Security;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public sealed class HostedLoginAddress : HostedAddressBase
{
    public HostedLoginAddress(PasskeyWorkflowSettings settings, IAccessTokenStore token)
        : base(settings.Address.AppendPathSegment(settings.Login).ToUri(), token) {}
}