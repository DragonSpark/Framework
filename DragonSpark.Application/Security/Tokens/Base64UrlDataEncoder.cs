using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class Base64UrlDataEncoder : Formatter<byte[]>
{
    public static Base64UrlDataEncoder Default { get; } = new();

    Base64UrlDataEncoder() : base(Base64EncodeData.Default.Then().Select(TokenFormatter.Default)) {}
}