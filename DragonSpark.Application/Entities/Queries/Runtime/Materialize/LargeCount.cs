using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

class LargeCount<T> : Materializer<T, ulong>, ILargeCount<T>
{
	public LargeCount(ISelect<IQueryable<T>, ValueTask<ulong>> select) : base(select) {}

	public LargeCount(Func<IQueryable<T>, ValueTask<ulong>> select) : base(select) {}
}