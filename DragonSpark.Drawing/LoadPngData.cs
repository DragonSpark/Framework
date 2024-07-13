namespace DragonSpark.Drawing;

public sealed class LoadPngData  :LoadImageData
{
	public static LoadPngData Default { get; } = new();

	LoadPngData() : base(DefaultPngEncoder.Default.Get()) {}
}