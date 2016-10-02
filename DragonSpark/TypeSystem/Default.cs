using System;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static T Value { get; } = (T)DefaultValues.Default.Get( typeof(T) );
	}

	public static class Defaults<T>
	{
		public static Type Type { get; } = typeof(T);

		public static TypeInfo Info { get; } = typeof(T).GetTypeInfo();
	}

	public static class Defaults
	{
		public static Type Void { get; } = typeof(void);
	}
}