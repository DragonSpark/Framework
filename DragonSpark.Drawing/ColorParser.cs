using DragonSpark.Text;
using System.Drawing;

namespace DragonSpark.Drawing;

public sealed class ColorParser : IParser<Color>
{
	public static ColorParser Default { get; } = new();

	ColorParser() {}

	public Color Get(string parameter)
	{
		var hex = parameter.TrimStart('#');
		var r   = Convert.ToByte(hex[..2], 16);
		var g   = Convert.ToByte(hex[2..4], 16);
		var b   = Convert.ToByte(hex[4..6], 16);
		var a   = hex.Length == 8 ? Convert.ToByte(hex[6..8], 16) : 255;
		return Color.FromArgb(a, r, g, b);
	}
}