using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class LinkClaimAction : ClaimAction
	{
		public static LinkClaimAction Default { get; } = new LinkClaimAction();

		LinkClaimAction() : base(Link.Default, "link") {}
	}
}