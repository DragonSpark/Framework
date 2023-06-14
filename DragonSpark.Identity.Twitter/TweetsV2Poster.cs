using DragonSpark.Model.Operations.Allocated;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Web;

namespace DragonSpark.Identity.Twitter;

/// <summary>
/// Attribution: https://github.com/linvi/tweetinvi/issues/1147#issuecomment-1173174302
/// </summary>
public sealed class TweetsV2Poster : IAllocating<string, ITwitterResult>
{
	readonly ITwitterClient client;

	public TweetsV2Poster(ITwitterClient client) => this.client = client;

	public Task<ITwitterResult> Get(string parameter)
		=> client.Execute.AdvanceRequestAsync(x =>
		                                      {
			                                      var request = new TweetV2PostRequest { Text = parameter };
			                                      var body    = client.Json.Serialize(request);
			                                      x.Query.Url        = "https://api.twitter.com/2/tweets";
			                                      x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
			                                      x.Query.HttpContent =
				                                      new StringContent(body, Encoding.UTF8, "application/json");
		                                      });
}