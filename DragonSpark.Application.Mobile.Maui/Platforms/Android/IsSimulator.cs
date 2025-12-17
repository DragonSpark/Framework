using DragonSpark.Application.Mobile.Maui.Device;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class IsSimulator : AnyCondition<None>, IIsSimulator
{
    public static IsSimulator Default { get; } = new();

    IsSimulator()
        : base(IsFingerprint.Default, IsModel.Default, IsManufacturer.Default, IsProduct.Default, IsHardware.Default) {}
}