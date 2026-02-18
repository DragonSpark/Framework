using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceAwareClearClientKey : DependingOnAll<Stop<None>>, IClearClientKey
{
    public DeviceAwareClearClientKey(IClearClientKey previous, IClearDeviceKey device) : base(previous, device) {}
}