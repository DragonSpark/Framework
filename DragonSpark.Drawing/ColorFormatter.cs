using System.Drawing;
using DragonSpark.Text;

namespace DragonSpark.Drawing;

public sealed class ColorFormatter : Formatter<Color>
{
	public static ColorFormatter Default { get; } = new();

	ColorFormatter() : base(x => $"#{x.R:X2}{x.G:X2}{x.B:X2}{x.A:X2}") {}
}