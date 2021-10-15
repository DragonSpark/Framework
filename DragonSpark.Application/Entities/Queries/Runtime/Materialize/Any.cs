using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

class Any<T> : Materializer<T, bool>, IAny<T>
{
	public Any(ISelect<IQueryable<T>, ValueTask<bool>> select) : base(select) {}

	public Any(Func<IQueryable<T>, ValueTask<bool>> select) : base(select) {}
}