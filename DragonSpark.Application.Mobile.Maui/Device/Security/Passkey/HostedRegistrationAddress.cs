using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class HostedRegistrationAddress : HostedAddressBase
{
    public HostedRegistrationAddress(PasskeyWorkflowSettings settings)
        : base(settings.Address.AppendPathSegment(settings.Register).ToUri()) {}
}