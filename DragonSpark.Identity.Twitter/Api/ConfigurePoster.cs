using DragonSpark.Model.Commands;
using System.Net.Http;
using System.Text;
using Tweetinvi.Client;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace DragonSpark.Identity.Twitter.Api;

sealed class ConfigurePoster : ICommand<ITwitterRequest>
{
	readonly StringContent _content;
	readonly HttpMethod    _method;
	readonly string        _address;

	public ConfigurePoster(IJsonClient client, string content)
		: this(client, new TweetV2PostRequest { Text = content }) {}

	public ConfigurePoster(IJsonClient client, TweetV2PostRequest request)
		: this(new StringContent(client.Serialize(request), Encoding.UTF8, "application/json")) {}

	public ConfigurePoster(StringContent content, HttpMethod method = HttpMethod.POST,
	                       string address = "https://api.twitter.com/2/tweets")
	{
		_content = content;
		_method  = method;
		_address = address;
	}

	public void Execute(ITwitterRequest parameter)
	{
		parameter.Query.Url         = _address;
		parameter.Query.HttpMethod  = _method;
		parameter.Query.HttpContent = _content;
	}
}