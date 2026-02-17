using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class HashedBase64UrlData : Parser<byte[]>
{
    public static HashedBase64UrlData Default { get; } = new();

    HashedBase64UrlData() : base(Base64UrlData.Default.Then().Subject.Select(SHA256.HashData)) {}
}