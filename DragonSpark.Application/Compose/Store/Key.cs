using DragonSpark.Compose;
using DragonSpark.Text;
using System;

namespace DragonSpark.Application.Compose.Store;

public class Key<T> : IFormatter<T>
{
	readonly string          _prefix;
	readonly char            _delimiter;
	readonly Func<T, string> _key;

	public Key(Type prefix, Func<T, string> key) : this(prefix.AssemblyQualifiedName.Verify(), key) {}

	public Key(string prefix, Func<T, string> key) : this(prefix, KeyDelimiter.Default, key) {}

	public Key(string prefix, char delimiter, Func<T, string> key)
	{
		_prefix    = prefix;
		_delimiter = delimiter;
		_key       = key;
	}

	public string Get(T parameter) => $"{_prefix}{_delimiter}{_key(parameter)}";
}