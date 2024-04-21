using DragonSpark.Application.Model;

namespace DragonSpark.Server.Output;

public class UserOutputKey : Text.Text, IUserOutputKey
{
	readonly string _key;

	protected UserOutputKey(string name) : this(name, name.ToLowerInvariant()) {}

	protected UserOutputKey(string name, string key) : base(name) => _key = key;

	//public string Get(T parameter) => $"{parameter.Get()}_{_key}";
	public string Get<T>(T parameter) where T : IUserIdentity => $"{parameter.Get()}_{_key}";
}