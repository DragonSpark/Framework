namespace DragonSpark.Drawing;

public sealed class DefaultTextSize : TextSize
{
	public static DefaultTextSize Default { get; } = new();

	DefaultTextSize() : base("Arial") {}
}