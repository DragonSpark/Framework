﻿using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;
using Tweetinvi;

namespace DragonSpark.Identity.Twitter.Api;

sealed class AuthenticatedTwitterClient : IResulting<TwitterClient>
{
	readonly TwitterClient _client;

	public AuthenticatedTwitterClient(TwitterClient client) => _client = client;

	public async ValueTask<TwitterClient> Get()
	{
		var result = _client;
		await result.Auth.InitializeClientBearerTokenAsync().ConfigureAwait(false);
		return result;
	}
}