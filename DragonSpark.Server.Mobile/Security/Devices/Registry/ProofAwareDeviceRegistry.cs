using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ProofAwareDeviceRegistry : StopAwareMaybe<string, DeviceRecord>, IDeviceRegistry
{
    public ProofAwareDeviceRegistry(IDeviceRegistry previous, ConstructDeviceFromRequest request)
        : base(previous, request) {}
}