using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Model
{
	public class Project<TIn, TOut> : DragonSpark.Model.Selection.Select<IQueryable<TIn>, IQueryable<TOut>>
	{
		protected Project(Func<IQueryable<TIn>, IQueryable<TOut>> select) : base(select) {}
	}
}