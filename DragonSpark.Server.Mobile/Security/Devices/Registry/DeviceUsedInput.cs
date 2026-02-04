using System;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

public readonly record struct DeviceUsedInput(string DeviceId, DateTime Now);