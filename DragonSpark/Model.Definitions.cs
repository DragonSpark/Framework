using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<_, ICondition<T>> DefinedAsCondition<_, T>(this ISelect<_, ICondition<T>> @this) => @this;

		public static ISelect<TIn, IResult<TOut>> DefinedAsResult<TIn, TOut>(this ISelect<TIn, IResult<TOut>> @this) => @this;

		public static IResult<IResult<T>> DefinedAsResult<T>(this IResult<IResult<T>> @this) => @this;

		public static IResult<ISelect<TIn, TOut>> DefinedAsSelection<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this) => @this;
	}
}
