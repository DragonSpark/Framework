using DragonSpark.Application.Mobile.Maui.Device;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS;

sealed class IsSimulator : FixedResultCondition<None>, IIsSimulator
{
    public static IsSimulator Default { get; } = new();

    IsSimulator() : base(ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR) {}
}