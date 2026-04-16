namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class Initializing : StateSwitch
{
    public static Initializing Default { get; } = new();

    Initializing() {}
}