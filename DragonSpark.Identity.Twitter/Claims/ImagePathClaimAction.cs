using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class ImagePathClaimAction : ClaimAction
	{
		public static ImagePathClaimAction Default { get; } = new ImagePathClaimAction();

		ImagePathClaimAction() : base(ImagePath.Default, "profile_image_url_https", "url") {}
	}
}