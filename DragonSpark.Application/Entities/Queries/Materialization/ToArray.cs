using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	class ToArray<T> : Selecting<IQueryable<T>, Array<T>>, IToArray<T>
	{
		public ToArray(ISelect<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}

		public ToArray(Func<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}
	}

	public class ToArray<TIn, TEntity> : Materialize<TIn, TEntity, Array<TEntity>>
	{
		public ToArray(IQuery<TIn, TEntity> query) : this(query, DefaultToArray<TEntity>.Default) {}

		protected ToArray(IQuery<TIn, TEntity> query, IMaterializer<TEntity, Array<TEntity>> materializer)
			: base(query, materializer) {}
	}
}