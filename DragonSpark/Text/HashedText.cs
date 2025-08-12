using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class HashedText : Select<string, byte[]>
{
    public static HashedText Default { get; } = new();

    HashedText() : base(TextAsData.Default.Then().Subject.Select(SHA256.HashData)) {}
}