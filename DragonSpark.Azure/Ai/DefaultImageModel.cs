namespace DragonSpark.Azure.Ai;

public sealed class DefaultImageModel : Text.Text
{
	public static DefaultImageModel Default { get; } = new();

	DefaultImageModel() : base("gpt-image-1.5") {}
}