using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public interface ISerializer<T> : IFormatter<T> where T : notnull
{
	public IParser<T> Parser { get; }
	public ITarget<T> Target { get; }
}