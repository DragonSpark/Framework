using System.Text;

namespace DragonSpark.Text;

public sealed class TextAsBinary : Parser<byte[]>
{
    public static TextAsBinary Default { get; } = new();

    TextAsBinary() : this(Encoding.UTF8) {}
    
    public TextAsBinary(Encoding encoding) : base(encoding.GetBytes) {}
}