using Microsoft.EntityFrameworkCore.Query;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct UpdateKeysInput(DeviceRecord Subject, UpdateSettersBuilder<DeviceKey> Builder);