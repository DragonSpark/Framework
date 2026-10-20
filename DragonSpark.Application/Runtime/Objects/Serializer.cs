using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public class Serializer<T> : Formatter<T>, ISerializer<T>
{
	protected Serializer(JsonSerializerOptions options)
		: this(new Serialize<T>(options),
		       new Parser<T>(new DefaultDeserialize<T>(options).Then().Select(x => x.Verify())),
		       new Target<T>(options)) {}

	protected Serializer(IFormatter<T> format, IParser<T> parser, ITarget<T> target) : base(format)
	{
		Parser = parser;
		Target = target;
	}

	public IParser<T> Parser { get; }
	public ITarget<T> Target { get; }
}