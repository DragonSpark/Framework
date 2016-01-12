using System;
using DragonSpark.Aspects;

namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static Func<T, T> Self => t => t;

		[Freeze]
		public static T Item => DefaultFactory<T>.Instance.Create();

		[Freeze]
		public static T[] Items => DefaultFactory<T[]>.Instance.Create();
	}
}