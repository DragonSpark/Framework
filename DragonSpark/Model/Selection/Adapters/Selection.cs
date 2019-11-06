using System;

namespace DragonSpark.Model.Selection.Adapters
{
	public sealed class Selection<TIn, TOut> : Select<TIn, TOut>
	{
		public Selection(ISelect<TIn, TOut> select) : base(select.Get) {}

		public Selection(Func<TIn, TOut> select) : base(select) {}

		public static implicit operator Selection<TIn, TOut>(TOut instance)
			=> new Selection<TIn, TOut>(new FixedResult<TIn, TOut>(instance));

		public static implicit operator Selection<TIn, TOut>(Func<TOut> instance)
			=> new Selection<TIn, TOut>(new DelegatedResult<TIn, TOut>(instance));

		public static implicit operator Selection<TIn, TOut>(Func<TIn, TOut> instance)
			=> new Selection<TIn, TOut>(instance);
	}
}