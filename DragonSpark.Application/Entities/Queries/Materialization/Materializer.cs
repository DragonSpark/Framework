using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class Materializer<T, TResult> : Selecting<IQueryable<T>, TResult>, IMaterializer<T, TResult>
	{
		protected Materializer(ISelect<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}

		protected Materializer(Func<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}
	}
}