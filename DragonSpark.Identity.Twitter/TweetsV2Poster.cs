﻿using DragonSpark.Model.Operations.Allocated;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.Web;

namespace DragonSpark.Identity.Twitter;

/// <summary>
/// Attribution: https://github.com/linvi/tweetinvi/issues/1147#issuecomment-1173174302
/// </summary>
public sealed class TweetsV2Poster : IAllocating<string, ITwitterResult>
{
	readonly IExecuteClient _execute;
	readonly IJsonClient    _json;

	public TweetsV2Poster(ITwitterClient client) : this(client.Execute, client.Json) {}

	public TweetsV2Poster(IExecuteClient execute, IJsonClient json)
	{
		_execute = execute;
		_json    = json;
	}

	public Task<ITwitterResult> Get(string parameter)
		=> _execute.AdvanceRequestAsync(x =>
		                                {
			                                var request = new TweetV2PostRequest { Text = parameter };
			                                var body    = _json.Serialize(request);
			                                x.Query.Url        = "https://api.twitter.com/2/tweets";
			                                x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
			                                x.Query.HttpContent =
				                                new StringContent(body, Encoding.UTF8, "application/json");
		                                });
}