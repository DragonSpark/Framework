namespace DragonSpark.Server.Output;

public class UserOutputKey : OutputKey, IUserOutputKey
{
	readonly string _key;

	protected UserOutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected UserOutputKey(string name, string key) : base(name) => _key = key;

	public string Get(uint parameter) => $"{parameter}_{_key}";
}