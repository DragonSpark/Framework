using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class Serializer<T> : ISerializer<T>
{
    public static Serializer<T> Default { get; } = new();

    Serializer()
        : this(DefaultSerialize<T>.Default,
               new Parser<T>(DefaultDeserialize<T>.Default.Then().Select(x => x.Verify()))) {}

    public Serializer(IFormatter<T> format, IParser<T> parse)
    {
        Format = format;
        Parse  = parse;
    }

    public IFormatter<T> Format { get; }
    public IParser<T> Parse { get; }
}