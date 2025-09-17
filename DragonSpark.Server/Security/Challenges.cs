using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Server.Security;

public sealed class Challenges : Result<string>, IText
{
    public static Challenges Default { get; } = new();

    Challenges() : base(Base64Nonce.Default.Then().Bind(32).Get()) {}
}