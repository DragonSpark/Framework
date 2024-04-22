using DragonSpark.Model;

namespace DragonSpark.Server.Output;

public class OutputKey : Text.Text, IOutputKey
{
	readonly string _key;

	protected OutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected OutputKey(string name, string key) : base(name) => _key = key;

	public string Get(None parameter) => _key;
}