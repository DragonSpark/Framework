using System;

namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static T Value { get; } = (T)DefaultValues.Default.Get( typeof(T) );
	}

	public static class Defaults
	{
		public static Type Void { get; } = typeof(void);
	}
}