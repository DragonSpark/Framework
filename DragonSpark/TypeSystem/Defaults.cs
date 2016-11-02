using System;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public static class Defaults<T>
	{
		public static Type Type { get; } = typeof(T);

		public static TypeInfo Info { get; } = typeof(T).GetTypeInfo();
	}
}