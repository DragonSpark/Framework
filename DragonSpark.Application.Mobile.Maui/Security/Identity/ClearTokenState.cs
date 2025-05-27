namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class ClearTokenState : Communication.Http.Security.ClearTokenState
{
    public static ClearTokenState Default { get; } = new();

    ClearTokenState() : base(AccessTokenProcessValue.Default, AccessTokenViewStorage.Default) {}
}