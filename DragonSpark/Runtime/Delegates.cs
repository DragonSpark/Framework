using System;

namespace DragonSpark.Runtime
{
	public static class Delegates<T>
	{
		public static Action<T> Empty { get; } = t => {};

		public static Func<T, T> Self { get; } = t => t;
	}

	public static class Delegate
	{
		public static Func<T, T> Self<T>() => Delegates<T>.Self;

		public static Action<T> Empty<T>() => Delegates<T>.Empty;
	}

	public static class Delegates
	{
		public static Action Empty { get; } = () => {};
	}
}