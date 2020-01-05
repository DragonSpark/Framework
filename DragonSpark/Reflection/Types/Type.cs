using System;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	// TODO:
	static class Type<T>
	{
		public static Type Instance { get; } = typeof(T);

		public static TypeInfo Metadata { get; } = typeof(T).GetTypeInfo();
	}
}