﻿namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

public sealed class CopyKnownClaims : CopyClaims
{
	public CopyKnownClaims(IKnownClaims known) : base(new ClaimNames(known)) {}
}