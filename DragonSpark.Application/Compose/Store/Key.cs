using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Compose.Store
{
	sealed class Key<T> : ISelect<T, string>
	{
		readonly string          _prefix;
		readonly char            _delimiter;
		readonly Func<T, string> _key;

		public Key(string prefix, Func<T, string> key) : this(prefix, KeyDelimiter.Default, key) {}

		public Key(string prefix, char delimiter, Func<T, string> key)
		{
			_prefix    = prefix;
			_delimiter = delimiter;
			_key       = key;
		}

		public string Get(T parameter) => $"{_prefix}{_delimiter.ToString()}{_key(parameter)}";
	}
}