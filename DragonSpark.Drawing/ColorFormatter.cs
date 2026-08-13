using DragonSpark.Text;
using System.Drawing;

namespace DragonSpark.Drawing;

public sealed class ColorFormatter : Formatter<Color>
{
	public static ColorFormatter Default { get; } = new();

	// Standard CSS RGBA order for SfColorPicker!
	ColorFormatter() : base(x => $"#{x.R:X2}{x.G:X2}{x.B:X2}{x.A:X2}") {}
}