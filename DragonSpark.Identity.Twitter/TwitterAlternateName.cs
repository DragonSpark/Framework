namespace DragonSpark.Identity.Twitter;

public sealed class TwitterAlternateName : Text.Text
{
	public static TwitterAlternateName Default { get; } = new();

	TwitterAlternateName() : base("X") {}
}