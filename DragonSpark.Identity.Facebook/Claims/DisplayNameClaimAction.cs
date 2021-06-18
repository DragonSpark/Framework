using DragonSpark.Application.Security;

namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class DisplayNameClaimAction : ClaimAction
	{
		public static DisplayNameClaimAction Default { get; } = new DisplayNameClaimAction();

		DisplayNameClaimAction() : base(DisplayName.Default, "name") {}
	}
}