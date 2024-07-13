using DragonSpark.Compose;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace DragonSpark.Drawing;

public static class Extensions
{
	public static Image ResizeToRatio(this Image @this, Image source) => ResizeToRatio(@this, source.Size);

	public static Image ResizeToRatio(this Image @this, Size size)
		=> Drawing.ResizeToRatio.Default.Parameter(new(@this, size)).Subject;

	public static Point Center(this Image @this, Image reference) => @this.Size.Center(reference);

	public static Point Center(this Size @this, Image reference) => Center(@this, reference.Bounds);

	public static Point Center(this Image @this, Rectangle size) => @this.Size.Center(size);

	public static Point Center(this Size @this, Rectangle size) => PositionCenter.Default.Get(new(@this, size));

	public static Size Size(this FontRectangle @this) => new((int)@this.Width, (int)@this.Height);
}