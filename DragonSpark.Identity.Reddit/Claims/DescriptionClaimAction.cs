﻿using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Reddit.Claims;

public sealed class DescriptionClaimAction : SubKeyClaimAction
{
	public static DescriptionClaimAction Default { get; } = new();

	DescriptionClaimAction() : base(Description.Default, "subreddit", "public_description") {}
}