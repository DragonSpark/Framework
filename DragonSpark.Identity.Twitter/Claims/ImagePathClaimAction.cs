using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class ImagePathClaimAction : SubKeyClaimAction
{
	public static ImagePathClaimAction Default { get; } = new();

	ImagePathClaimAction() : base(ImagePath.Default, new SubKey("data", "profile_image_url"), "url") {}
}