using DragonSpark.Compose;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace DragonSpark.Drawing;

public static class Extensions
{
	public static Image ResizeToRatioMaximum(this Image @this, Image source) => ResizeToRatioMaximum(@this, source.Size);

	public static Image ResizeToRatioMaximum(this Image @this, Size size)
		=> Drawing.ResizeToRatioMaximum.Default.Parameter(new(@this, size)).Subject;

	public static Image ResizeToRatioMinimum(this Image @this, Image source) => ResizeToRatioMinimum(@this, source.Size);

	public static Image ResizeToRatioMinimum(this Image @this, Size size)
		=> Drawing.ResizeToRatioMinimum.Default.Parameter(new(@this, size)).Subject;

	public static Image CloneToRatioMaximum(this Image @this, Image source) => ResizeToRatioMaximum(@this, source.Size);

	public static Image CloneToRatioMaximum(this Image @this, Size size)
		=> Drawing.CloneToRatioMaximum.Default.Get(new(@this, size));

	public static Image ConeToRatioMinimum(this Image @this, Image source) => ResizeToRatioMinimum(@this, source.Size);

	public static Image CloneToRatioMinimum(this Image @this, Size size)
		=> Drawing.CloneToRatioMinimum.Default.Get(new(@this, size));

	public static Point Center(this Image @this, Image reference) => @this.Size.Center(reference);

	public static Point Center(this Size @this, Image reference) => Center(@this, reference.Bounds);

	public static Point Center(this Image @this, Rectangle size) => @this.Size.Center(size);

	public static Point Center(this Size @this, Rectangle size) => PositionCenter.Default.Get(new(@this, size));

	public static Size Size(this FontRectangle @this) => new((int)@this.Width, (int)@this.Height);
	
	public static Color Convert(this System.Drawing.Color color) => Color.FromRgba(color.R, color.G, color.B, color.A);

}