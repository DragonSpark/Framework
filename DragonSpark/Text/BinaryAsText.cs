using System.Text;

namespace DragonSpark.Text;

public sealed class BinaryAsText : Formatter<byte[]>
{
    public static BinaryAsText Default { get; } = new();

    BinaryAsText() : this(Encoding.UTF8) {}

    public BinaryAsText(Encoding encoding) : base(encoding.GetString) {}
}