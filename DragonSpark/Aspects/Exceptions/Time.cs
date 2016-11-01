using DragonSpark.Diagnostics.Exceptions;
using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects.Exceptions
{
	public static class Time
	{
		public static Func<int, TimeSpan> None { get; } = i => TimeSpan.Zero;

		public static Func<int, TimeSpan> Default { get; } = LinearRetryTime.Default.ToDelegate();
	}
}