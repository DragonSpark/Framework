using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class DisplayNameClaimAction : SubKeyClaimAction
	{
		public static DisplayNameClaimAction Default { get; } = new DisplayNameClaimAction();

		DisplayNameClaimAction() : base(DisplayName.Default, "profile", "real_name") {}
	}
}