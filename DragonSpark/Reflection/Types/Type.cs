using System;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	public static class Type<T>
	{
		public static Type Instance { get; } = typeof(T);

		public static TypeInfo Metadata { get; } = typeof(T).GetTypeInfo();
	}
}