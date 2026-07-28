using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public class OutputKey<T> : OutputKey, IOutputKey<T> where T : notnull
{
	readonly string                              _key;
	readonly IFormatter<OutputKeyFormatterInput> _formatter;

	public OutputKey(string name) : this(name, name.ToLowerInvariant(), OutputKeyFormatter.Default) {}

	protected OutputKey(string name, string key, IFormatter<OutputKeyFormatterInput> formatter) : base(name)
	{
		_key       = key;
		_formatter = formatter;
	}

	public string Get(T parameter) => _formatter.Get(new(_key, parameter.ToString().Verify()));
}

public class OutputKey : Text.Text, IOutputKey
{
	protected OutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected OutputKey(string name, string key) : base(key) => Name = name;

	public string Name { get; }
}