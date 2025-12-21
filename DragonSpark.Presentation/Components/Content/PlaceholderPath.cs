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
// TODO
public readonly record struct PlaceholderInput(ushort Width = 120, ushort Height = 80, string? Text = null);