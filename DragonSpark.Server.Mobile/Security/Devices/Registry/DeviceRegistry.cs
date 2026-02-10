using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class DeviceRegistry : EvaluateToSingleOrDefault<string, DeviceRecord>, IDeviceRegistry
{
    public DeviceRegistry(IScopes scopes) : base(scopes, SelectDeviceRecord.Default) {}
}