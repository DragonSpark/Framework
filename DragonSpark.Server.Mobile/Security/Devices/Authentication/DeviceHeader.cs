using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.Security.Tokens;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class DeviceHeader : Header
{
    public static DeviceHeader Default { get; } = new();

    DeviceHeader() : base(SchemeName.Default) {}
}