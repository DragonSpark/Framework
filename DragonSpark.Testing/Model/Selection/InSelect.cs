using DragonSpark.Model.Selection;

namespace DragonSpark.Testing.Model.Selection;

public class InSelect<TIn, TOut> : IInSelect<TIn, TOut>
{
	readonly In<TIn, TOut> _source;

	public InSelect(ISelect<TIn, TOut> select) : this(select.Get) {}

	public InSelect(In<TIn, TOut> select) => _source = select;

	public TOut Get(in TIn parameter) => _source(parameter);
}