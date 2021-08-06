using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	class ToList<T> : Materializer<T, List<T>>, IToList<T>
	{
		public ToList(ISelect<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}

		public ToList(Func<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}
	}

	public class ToList<TIn, TEntity> : Materialize<TIn, TEntity, List<TEntity>>
	{
		public ToList(IQuery<TIn, TEntity> query) : this(query, DefaultToList<TEntity>.Default) {}

		protected ToList(IQuery<TIn, TEntity> query, IMaterializer<TEntity, List<TEntity>> materializer)
			: base(query, materializer) {}
	}
}