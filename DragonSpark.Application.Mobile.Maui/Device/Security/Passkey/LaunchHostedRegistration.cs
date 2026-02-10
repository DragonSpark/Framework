namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class LaunchHostedRegistration : LaunchHostedAddressBase, ILaunchHostedRegistration
{
    public LaunchHostedRegistration(HostedRegistrationAddress address) : base(address) {}
}