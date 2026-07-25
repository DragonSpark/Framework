using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Text;

namespace DragonSpark.Application.Compose.Store;

public class Key : Key<None>
{
	public Key(string prefix) : base(prefix, _ => string.Empty) {}

	protected Key(Type prefix) : base(prefix, _ => string.Empty) {}

	protected Key(string prefix, char delimiter) : base(prefix, delimiter, _ => string.Empty) {}
}

public class Key<T> : IFormatter<T>
{
	readonly string          _prefix;
	readonly char            _delimiter;
	readonly Func<T, string> _key;

	public Key(string prefix, Func<T, string> key) : this(prefix, KeyDelimiter.Default, key) {}

	protected Key(Type prefix) : this(prefix, x => x?.ToString() ?? string.Empty) {}

	protected Key(Type prefix, Func<T, string> key) : this(prefix.AssemblyQualifiedName.Verify(), key) {}

	protected Key(string prefix, char delimiter, Func<T, string> key)
	{
		_prefix    = prefix;
		_delimiter = delimiter;
		_key       = key;
	}

	public string Get(T parameter) => $"{_prefix}{_delimiter}{_key(parameter)}";
}