using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

public interface IDeviceRegistry : IStopAware<string, DeviceRecord?>;