using Tweetinvi;
using Tweetinvi.Models;

namespace DragonSpark.Identity.Twitter
{
	sealed class TwitterClients : Model.Results.Instance<TwitterClient>
	{
		public TwitterClients(TwitterApplicationSettings settings)
			: this(new ConsumerOnlyCredentials(settings.Key, settings.Secret)) {}

		public TwitterClients(ConsumerOnlyCredentials credentials) : this(new TwitterClient(credentials)) {}

		public TwitterClients(TwitterClient instance) : base(instance) {}
	}
}