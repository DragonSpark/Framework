using DragonSpark.Model.Selection;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class Project<TIn, TOut> : Select<IQueryable<TIn>, IQueryable<TOut>>
	{
		public Project(Func<IQueryable<TIn>, IQueryable<TOut>> select) : base(select) {}
	}
}