using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.EntityFrameworkCore.Query;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

public readonly record struct UpdateKeysInput(DeviceRecord Subject, UpdateSettersBuilder<DeviceKey> Builder);