using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public sealed class Adapter<TIn, T> : DragonSpark.Model.Selection.Select<TIn, IQueryable<T>>, IQuery<TIn, T>
	{
		public Adapter(ISelect<TIn, IQueryable<T>> select) : base(select) {}
	}
}