using System;

namespace DragonSpark.TypeSystem
{
	public static class SpecialValues
	{
		// public static object Null { get; } = new object();

		public static T DefaultOrEmpty<T>() => Default<T>.Value;

		public static object DefaultOrEmpty( Type type ) => DefaultValues.Default.Get( type );
	}
}