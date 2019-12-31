﻿namespace DragonSpark.Compose
{
	public sealed class VowelContext
	{
		public static VowelContext Default { get; } = new VowelContext();

		VowelContext() {}

		public Extents.Extents Extent => Extents.Extents.Default;
	}
}