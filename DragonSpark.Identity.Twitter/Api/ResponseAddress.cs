using DragonSpark.Compose;
using DragonSpark.Text;
using System.Text.Json;
using Tweetinvi.Core.Web;

namespace DragonSpark.Identity.Twitter.Api;

/// <summary>
/// Attribution: https://github.com/linvi/tweetinvi/issues/1147#issuecomment-1638896277
/// </summary>
public sealed class ResponseAddress : IFormatter<ITwitterResponse>, IFormatter<ITwitterResult>
{
	public static ResponseAddress Default { get; } = new();

	ResponseAddress() : this(StatusAddressFormatter.Default) {}

	readonly IFormatter<ulong> _formatter;

	public ResponseAddress(IFormatter<ulong> formatter) => _formatter = formatter;

	public string Get(ITwitterResponse parameter)
		=> parameter.IsSuccessStatusCode
			   ? _formatter.Get(ulong.Parse(JsonSerializer.Deserialize<TwitterResponseDocument>(parameter.Content)
			                                              .Verify()
			                                              .Data.Id))
			   : string.Empty;

	public string Get(ITwitterResult parameter) => Get(parameter.Response);
}