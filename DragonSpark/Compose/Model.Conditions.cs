using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using System.Runtime.CompilerServices;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSatisfiedBy<T>(this IConditionAware<T> @this, in T parameter)
			=> @this.Condition.Get(parameter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSatisfiedBy(this IConditionAware<None> @this)
			=> @this.Condition.Get(None.Default);

	}
}