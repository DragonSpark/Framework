using System;

namespace DragonSpark.Application.Security.Identity.Claims
{
	sealed class DeferredCopyClaims : CopyClaims
	{
		public DeferredCopyClaims(Func<IKnownClaims> claims) : base(new DeferredClaimNames(claims)) {}
	}
}