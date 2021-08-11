using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Google.Claims
{
	public sealed class PictureClaimAction : ClaimAction
	{
		public static PictureClaimAction Default { get; } = new PictureClaimAction();

		PictureClaimAction() : base(Picture.Default, "picture") {}
	}
}