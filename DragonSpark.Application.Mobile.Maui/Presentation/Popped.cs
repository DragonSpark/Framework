namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class Popped : DragonSpark.Model.Results.Switch
{
    public static Popped Default { get; } = new();

    Popped() {}
}