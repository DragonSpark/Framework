using System.Security.Cryptography;
using DragonSpark.Compose;

namespace DragonSpark.Text;

public sealed class HashedText : Parser<byte[]>
{
    public static HashedText Default { get; } = new();

    HashedText() : base(TextAsData.Default.Then().Subject.Select(SHA256.HashData)) {}
}