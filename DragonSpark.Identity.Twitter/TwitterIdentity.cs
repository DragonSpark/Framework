using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters.V2;

namespace DragonSpark.Identity.Twitter
{
	sealed class TwitterIdentity : ITwitterIdentity
	{
		readonly AwaitOf<TwitterClient> _client;
		readonly HashSet<string>        _fields;
		readonly HashSet<string>        _empty;

		public TwitterIdentity(AuthenticatedTwitterClient client)
			: this(client.Await, new HashSet<string> { UserResponseFields.User.Id }, new HashSet<string>()) {}

		public TwitterIdentity(AwaitOf<TwitterClient> client, HashSet<string> fields, HashSet<string> empty)
		{
			_client = client;
			_fields = fields;
			_empty  = empty;
		}

		public async ValueTask<string?> Get(string parameter)
		{
			var parameters = new GetUserByNameV2Parameters(parameter)
			{
				Expansions  = _empty,
				TweetFields = _empty,
				UserFields  = _fields
			};
			var client   = await _client();
			var response = await client.UsersV2.GetUserByNameAsync(parameters).ConfigureAwait(false);
			var result   = response?.User?.Id;
			return result;
		}
	}
}