using System;
using DragonSpark.Composition.Compose;
using DragonSpark.Server.Mobile.Security.Devices;

namespace DragonSpark.Server.Mobile;

public static class Extensions
{
    public static BuildHostContext WithDeviceAuthorization(this BuildHostContext @this)
        => @this.WithDeviceAuthorization(_ => {});

    public static BuildHostContext WithDeviceAuthorization(this BuildHostContext @this, Action<DevicePoPOptions> configure)
        => @this.Configure(new Registrations(configure));
}