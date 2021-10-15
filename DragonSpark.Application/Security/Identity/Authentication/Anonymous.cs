﻿namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class Anonymous : Text.Text
{
	public static Anonymous Default { get; } = new Anonymous();

	Anonymous() : base(nameof(Anonymous)) {}
}