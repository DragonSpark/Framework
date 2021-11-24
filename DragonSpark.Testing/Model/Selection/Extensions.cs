using DragonSpark.Model.Selection;

namespace DragonSpark.Testing.Model.Selection;

public static class Extensions
{
	public static TOut Get<TIn, TOut>(this ISelect<TIn, TOut> @this, in TIn parameter) => @this.Get(parameter);
}