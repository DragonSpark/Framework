using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection;

public sealed class Adapter<TIn, T> : Select<TIn, IQueryable<T>>, IQuery<TIn, T>
{
	public Adapter(ISelect<TIn, IQueryable<T>> select) : base(select) {}
}