using Newtonsoft.Json;

namespace DragonSpark.Identity.Twitter;

/// <summary>
/// Attribution: https://github.com/linvi/tweetinvi/issues/1147#issuecomment-1173174302
/// </summary>
public class TweetV2PostRequest
{
	/// <summary>
	/// The text of the tweet to post.
	/// </summary>
	[JsonProperty("text")]
	public string Text { get; set; } = string.Empty;
}