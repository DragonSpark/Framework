using Tweetinvi;
using Tweetinvi.Models;

namespace DragonSpark.Identity.Twitter.Api;

sealed class TwitterClients : Model.Results.Instance<TwitterClient>
{
	public TwitterClients(TwitterApiSettings settings)
		: this(new ReadOnlyConsumerCredentials(settings.Key, settings.Secret, settings.Bearer)) {}

	public TwitterClients(ReadOnlyConsumerCredentials credentials) : base(new TwitterClient(credentials)) {}

}