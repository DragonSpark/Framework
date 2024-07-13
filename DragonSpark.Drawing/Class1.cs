using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace DragonSpark.Drawing;

class Class1;

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

public sealed record FontDefinition(string Family, byte Size, byte Dpi = 72, KerningMode Mode = KerningMode.Auto);

public sealed class DefaultTextSize : TextSize
{
	public static DefaultTextSize Default { get; } = new();

	DefaultTextSize() : base("Arial") {}
}

public readonly record struct TextSizeResult(Font Font, Size Size);

public interface ITextSize : ISelect<string, TextSizeResult>;

public class TextSize : ITextSize
{
	readonly TextOptions _options;

	protected TextSize(string family, byte size = 16)
		: this(new(SystemFonts.Get(family).CreateFont(size)) { Dpi = 72, KerningMode = KerningMode.Auto }) {}

	protected TextSize(TextOptions options) => _options = options;

	public TextSizeResult Get(string parameter)
		=> new(_options.Font, TextMeasurer.MeasureSize(parameter, _options).Size());
}

public readonly record struct ResizeToRatioInput(Image Subject, Size Size);

sealed class ResizeToRatio : ICommand<ResizeToRatioInput>
{
	public static ResizeToRatio Default { get; } = new();

	ResizeToRatio() {}

	public void Execute(ResizeToRatioInput parameter)
	{
		var (subject, (width, height)) = parameter;
		var ratio = Math.Max(width, height).Real() / Math.Max(subject.Width, subject.Height);
		var size  = new Size((int)(subject.Width * ratio), (int)(subject.Height * ratio));
		subject.Mutate(x => x.Resize(size));
	}
}

public readonly record struct PositionCenterInput(Size Subject, Rectangle Area);

sealed class PositionCenter : ISelect<PositionCenterInput, Point>
{
	public static PositionCenter Default { get; } = new();

	PositionCenter() {}

	public Point Get(PositionCenterInput parameter)
	{
		var ((w, h), (x, y, width, height)) = parameter;
		return new(x + width / 2 - w / 2, y + height / 2 - h / 2);
	}
}