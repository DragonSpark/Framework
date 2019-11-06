using DragonSpark.Runtime;
using System;

namespace DragonSpark.Model.Selection
{
	public interface ISelect<in TIn, out TOut>
	{
		TOut Get(TIn parameter);
	}

	public interface ISelect<out T> : ISelect<None, T> {}

	public interface ISelect<in T, in TIn, out TOut> : ISelect<T, Func<TIn, TOut>> {}

	public class Select<T, TIn, TOut> : Select<T, Func<TIn, TOut>>, ISelect<T, TIn, TOut>
	{
		public Select(ISelect<T, Func<TIn, TOut>> select) : this(select.Get) {}

		public Select(Func<T, Func<TIn, TOut>> select) : base(select) {}
	}
}