using DragonSpark.Compose;
using DragonSpark.Text;
using System.Net;

namespace DragonSpark.Presentation.Components.Content;

public sealed class PlaceholderAlternatePath : IFormatter<PlaceholderInput>
{
	public static PlaceholderAlternatePath Default { get; } = new();

	PlaceholderAlternatePath() {}

	public string Get(PlaceholderInput parameter)
	{
		var (width, height, text) = parameter;
		return
			$"https://gifpng.com/{width}x{height}/{(text.IsNullOrEmpty() ? string.Empty : $"?text={WebUtility.UrlEncode(text)}")}";
	}
}