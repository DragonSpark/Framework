﻿namespace DragonSpark.Compose.Extents;

public sealed class Extents : IExtents
{
	public static Extents Default { get; } = new();

	Extents() {}

	public Extent Of => Extent.Default;
}