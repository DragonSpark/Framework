using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

public sealed class MaximumTokenCount : Instance<ushort>
{
    public static MaximumTokenCount Default { get; } = new();

    MaximumTokenCount() : base(500) {}
}