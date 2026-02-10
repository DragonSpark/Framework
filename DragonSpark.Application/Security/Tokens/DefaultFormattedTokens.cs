using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class DefaultFormattedTokens : FixedSelection<byte, string>, IText
{
    public static DefaultFormattedTokens Default { get; } = new();

    DefaultFormattedTokens() : base(FormattedTokens.Default, 24) {}
}