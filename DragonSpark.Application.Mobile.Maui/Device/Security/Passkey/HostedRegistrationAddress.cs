using DragonSpark.Application.Communication.Http.Security;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class HostedRegistrationAddress : HostedAddressBase
{
    public HostedRegistrationAddress(PasskeyWorkflowSettings settings, IAccessTokenStore token)
        : base(settings.Address.AppendPathSegment(settings.Register).ToUri(), token) {}
}