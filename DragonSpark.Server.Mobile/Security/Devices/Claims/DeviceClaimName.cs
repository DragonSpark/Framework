namespace DragonSpark.Server.Mobile.Security.Devices.Claims;

sealed class DeviceClaimName : Text.Text
{
    public static DeviceClaimName Default { get; } = new();

    DeviceClaimName() : base("device_id") {}
}