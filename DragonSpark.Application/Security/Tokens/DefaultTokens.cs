using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

sealed class DefaultTokens : FixedSelection<byte, string>, IText
{
    public static DefaultTokens Default { get; } = new();

    DefaultTokens() : base(Tokens.Default, 24) {}
}