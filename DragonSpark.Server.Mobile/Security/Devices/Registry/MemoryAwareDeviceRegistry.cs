using System;
using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class MemoryAwareDeviceRegistry : StopAware<string, DeviceRecord?>, IDeviceRegistry
{
    public MemoryAwareDeviceRegistry(IDeviceRegistry previous, IMemoryCache memory)
        : base(previous.Then()
                       .Store()
                       .In(memory)
                       .For(TimeSpan.FromDays(1))
                       .Using(Key.Default.Then().Accept<Stop<string>>(x => x.Subject))) {}
}