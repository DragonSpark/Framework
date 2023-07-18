using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.Web;

namespace DragonSpark.Identity.Twitter.Api;

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
		=> _execute.AdvanceRequestAsync(new ConfigurePoster(_json, parameter).Execute);
}