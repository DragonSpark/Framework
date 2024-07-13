using SixLabors.ImageSharp.Formats.Png;

namespace DragonSpark.Drawing;

public sealed class DefaultPngEncoder : Model.Results.Instance<PngEncoder>
{
	public static DefaultPngEncoder Default { get; } = new();

	DefaultPngEncoder() : base(new PngEncoder { ColorType = PngColorType.RgbWithAlpha }) {}
}