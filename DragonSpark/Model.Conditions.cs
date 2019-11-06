using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISelect<_, ICondition<T>> AsDefined<_, T>(this ISelect<_, ICondition<T>> @this) => @this;

		public static ICondition<T> Equal<T>(this T @this) => I.A<EqualityCondition<T>>().From(@this);

		public static ICondition<T> Not<T>(this T @this) => @this.Equal().Then().Inverse().Get().ToCondition();
	}
}