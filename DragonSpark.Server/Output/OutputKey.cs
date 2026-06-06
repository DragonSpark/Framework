namespace DragonSpark.Server.Output;

public class OutputKey<T> : OutputKey, IOutputKey<T>
{
	readonly string _key;

	protected OutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected OutputKey(string name, string key) : base(name) => _key = key;

	public string Get(T parameter) => $"{_key}:{parameter}";
}

public class OutputKey : Text.Text, IOutputKey
{
	protected OutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected OutputKey(string name, string key) : base(key) => Name = name;

	public string Name { get; }
}