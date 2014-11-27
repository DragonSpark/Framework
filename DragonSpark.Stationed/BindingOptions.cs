﻿namespace DragonSpark.Stationed
{
	using System.Reflection;

	public static class BindingOptions
	{
		public static readonly BindingFlags
			AllProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
			PublicProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
	}
}
