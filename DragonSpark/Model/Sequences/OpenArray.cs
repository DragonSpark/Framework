using System;

namespace DragonSpark.Model.Sequences
{
	public class OpenArray<T> : IArray<T>
	{
		readonly Func<T[]> _result;

		public OpenArray(Func<T[]> result) => _result = result;

		public Array<T> Get() => _result();
	}

	public class OpenArray<TIn, TOut> : IArray<TIn, TOut>
	{
		readonly Func<TIn, TOut[]> _select;

		protected OpenArray(Func<TIn, TOut[]> select) => _select = @select;

		public Array<TOut> Get(TIn parameter) => _select(parameter);
	}
}