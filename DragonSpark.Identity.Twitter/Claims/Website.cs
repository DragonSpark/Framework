﻿namespace DragonSpark.Identity.Twitter.Claims;

public sealed class Website : TwitterClaim
{
	public static Website Default { get; } = new();

	Website() : base(nameof(Website).ToLower()) {}
}