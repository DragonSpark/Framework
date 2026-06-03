using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public interface IOutputKey : IText
{
	string Name { get; }
}

public interface IOutputKey<in T> : IFormatter<T>, IOutputKey;