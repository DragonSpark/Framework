using System;

namespace DragonSpark.Runtime;

public static class Delegate
{
	public static Func<T, T> Self<T>() => Delegates<T>.Self;

	public static Action<T> Empty<T>() => Delegates<T>.Empty;
}