using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Tokens;

public sealed class FormattedTokens : Select<byte, string>
{
    public static FormattedTokens Default { get; } = new();

    FormattedTokens() : base(Tokens.Default.Then().Select(TokenFormatter.Default)) {}
}