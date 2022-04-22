using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public sealed class Handle<TIn, TOut> : ISelect<Task<TIn>, TOut>
{
	readonly Func<TIn, TOut> _select;

	public Handle(Func<TIn, TOut> select) => _select = select;

	// ReSharper disable once AsyncApostle.AsyncWait
	public TOut Get(Task<TIn> parameter) => _select(parameter.Result);
}