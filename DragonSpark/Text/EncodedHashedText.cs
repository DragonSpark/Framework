using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class EncodedHashedText : Select<string, byte[]>
{
    public static EncodedHashedText Default { get; } = new();

    EncodedHashedText() : base(EncodedTextAsData.Default.Then().Subject.Select(SHA256.HashData)) {}
}