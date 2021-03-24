using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Querying<T, TResult> : Selecting<IQueryable<T>, TResult>, IQuerying<T, TResult>
	{
		public Querying(ISelect<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}

		public Querying(Func<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}
	}
}