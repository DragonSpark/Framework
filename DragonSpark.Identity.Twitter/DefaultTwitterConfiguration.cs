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
		parameter.AuthorizationEndpoint   = parameter.AuthorizationEndpoint.Replace("twitter.com", "x.com");
		parameter.TokenEndpoint           = parameter.TokenEndpoint.Replace("twitter.com", "x.com");
		parameter.UserInformationEndpoint = parameter.UserInformationEndpoint.Replace("twitter.com", "x.com");
		foreach (var fields in _fields)
		{
			parameter.UserFields.Add(fields);
		}
	}
}