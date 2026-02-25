using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class DeviceAwareMarkUsed : DependingOnAll<Stop<string>>, IMarkUsed
{
    public DeviceAwareMarkUsed(IMarkUsed previous, IDeviceUsed device) : base(previous, device) {}
}