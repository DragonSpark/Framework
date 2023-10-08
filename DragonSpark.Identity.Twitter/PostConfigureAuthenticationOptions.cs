using AspNet.Security.OAuth.Twitter;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DragonSpark.Identity.Twitter;

public sealed class PostConfigureAuthenticationOptions : IPostConfigureOptions<TwitterAuthenticationOptions>
{
	readonly IDistributedCache _store;

	public PostConfigureAuthenticationOptions(IDistributedCache store) => _store = store;

	public void PostConfigure(string? name, TwitterAuthenticationOptions options)
	{
		options.StateDataFormat = new PropertiesFormatter(_store, options.StateDataFormat);
	}
}