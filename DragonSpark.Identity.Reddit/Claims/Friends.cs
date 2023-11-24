﻿namespace DragonSpark.Identity.Reddit.Claims;

public sealed class Friends : RedditClaim
{
	public static Friends Default { get; } = new();

	Friends() : base(nameof(Friends).ToLowerInvariant()) {}
}