using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

public sealed class DefaultTemperature : Instance<double>
{
    public static DefaultTemperature Default { get; } = new();

    DefaultTemperature() : base(0.7) {}
}