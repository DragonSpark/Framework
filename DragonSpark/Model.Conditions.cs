using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection;
using System.Runtime.CompilerServices;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISelect<_, ICondition<T>> AsDefined<_, T>(this ISelect<_, ICondition<T>> @this) => @this;

		public static ICondition<T> Equal<T>(this T @this) => I.A<Equals<T>>().From(@this);

		public static ICondition<T> Not<T>(this T @this) => @this.Equal().Then().Inverse().Out();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSatisfiedBy<T>(this IConditionAware<T> @this, in T parameter)
			=> @this.Condition.Get(parameter);
	}
}