using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System.Runtime.CompilerServices;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ICondition<T> Equal<T>(this T @this) => Compose.Start.An.Extent<Equals<T>>().From(@this);

		public static ICondition<T> Not<T>(this T @this) => @this.Equal().Then().Inverse().Out();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSatisfiedBy<T>(this IConditionAware<T> @this, in T parameter)
			=> @this.Condition.Get(parameter);
	}
}