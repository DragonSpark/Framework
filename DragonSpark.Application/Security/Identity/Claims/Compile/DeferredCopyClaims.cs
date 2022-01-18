using System;

namespace DragonSpark.Application.Security.Identity.Claims.Compile;

sealed class DeferredCopyClaims : CopyClaims
{
	public DeferredCopyClaims(Func<IKnownClaims> claims) : base(new DeferredClaimNames(claims)) {}
}
// TODO:

public sealed class CopyKnownClaims : CopyClaims
{
	public CopyKnownClaims(IKnownClaims known) : base(new ClaimNames(known)) {}
}

