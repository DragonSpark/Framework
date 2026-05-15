using SixLabors.Fonts;

namespace DragonSpark.Drawing;

public class TextSize : ITextSize
{
	readonly TextOptions _options;

	protected TextSize(string family, byte size = 16)
		: this(new(SystemFonts.Get(family).CreateFont(size)) { Dpi = 72, KerningMode = KerningMode.Auto }) {}

	protected TextSize(TextOptions options) => _options = options;

	public TextSizeResult Get(string parameter)
	{
		return new TextSizeResult(_options.Font, TextMeasurer.Measure(parameter, _options).Bounds.Size());
	}
}