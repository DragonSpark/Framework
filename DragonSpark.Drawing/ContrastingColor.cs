using DragonSpark.Text;
using System.Drawing;

namespace DragonSpark.Drawing;

public sealed class ContrastingColor : IFormatter<Color>
{
	public static ContrastingColor Default { get; } = new();

	ContrastingColor() : this("#ffffff", "#212529") {}

	readonly string _light, _dark;

	public ContrastingColor(string light, string dark)
	{
		_light = light;
		_dark  = dark;
	}

	public string Get(Color parameter)
	{
		if (parameter.A >= 128)
		{
			var r = parameter.R / 255.0;
			var g = parameter.G / 255.0;
			var b = parameter.B / 255.0;

			r = r <= 0.03928 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
			g = g <= 0.03928 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
			b = b <= 0.03928 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

			var luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;
			if (luminance < 0.35)
			{
				return _light;
			}
		}

		return _dark;
	}
}