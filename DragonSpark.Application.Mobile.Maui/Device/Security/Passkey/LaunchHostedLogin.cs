namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class LaunchHostedLogin : LaunchHostedAddressBase, ILaunchHostedLogin
{
    public LaunchHostedLogin(HostedLoginAddress address) : base(address) {}
}