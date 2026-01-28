using System;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct DeviceUsedInput(string DeviceId, DateTime Now);