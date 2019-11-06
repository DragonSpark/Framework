using System;
using System.Reflection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Compose
{
	public static class A
	{
		public static T Default<T>() => Model.Results.Default<T>.Instance.Get();

		public static T Of<T>() => Start.An.Instance<T>();

		public static T This<T>(T instance) => instance;

		public static Type Type<T>() => Reflection.Types.Type<T>.Instance;

		public static TypeInfo Metadata<T>() => Reflection.Types.Type<T>.Metadata;

		public static IAlteration<T> Self<T>() => Model.Selection.Self<T>.Default;
	}
}