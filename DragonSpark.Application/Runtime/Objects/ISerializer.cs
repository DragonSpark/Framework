using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public interface ISerializer<T>
{
    IFormatter<T> Format { get; }
    IParser<T> Parse { get; }
}