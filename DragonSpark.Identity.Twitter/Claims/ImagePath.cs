namespace DragonSpark.Identity.Twitter.Claims;

public sealed class ImagePath : TwitterClaim
{
	public static ImagePath Default { get; } = new();

	ImagePath() : base(nameof(ImagePath).ToLower()) {}
}