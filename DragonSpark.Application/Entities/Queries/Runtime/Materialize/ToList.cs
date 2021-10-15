using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

class ToList<T> : Materializer<T, List<T>>, IToList<T>
{
	public ToList(ISelect<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}

	public ToList(Func<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}
}