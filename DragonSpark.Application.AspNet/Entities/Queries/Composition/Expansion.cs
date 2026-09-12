using DragonSpark.Model.Selection;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public abstract class Expansion<TIn, TOut> : Projection<TIn, TOut>, ISelect<TIn, TOut>
{
	public static implicit operator Func<TIn, TOut>(Expansion<TIn, TOut> instance) => instance._select;

	public static implicit operator Expression<Func<TIn, TOut>>(Expansion<TIn, TOut> @this) => @this.Get();

	readonly Func<TIn, TOut> _select;

	protected Expansion(Expression<Func<TIn, TOut>> projection) : this(projection, projection.Compile()) {}

	protected Expansion(Expression<Func<TIn, TOut>> projection, Func<TIn, TOut> select) : base(projection)
		=> _select = select;

	public TOut Get(TIn parameter) => _select(parameter);
}