namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class DeviceClaimName : Text.Text
{
    public static DeviceClaimName Default { get; } = new();

    DeviceClaimName() : base("device_id") {}
}