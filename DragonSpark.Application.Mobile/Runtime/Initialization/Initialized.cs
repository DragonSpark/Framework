namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class Initialized : StateSwitch
{
    public static Initialized Default { get; } = new();

    Initialized() {}
}