using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class Popped : Switch
{
    public static Popped Default { get; } = new();

    Popped() {}
}