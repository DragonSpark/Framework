using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class ImageClaimAction : ClaimAction
	{
		public static ImageClaimAction Default { get; } = new ImageClaimAction();

		ImageClaimAction() : base(Image.Default, "usericon") {}
	}
}