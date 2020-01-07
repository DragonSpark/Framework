using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Selection<TIn, TOut> : ISelect<Task<TIn>, Task<TOut>>
	{
		readonly Func<Task<TIn>, TOut> _select;

		public Selection(Func<TIn, TOut> select) : this(new Handle<TIn, TOut>(select).Get) {}

		public Selection(Func<Task<TIn>, TOut> select) => _select = select;

		public Task<TOut> Get(Task<TIn> parameter) => parameter.ContinueWith(_select);
	}
}