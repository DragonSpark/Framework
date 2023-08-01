using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client.V2;
using Tweetinvi.Parameters.V2;

namespace DragonSpark.Identity.Twitter.Api;

sealed class TwitterIdentity : ITwitterIdentity
{
	readonly IUsersV2Client  _client;
	readonly HashSet<string> _fields;
	readonly HashSet<string> _expansions;

	public TwitterIdentity(TwitterClient client) : this(client.UsersV2, new() { UserResponseFields.User.Id }, new()) {}

	public TwitterIdentity(IUsersV2Client client, HashSet<string> fields, HashSet<string> expansions)
	{
		_client     = client;
		_fields     = fields;
		_expansions = expansions;
	}

	public async ValueTask<string?> Get(string parameter)
	{
		var parameters = new GetUserByNameV2Parameters(parameter)
		{
			Expansions  = _expansions,
			TweetFields = _expansions,
			UserFields  = _fields
		};
		var response = await _client.GetUserByNameAsync(parameters).ConfigureAwait(false);
		var result   = response?.User?.Id;
		return result;
	}
}