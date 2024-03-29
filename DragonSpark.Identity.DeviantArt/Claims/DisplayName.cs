﻿namespace DragonSpark.Identity.DeviantArt.Claims;

public sealed class DisplayName : DeviantArtClaim
{
	public static DisplayName Default { get; } = new();

	DisplayName() : base(nameof(DisplayName).ToLower()) {}
}