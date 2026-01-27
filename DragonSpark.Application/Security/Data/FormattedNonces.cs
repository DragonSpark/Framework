using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Data;

public sealed class FormattedNonces : Select<byte, string>
{
    public static FormattedNonces Default { get; } = new();

    FormattedNonces() : base(Nonces.Default.Then().Select(NonceFormatter.Default)) {}
}