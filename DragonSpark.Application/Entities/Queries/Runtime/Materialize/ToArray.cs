using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

class ToArray<T> : Selecting<IQueryable<T>, Array<T>>, IToArray<T>
{
	public ToArray(ISelect<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}

	public ToArray(Func<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}
}