using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Runtime.Objects;

public class Copy<T> : IAlteration<T> where T : notnull
{
	readonly ISerializer<T> _serializer;

	protected Copy(ISerializer<T> serializer) => _serializer = serializer;

	public T Get(T parameter) => _serializer.Parser.Get(_serializer.Get(parameter));
}