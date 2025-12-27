using DragonSpark.Compose;
using DragonSpark.Text;
using System.Net;

namespace DragonSpark.Presentation.Components.Content;

public sealed class PlaceholderPath : IFormatter<PlaceholderInput>
{
	public static PlaceholderPath Default { get; } = new();

	PlaceholderPath() {}

	public string Get(PlaceholderInput parameter)
	{
		var (width, height, text) = parameter;
		return $"https://placehold.co/{width}x{height}.png{(text.IsNullOrEmpty() ? string.Empty : $"?text={WebUtility.UrlEncode(text)}")}";
	}
}