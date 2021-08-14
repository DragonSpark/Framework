using System;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	sealed class DeferredCopyClaims : CopyClaims
	{
		public DeferredCopyClaims(Func<IKnownClaims> claims) : base(new DeferredClaimNames(claims)) {}
	}
}