using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Data;

public sealed class DefaultFormattedNonces : FixedSelection<byte, string>, IText
{
    public static DefaultFormattedNonces Default { get; } = new();

    DefaultFormattedNonces() : base(FormattedNonces.Default, 24) {}
}