using AspNet.Security.OAuth.Twitter;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Identity.Twitter;

public sealed class DefaultTwitterConfiguration : ICommand<TwitterAuthenticationOptions>
{
	public static DefaultTwitterConfiguration Default { get; } = new();

	DefaultTwitterConfiguration()
		: this("url", "description", "verified", "location", "profile_image_url", "public_metrics", "entities") {}

	readonly Array<string> _fields;

	public DefaultTwitterConfiguration(params string[] fields) => _fields = fields;

	public void Execute(TwitterAuthenticationOptions parameter)
	{
		foreach (var fields in _fields)
		{
			parameter.UserFields.Add(fields);
		}
	}
}